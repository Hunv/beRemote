   M S B A M L   `   `   `  ����T  PbeRemote.Core.Definitions, Version=0.0.3.0, Culture=neutral, PublicKeyToken=nullxRclr-namespace:beRemote.Core.Definitions.Classes;assembly=beRemote.Core.Definitions!beRemote.Core.Definitions.Classes  5      & "beRemote.VendorProtocols.ietrident?   9beRemote.VendorProtocols.IETrident.IETridentSessionWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      fclassesRclr-namespace:beRemote.Core.Definitions.Classes;assembly=beRemote.Core.Definitions     5        � DataContext  5	      ��  �� RelativeSource �� *Self+.��5      �  � RowDefinitions 5   
   �� 5      $��26 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   (  "System.Windows.Controls.WebBrowser  -   5        $��WebView��5   6   $��2��6   $��1��6)   6E   �� 5   
    �� Orientation$ 
Horizontal=�6   $��0��6.   $��0��6;   .+�5      �� -   5        $��btnBack��5      $
��Left=�5      $��26��5      {�5      �� *	CanGoBack+��5      �� 5      $��Images/back_24.png��6   5!      �� 5#      -   5        $��btnNext��5'      $
��Left=�5$      $��26��5%      {�5&      �� *	CanGoNext+��5*      �� 5+      $��Images/forward_24.png��6   5-      5/      � 50   
   $��0��6   $��1��6    51      �� 52      $��* �6#   �� 53      $
��auto �6#   .+�56      �� -   5        $��0��57      $��Center=�59      ��58      �� *
WebAddress+59   3   �� 5<      -   5        $��
btnRefresh��5@      $��1��5=      $
��Left=�5>      $��26��5?      ��5C      �� 5D      $��Images/refresh_24.png��6   5F      5H      5J      5K      