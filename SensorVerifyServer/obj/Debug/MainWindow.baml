   M S B A M L   `   `   `  ����M  ISensorVerifyServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:SensorVerifyServerSensorVerifyServer  5   	   #    SensorVerifyServer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:SensorVerifyServer     5   	     � Title$  
MainWindow��5   	    � WindowState$ 	Maximized=�6   .��5
      �  � ColumnDefinitions 5   
   �� 5      �� 5      �� 5       � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $��* �6   �� 5      $��20 �6   .+�5   
   �� -   5         ��Name$ ScrollViewer1��5      $��1��6.   $��2��6<   $��0,0,0,0q�6L   $
��Auto=�6]   .��5      �� -   5         ��Name$ 
textBlock1��5      $��0,0,0,0q�6-   $��13(�6>   $
��Wrap=�6L   5      �� 5   
   -   5        $ 
textBlock2��5      $��0��6(   $��2��65   $��0,0,0,0q�6E   $��Console:��6V   $��Times New Roman)�6f   $��18(�6�   �� 5   
   -   5         ��Name$ button1��5      $��
Run Server��6"   $
��Left=�67   $	��Top=�6R   $��75��6j   6u   �� 5   
   -   5        $ button2��5      $��
Connect DB��6"   $
��Left=�67   $	��Top=�6R   $��75��6j   $��98,0,0,0q�6u   $��2��6�   6�   A� 5   
   -   5         A�Name$ Ellipse1��5      
���  ��$��8,0,0,0q�60   
���   �$��20��6P   $��Bottom=�6\    J� RenderTransformOrigin$ 3.176,0.506&�6w   $
��Left=�6�   $��20��6�   $��1��6�   A� 5   
   -   5        $ Ellipse2��5      $��1��6$   
���  ��$��72,0,0,0q�6?   
���   �$��20��6`   $	��Top=�6l   $ 3.176,0.506&�6�   $
��Left=�6�   $��20��6�   A� 5   
   -   5        $ Ellipse3��5      $��1��6$   
���  ��$��	133,0,0,0q�6?   
���   �$��20��6a   $	��Top=�6m   $ 3.176,0.506&�6�   $
��Left=�6�   $��20��6�   A� 5   
   -	   5        $ Ellipse4��5      $��1��6$   
���  ��$��	193,0,0,0q�6?   
���   �$��20��6a   $	��Top=�6m   $ 3.176,0.506&�6�   $
��Left=�6�   $��20��6�   �� 5   
   -
   5        $ button3��5      $��Train��6"   $
��Left=�62   $��0,0.333,0,0q�6M   $��2��6b   $	��Top=�6o   $��75��6�   6�   �� 5    
   -   5        $ button4��5       $��Result��6"   $��169,0.667,10,0q�63   $��2��6K   $	��Top=�6X   6p   �� 5!   
   -   5        	 ��Name$	 textBox��5!      $��1��6#   $
��Left=�64   $��18��6O   $��33,0,0,0q�6[   
 �� TextWrapping$

 Wrap=�6m   $��Server��6�   $	��Top=�6�   $��39��6�   $��10(�6�   $��Center=�6�   $��Center=�6�    �� 
IsReadOnly . 6  �� 5"   
   -   5        $	 textBox2��5"      $��1��6$   $
��Left=�64   $��18��6O   $��92,0,0,0q�6[   $

 Wrap=�6m   $��DB��6�   $	��Top=�6�   $��39��6�   $��10(�6�   $��Center=�6�   $��Center=�6�    . 6  �� 5#   
   -   5        $	 textBox3��5#      $��1��6$   $
��Left=�64   $��18��6O   $��	153,0,0,0q�6[   $

 Wrap=�6n   $	��SDR��6�   $	��Top=�6�   $��39��6�   $��10(�6�   $��Center=�6�   $��Center=�6�    . 6  �� 5$   
   -   5        $	 textBox4��5$      $��1��6$   $
��Left=�64   $��18��6O   $��	213,0,0,0q�6[   $

 Wrap=�6n   $	��SDS��6�   $	��Top=�6�   $��39��6�   $��10(�6�   $��Center=�6�   $��Center=�6�    . 6  � 5'   
   $��0,0,0,0.333q�6   $��1��6$   $��0��61    5(      �� 5)      �� 5*      .+�5/      [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 6.   7  1System.Windows.Forms.Integration.WindowsFormsHost  6   UWinForms@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     6.   $��0��6       Child. 50      %  System.Windows.Forms.PictureBox  -   5        Picturebox1��   ImageLocation$ 1.gif��50   ;      SizeMode$
 Zoom=�6Q   6c   51      Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 52   .     6   UWinForms@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     6.   $��1��6    . 53        -   5        Picturebox2��$ 21.gif��53   ;   $
 Zoom=�6R   6d   54      55      � 57   
   $��0,0,0,0.333q�6   $��1��6$   $��1��61   .+�5;      Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 6     6   UWinForms@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     6   . 5<        -   5        Picturebox3��$ 3.gif��5<   ;   $
 Zoom=�6Q   6c   5=      5>      5@      5A      