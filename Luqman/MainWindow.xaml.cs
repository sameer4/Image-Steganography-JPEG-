using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Text;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;

namespace Luqman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public FileDialog ImageFileDialog = new OpenFileDialog();
        public FileDialog SaveFile = new SaveFileDialog();
        public Bitmap coverimage;
        public string txtData;

        public MainWindow()
        {
            InitializeComponent();

        }


        private async void eImageBrowse(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageFileDialog.FileName = "";
                ImageFileDialog.Filter = "Image files (*.JPG)|*.JPG"; // add more formats
                                                                      //ImageFileDialog.InitialDirectory = environment.GetFolderPath(environment.SpecialFolder.MyDocuments);
                ImageFileDialog.ShowDialog();
                coverimage = new Bitmap(@"" + ImageFileDialog.FileName, true);
                etxtImagePath.Text = ImageFileDialog.FileName;
                etxtImagePath.TextWrapping = TextWrapping.NoWrap;
                eimage.Source = new BitmapImage(new Uri(etxtImagePath.Text));
            }
            catch (ArgumentException excep)
            { await this.ShowMessageAsync("Exception:", "Image not selected."); }
            catch (Exception excep)
            { await this.ShowMessageAsync("Exception:", excep.Message); }
        }

        private async void eTextData(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageFileDialog.FileName = "";
                ImageFileDialog.Filter = "Text files (*.txt)|*.txt";
                //ImageFileDialog.InitialDirectory = environment.GetFolderPath(environment.SpecialFolder.MyDocuments);
                ImageFileDialog.ShowDialog();
                FlowDocument myFlowDoc = new FlowDocument();
                myFlowDoc.Blocks.Add(new Paragraph(new Run(System.IO.File.ReadAllText(@"" + ImageFileDialog.FileName))));
                etxtMessage.Document = myFlowDoc;
                etxtMessage.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            catch (ArgumentException excep)
            { await this.ShowMessageAsync("Exception:", "Text File not selected."); }
            catch (Exception excep)
            { await this.ShowMessageAsync("Exception:", excep.Message); }
        }

        private async void bEmbed(object sender, RoutedEventArgs e)
        {
            txtData = new TextRange(etxtMessage.Document.ContentStart, etxtMessage.Document.ContentEnd).Text;

            if (etxtImagePath.Text.Length == 0 && txtData.Length == 0)
            {
                await this.ShowMessageAsync("Error:", "Image file and text data is not selected.");
                return;
            }
            else if (etxtImagePath.Text.Length == 0)
            {
                await this.ShowMessageAsync("Error:", "Image file and is not selected.");
                return;
            }
            else if (txtData.Length == 0)
            {
                await this.ShowMessageAsync("Error:", "Enter text to hide.");
                return;
            }
            try
            {
                var controller = await this.ShowProgressAsync("Please wait:", "Embedding...");
                
                BitMiracle.LibJpeg.Classic.jpeg_decompress_struct oJpegDecompress = new BitMiracle.LibJpeg.Classic.jpeg_decompress_struct();
                System.IO.FileStream oFileStreamImage = new System.IO.FileStream(etxtImagePath.Text, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                oJpegDecompress.jpeg_stdio_src(oFileStreamImage);
                oJpegDecompress.jpeg_read_header(true);
                BitMiracle.LibJpeg.Classic.jvirt_array<BitMiracle.LibJpeg.Classic.JBLOCK>[] DCTcoff = oJpegDecompress.jpeg_read_coefficients();

                int hightTemp = (int)Math.Ceiling(coverimage.Height / 8.0);
                int widthTemp = (int)Math.Ceiling(coverimage.Width / 8.0);
                int widthComponent1 = widthTemp % 2 == 0 ? widthTemp : widthTemp + 1;
                int heightComponent1 = hightTemp % 2 == 0 ? hightTemp : hightTemp + 1;
                int widthComponent2_3 = widthComponent1 / 2;
                int heightComponent2_3 = heightComponent1 / 2;

                string dataBitsToSave = string.Join("", Encoding.UTF8.GetBytes(txtData).Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))); dataBitsToSave += "00000000";
                Int32 number_SavedBits = 0;
                bool operationTerminator = false;


                for (int colorIterate = 0; colorIterate < 3 && !operationTerminator; colorIterate++)
                {
                    int height = colorIterate == 0 ? heightComponent1 : heightComponent2_3, width = colorIterate == 0 ? widthComponent1 : widthComponent2_3;
                    for (int heightIteratein2D = 0; heightIteratein2D < height && !operationTerminator; heightIteratein2D++)
                    {
                        for (int widthIteratein2D = 0; widthIteratein2D < width && !operationTerminator; widthIteratein2D++)
                        {
                            for (int cofficentNumber = 1; cofficentNumber < 64 && !operationTerminator; cofficentNumber++)
                            {
                                short cofficent = DCTcoff[colorIterate].Access(heightIteratein2D, 1)[0][widthIteratein2D][cofficentNumber];
                                if (number_SavedBits == dataBitsToSave.Length)
                                {
                                    operationTerminator = true;
                                }
                                else {
                                    if (!dataBitsToSave[number_SavedBits].ToString().Equals((cofficent & 1).ToString()))
                                    {
                                        DCTcoff[colorIterate].Access(heightIteratein2D, 1)[0][widthIteratein2D][cofficentNumber] = (short)(cofficent ^ 1);
                                    }
                                    number_SavedBits++;
                                }
                            }
                        }
                    }
                }


                oJpegDecompress.jpeg_finish_decompress();
                oFileStreamImage.Close();

                System.IO.FileStream objFileStreamMegaMap = System.IO.File.Create(@"" + etxtImagePath.Text.Substring(0, etxtImagePath.Text.LastIndexOf(".")) + "_steg.JPG");
                BitMiracle.LibJpeg.Classic.jpeg_compress_struct oJpegCompress = new BitMiracle.LibJpeg.Classic.jpeg_compress_struct();
                oJpegCompress.jpeg_stdio_dest(objFileStreamMegaMap);
                oJpegDecompress.jpeg_copy_critical_parameters(oJpegCompress);
                oJpegCompress.Image_height = coverimage.Height;
                oJpegCompress.Image_width = coverimage.Width;
                oJpegCompress.jpeg_write_coefficients(DCTcoff);
                oJpegCompress.jpeg_finish_compress();
                objFileStreamMegaMap.Close();
                oJpegDecompress.jpeg_abort_decompress();
                oFileStreamImage.Close();
                await controller.CloseAsync();
                await this.ShowMessageAsync("Embedding Successful:", "File is saved to orignal image file directory with _steg after the original name.");
            }
            catch (Exception excep)
            { await this.ShowMessageAsync("Exception:", excep.Message); }
            
        }

        private async void dIBrowse(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageFileDialog.FileName = "";
                ImageFileDialog.Filter = "Image files (*.JPG)|*.JPG"; // add more formats
                //ImageFileDialog.InitialDirectory = environment.GetFolderPath(environment.SpecialFolder.MyDocuments);
                ImageFileDialog.ShowDialog();
                coverimage = new Bitmap(@"" + ImageFileDialog.FileName, true);
                Console.WriteLine(ImageFileDialog.FileName);
                dtxtImagePath.Text = ImageFileDialog.FileName;
                dtxtImagePath.TextWrapping = TextWrapping.NoWrap;
                dimage.Source = new BitmapImage(new Uri(dtxtImagePath.Text));
            }
            catch (ArgumentException excep)
            { await this.ShowMessageAsync("Exception:", "Image not selected."); }
            catch (Exception excep)
            { await this.ShowMessageAsync("Exception:", excep.Message); }
        }

        private async void messageExtract(object sender, RoutedEventArgs e)
        {
            if (dtxtImagePath.Text.Length == 0)
            {
                await this.ShowMessageAsync("Error:", "Image file is not selected.");
                return;
            }
            try
            {
                var controller = await this.ShowProgressAsync("Please wait:", "Extracting...");
                BitMiracle.LibJpeg.Classic.jpeg_decompress_struct oJpegDecompress = new BitMiracle.LibJpeg.Classic.jpeg_decompress_struct();
                System.IO.FileStream oFileStreamImage = new System.IO.FileStream(dtxtImagePath.Text, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                oJpegDecompress.jpeg_stdio_src(oFileStreamImage);
                oJpegDecompress.jpeg_read_header(true);
                BitMiracle.LibJpeg.Classic.jvirt_array<BitMiracle.LibJpeg.Classic.JBLOCK>[] DCTcoff = oJpegDecompress.jpeg_read_coefficients();

                int hightTemp = (int)Math.Ceiling(coverimage.Height / 8.0);
                int widthTemp = (int)Math.Ceiling(coverimage.Width / 8.0);
                int widthComponent1 = widthTemp % 2 == 0 ? widthTemp : widthTemp + 1;
                int heightComponent1 = hightTemp % 2 == 0 ? hightTemp : hightTemp + 1;
                int widthComponent2_3 = widthComponent1 / 2;
                int heightComponent2_3 = heightComponent1 / 2;

                StringBuilder dataBitsRetreived = new StringBuilder();
                int number_RetrivedBits = 0;
                bool operationTerminator = false;


                for (int colorIterate = 0; colorIterate < 3 && !operationTerminator; colorIterate++)
                {
                    int height = colorIterate == 0 ? heightComponent1 : heightComponent2_3, width = colorIterate == 0 ? widthComponent1 : widthComponent2_3;
                    for (int heightIteratein2D = 0; heightIteratein2D < height && !operationTerminator; heightIteratein2D++)
                    {
                        for (int widthIteratein2D = 0; widthIteratein2D < width && !operationTerminator; widthIteratein2D++)
                        {
                            for (int l = 1; l < 64 && !operationTerminator; l++)
                            {
                                short cofficent = DCTcoff[colorIterate].Access(heightIteratein2D, 1)[0][widthIteratein2D][l];
                                dataBitsRetreived.Append(cofficent & 1);
                                if (dataBitsRetreived.Length % 8 == 0 && dataBitsRetreived.ToString(dataBitsRetreived.Length - 8, 8).Equals("00000000"))
                                {
                                    operationTerminator = true;
                                }
                                else {
                                    number_RetrivedBits++;
                                }
                            }
                        }
                    }
                }
                oJpegDecompress.jpeg_finish_decompress();
                oFileStreamImage.Close();

                string dataBitsToWrite = dataBitsRetreived.ToString(0, dataBitsRetreived.Length - 8);
                byte[] dataBytes = new byte[dataBitsToWrite.Length / 8];
                for (int i = 0; i + 8 <= dataBitsToWrite.Length; i = i + 8)
                {
                    dataBytes[i / 8] = Convert.ToByte(dataBitsToWrite.Substring(i, 8), 2);
                }
                output.Text = Encoding.UTF8.GetString(dataBytes);
                output.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                await controller.CloseAsync();
                await this.ShowMessageAsync("Extraction Successful:", "Text is displayed in the text box.");
            }
            catch (Exception excep)
            { await this.ShowMessageAsync("Exception:", excep.Message); }
        }

        private async void savtTextFile(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageFileDialog.FileName = "";
                SaveFile.Filter = "Text files (*.txt)|*.txt";
                SaveFile.ShowDialog();
                System.IO.File.WriteAllText(@"" + SaveFile.FileName, output.Text);
                await this.ShowMessageAsync("Saved Successfully:", "saved to " + SaveFile.FileName);
            }
            catch (ArgumentException excep)
            { await this.ShowMessageAsync("Exception:", "Select proper path and name."); }
            catch (Exception excep)
            { await this.ShowMessageAsync("Exception:", excep.Message); }
        }

        private async void about_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync("   Project Members", "Luqman Abdullah\t  2011-COE-01(M)\nSameer Azeem\t  2011-COE-28(M)");
        }
    }
}
