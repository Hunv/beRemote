   M S B A M L   `   `   `  ����T  PCefSharp.Wpf, Version=1.25.5.0, Culture=neutral, PublicKeyToken=40c4b6fc221f4138A0clr-namespace:CefSharp.Wpf;assembly=CefSharp.WpfCefSharp.Wpf  5      T PbeRemote.Core.Definitions, Version=0.0.3.0, Culture=neutral, PublicKeyToken=nullxRclr-namespace:beRemote.Core.Definitions.Classes;assembly=beRemote.Core.Definitions!beRemote.Core.Definitions.Classes 5      % !beRemote.VendorProtocols.Chromium=   7beRemote.VendorProtocols.Chromium.ChromiumSessionWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      EcefSharp0clr-namespace:CefSharp.Wpf;assembly=CefSharp.Wpf     5      fclassesRclr-namespace:beRemote.Core.Definitions.Classes;assembly=beRemote.Core.Definitions     5        � DataContext  5
      ��  �� RelativeSource �� *Self+.��5      �  � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
      CefSharp.Wpf.WebView  -   5          Name$ WebView��5      $��1��6,   $��2��69   6M   �� 5   
    �� Orientation$ 
Horizontal=�6   $��0��6.   $��0��6;   .+�5      �� -   5        $��btnBack��5      $
��Left=�5      $��26��5      {�5      �� *	CanGoBack+��5       �� 5!      $��Images/back_24.png��6   5#      �� 5%      -   5        $��btnNext��5)      $
��Left=�5&      $��26��5'      {�5(      �� *	CanGoNext+��5,      �� 5-      $��Images/forward_24.png��6   5/      51      � 52   
   $��0��6   $��1��6    53      �� 54      $��* �6#   �� 55      $
��auto �6#   .+�58      �� -   5        $��0��59      $��Center=�5;      ��5:      �� *
WebAddress+5;   3   �� 5>      -   5        $��
btnRefresh��5B      $��1��5?      $
��Left=�5@      $��26��5A      ��5E      �� 5F      $��Images/refresh_24.png��6   5H      5J      5K      5L      