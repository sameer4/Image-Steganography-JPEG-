   M S B A M L   `   `   `  ����A  =Luqman, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:LuqmanLuqman  5   	   T PMahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5	   	       Luqman.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Luqman     5   	   h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35c _Microsoft.Expression.Effects, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35?ee4http://schemas.microsoft.com/expression/2010/effects   5   	   LControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro   5	   	     � Title$  Steganography��5
   	   $��518.434��6   $	��525��60   
������� � 
ResizeMode$ CanMinimize=�5   	   (  "MahApps.Metro.Controls.MetroWindow   ShowMaxRestoreButton .     	GlowBrush
 ��� �$��1q�5   	    � WindowStartupLocation$ CenterScreen=�5   	      RightWindowCommands 5      +  %MahApps.Metro.Controls.WindowCommands  5   
   .@�5      �� -   5         ��Name$ about��5      $��About!��6$   66   5      .��5      � .+�5   
   �� -   5         ��Name$ 
tabControl��5      
����������	#FFFFFDFD$��489.101��6T   $��Bottom=�6e   
�������.@�5      �� -   5         ��Name$ 
tabEncrypt��5      $��Encrypt��6*   $
��Bold"�6;   ��6M   Z� ��6c   Z� .��5      � 
�������$��0,-1.5,-2,-1.833q�6*   	 � ColumnDefinitions	 5      �� 5      $
��144* �6+   �� 5      $
��367* �6+   .+�5       �� -   5        
 ��Name$
 label��5       $��Select Image��6+   $��30.666,11.373,31.461,0q�6B   $	��Top=�6b   $
��Bold"�6z   �� 5!      -   5        $
 label1��5!      $��Text to Hide��6,   $��30.666,42.333,34.651,0q�6C   $	��Top=�6c   $
��Bold"�6{   ��6�   Z� �� 5"      -   5         ��Name$ etxtImagePath��5"      $��23��65   $��13.833,15.373,138.334,0q�6A    �� TextWrapping$
 Wrap=�6b   $	��Top=�6v    J� RenderTransformOrigin$ 0.483,0.493&�6�   
�������$��1��6�    �� 
IsReadOnly . .��6�    J� Effect 5#      3  -System.Windows.Media.Effects.DropShadowEffect  5$      .��5%   *   5&      �� 5'      -   5         ��Name$ etxtMessage��5'   "   $��13.833,46.333,138.334,0q�67   $	��100��6X   $	��Top=�6e   $��1��6}    5(        5)      . �5+      /� 5,      �� 5-      -   5        $ eimport��5-      $��Import ��6.   $��Right=�6@   $��0,49.373,22.001,0q�6\   $	��Top=�6w   $��97��6�   $
��Bold"�6�      AccentedSquareButtonStyle#��[  6�   $��1��6�   6�   �� 5.      -	   5        $ ebrowse��5.      $��Browse��6.   $��Right=�6?   $	��Top=�6[   $��97��6s   $��0,15.373,22.001,0q�6~   $
��Bold"�6�   #��[  6�   $��1��6�   6�   �� 5/      -
   5        $ embed��5/      $��Embed��6,   $	��Top=�6<   $��
0,119,22,0q�6T   $
��Bold"�6h   #��[  6z   $��Right=�6�   $��97��6�   $��1��6�   6�   �� 50      -   5         ��Name$ eimage��50      $��Center=�6,   $	��269��6I   $��
16,168,6,0q�6V   $	��Top=�6j   $	��494��6�   $��2��6�   51      52      �� 53      -   5        $ 
tabDecrypt��53      $��Decrypt��6*   $
��Bold"�6;   .��54      � 
�������$��0,4.667,0,-3q�6*   .+�55      �� -   5        $
 label3��55      $��Select Image��6,   $
��Left=�6C   $��31.279,4.883,0,0q�6^   $	��Top=�6x   �� 56      -   5        $ output��56      $��10,0,11,44.333q�6.   $
 Wrap=�6F   ���	#FFD6F9EE$	��135��6q   $��Bottom=�6~    . 6�   �� 57      -   5        $ dImageBrowse��57      $��Browse��63   $	��Top=�6D   $��0,11.923,42.666,0q�6\   $��Right=�6w   $��75��6�   #��[  6�   6�   �� 58      -   5        $ dtxtImagePath��58      $��23��65   $
 Wrap=�6A   $	��Top=�6U   $��167.039,8.883,141.294,0q�6m    . .��6�    59        5:      .��5;   *   5<      �� 5=      -   5        $ extract��5=      $��Extract��6.   $	��Top=�6@   $��0,50.206,42.666,0q�6X   #��[  6s   $��Right=�6�   $��75��6�   6�   �� 5>      -   5        $ dimage��5>      $��Center=�6,   $	��175��6I   $��	23,83,0,0q�6V   $	��Top=�6i   $	��491��6�   �� 5?      -   5        $ dsave��5?      $
��Save��6,   $
��Left=�6;   $��224,407,0,0q�6V   $	��Top=�6k   $��75��6�   $��28��6�   #��[  6�   6�   5@      5A      5C      5E      5F      