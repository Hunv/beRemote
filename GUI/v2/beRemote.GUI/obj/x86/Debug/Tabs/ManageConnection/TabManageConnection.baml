   M S B A M L   `   `   `  ����P  LbeRemote.GUI.Controls, Version=0.0.3.0, Culture=neutral, PublicKeyToken=null\Bclr-namespace:beRemote.GUI.Controls;assembly=beRemote.GUI.ControlsbeRemote.GUI.Controls  5      T PbeRemote.Core.Definitions, Version=0.0.3.0, Culture=neutral, PublicKeyToken=nullxRclr-namespace:beRemote.Core.Definitions.Classes;assembly=beRemote.Core.Definitions!beRemote.Core.Definitions.Classes 5       beRemote.GUI<   6beRemote.GUI.Tabs.ManageConnection.TabManageConnection   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Rmy1Bclr-namespace:beRemote.GUI.Controls;assembly=beRemote.GUI.Controls     5      fclassesRclr-namespace:beRemote.Core.Definitions.Classes;assembly=beRemote.Core.Definitions     5      -   5        $	��100��5
      $	��420��6    /  )beRemote.Core.Definitions.Classes.TabBase    
IconSource$!  Images/usergreen_edit16.png��5       � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ Language/language.xaml?�6%   �� 5      $" Language/language.de-DE.xaml?�6%   �� 5      $" Language/language.es-ES.xaml?�6%   �� 5      $" Language/language.fr-FR.xaml?�6%   �� 5      $" Language/language.it-IT.xaml?�6%   �� 5      $" Language/language.nl-NL.xaml?�6%   �� 5      $" Language/language.pl-PL.xaml?�6%   �� 5      $" Language/language.ru-RU.xaml?�6%   �� 5      $" Language/language.zh-CN.xaml?�6%   �� 5      $" Language/language.cs-CZ.xaml?�6%   �� 5      $" Language/language.ar-SA.xaml?�6%   �� 5      $" Language/language.bg-BG.xaml?�6%   �� 5      $" Language/language.dk-DK.xaml?�6%   �� 5      $" Language/language.el-GR.xaml?�6%   �� 5      $" Language/language.fa-IR.xaml?�6%   �� 5       $" Language/language.fi-FI.xaml?�6%   �� 5!      $" Language/language.he-IL.xaml?�6%   �� 5"      $" Language/language.hi-IN.xaml?�6%   �� 5#      $" Language/language.hr-HR.xaml?�6%   �� 5$      $" Language/language.hu-HU.xaml?�6%   �� 5%      $" Language/language.ko-KR.xaml?�6%   �� 5&      $" Language/language.nn-NO.xaml?�6%   �� 5'      $" Language/language.se-SE.xaml?�6%   �� 5(      $" Language/language.tr-TR.xaml?�6%   �� 5)      $" Language/language.zh-CN.xaml?�6%   %    5+         Header 5-      �� 5.   
    �� ResourceKey$ TabTitleManageConnection��6   .��50      � .+�51   
   �� $
��Auto=�6   .��52      � -   5        $��
grdContent��52   .   $
��Left=�6   .+�53      �� $��10,10q�6   $	��Top=�6*   $
��Left=�6B   $	��380��6]   $	��210��6i   ��54      �� 55      -   5        $��	lblHostId��55        &  "TabManageConnectionHostinformation#��[  61   .��57      � .+�58      �� -   5        $��lblHostname��58   �    ! TabManageConnectionIpHostname#��[ 6    $��28��6Y   $
��Left=�6e   $��10,0,0,0q�6�   $	��Top=�6�   $	��115��6�   $��Right=�6�   �� 59      -   5        $��txtHostname��59   ]   $��23��6"   $
��Left=�6.   $��	125,2,0,0q�6J   $	��Top=�6p   $	��222��6�    �� 	MaxLength$	 250��6�    �� MaxLines$ 1��6�   $	��200��6�   6�   �� 5:      -   5        $��
lblHostNew��5:        " TabManageConnectionDisplayname#��[ 62   $��28��6l   $
��Left=�6x   $��	10,27,0,0q�6�   $	��Top=�6�   $	��115��6�   $��Right=�6�   �� 5;      -   5        $��txtName��5;   Q   $
��Left=�6"   $��
125,28,0,0q�6=   $	��222��6`   $��23��6l   $	��Top=�6x   $ 1��6�   $	 100��6�   $	��300��6�   6�   �� 5<      -   5        $��lblDescription��5<   �    " TabManageConnectionDescription#��[ 6    $��28��6Z   $
��Left=�6f   $��	10,53,0,0q�6�   $	��Top=�6�   $	��115��6�   $��Right=�6�   �� 5=      -	   5        $��txtDescription��5=   ^   $��23��6"   $
��Left=�6.   $��
125,54,0,0q�6J   $	��Top=�6t   $	��222��6�   $ 1��6�   $	 100��6�   $	��400��6�   �� 5>      -
   5        $��lblOperatingSystem��5>   �    & "TabManageConnectionOperatingSystem#��[ 6    $��28��6^   $
��Left=�6j   $��	10,80,0,0q�6�   $	��Top=�6�   $��Right=�6�   $	��115��6�   �� 5?      -   5        $��cmbOperatingSystem��5?   ^   $��23��6#   $
��Left=�6/   $��
125,80,0,0q�6J   $	��Top=�6x   $	��222��6�   $	��500��6�   �� 5@      -   5        $��	lblFolder��5@   �     TabManageConnectionFolder#��[ 6    $��28��6U   $
��Left=�6a   $��
10,106,0,0q�6|   $	��Top=�6�   $	��115��6�   $��Right=�6�   .   (beRemote.GUI.Controls.ConnectionComboBox  5A      -   5          Name$ 	ccbFolder��5A   m   $��23��61   $
��Left=�6=   $��125,106,0,0q�6X   $	��Top=�6�   $	��222��6�   $	��600��6�   	   AllowHostSelection	 .  
   AllowProtocolSelection
 .     InvalidBackgroundColor �	#FFFF555566  �� 5B      -   5        $��lblVpn��5B   �    $  TabManageConnectionVpnConnection#��[ 6    $��28��6\   $
��Left=�6h   $��
10,132,0,0q�6�   $	��Top=�6�   $	��115��6�   $��Right=�6�   �� 5C      -   5        $��cmbVpn��5C   _   $��23��6#   $
��Left=�6/   $��125,132,0,0q�6J   $	��Top=�6m   $	��222��6�   $	��600��6�    �� DisplayMemberPath$ Value��6�    �� SelectedValuePath$	 Key��6�   �� 5D      -   5        $��	chkPublic��5D   �    ' #TabManageConnectionPublicConnection#��[ 6#   $��16��6b   $
��Left=�6n   $��125,158,0,0q�6�   $	��Top=�6�   $	��225��6�   $	��700��6�   5E      5F      �� 5H      $��
10,225,0,0q�6   $	��Top=�6/   $
��Left=�6G   $	��380��6b   $��87��6n   ��5I      �� 5J       % !TabManageConnectionAddNewProtocol#��[ 6    .��5L      � .+�5M      ��  	 TabManageConnectionProtocol#��[	 6    $��28��6W   $
��Left=�6c   $��4,0,0,0q�6~   $	��Top=�6�   �� 5N      -   5        $��cmbProtocol��5N   B   $��23��6#   $��	67,0,10,0q�6/   $	��Top=�6U   $
��1200��6m   �� 5P      -   5        $��btnAdd��5P   �   $	��Top=�6!   $
��Left=�69   $	��100��6T   $��20��6`   $��	67,28,0,0q�6l    "
 TabManageConnectionAddProtocol#��[
 6   6�   5Q      5R      �� 5T      -   5        $��cmbExistingProtocols��5T   z   $
��Left=�6   $��24��66   $	��Top=�6B   $	��270��6Z   $��
510,25,0,0q�6f   6�   �� 5U       ( $TabManageConnectionAssignedProtocols#��[ 6   $��28��6X   $	��Top=�6d   $
��Left=�6|   $��
400,25,0,0q�6�   �� 5W      -   5        $��gbProtocols��5W   {   $��
400,55,0,0q�6   $	��Top=�6/   $
��Left=�6G   $	��380��6b   $	��237��6n   ��5X      �� 5Y       �� ContentStringFormat$ ID: {0}��6g   ��6    �� *SelectedItem.Tag+ �� ElementName$ cmbExistingProtocols��6g   .��5[      � -   5        $��grdProtocolsettings��5[      .+�5\      ��  �� Orientation$ Vertical=�6%   .+�5]      �� $ 
Horizontal=�6)   .+�5^   "   ��   TabManageConnectionPort#��[ 6(   $��28��6[   $
��Left=�6g   $	��Top=�6�   )   #beRemote.GUI.Controls.NumericUpDown  5_   "   -   5          Name$ nudPort��5_   [   $��25��64   $
��Left=�6@   $	��Top=�6l   $��80��6�      Maximum$ 65535��6�      Minimum$ 1��6�      Value$
 1024��6�   $
��1300��6�   5`      �� 5a      -   5        $��chkCredentials��5a   �    % !TabManageConnectionUseCredentials#��[ 6'   $��16��6d   $
��Left=�6p   $��5,5,0,0q�6�   $	��Top=�6�   $
��1000��6  �� 5b      -   5        $��cmbCredentials��5b   _   $��23��6'   $
��Left=�63   $��5,0,0,0q�6N   $	��Top=�6u   $	��353��6�   {�.  $
��1100��6�   �� 5c      -   5        $��btnAddCredentials��5c   %   $��5,0,0,0q�6>   $��20��6O   $	��100��6[   $	��Top=�6g   $
��Left=�6    $  TabManageConnectionAddCredential#��[ 6�   {�.  6�   �� 5e      -   5        $��spCredentials��5e   �   $
��Left=�6)   $	��Top=�6D   $	��5,0q�6\   $y�	Collapsed=�6i   .+�5f   "   �  � ColumnDefinitions 5g   &   �� 5h   *   $	��120 �6;   �� 5i   *   $��* �6;    � RowDefinitions 5k   &   �� 5l   *   $��* �68   �� 5m   *   $��* �68   �� 5n   *   $��* �68   �� 5o   *   $��* �68   �� 5p   *   $��* �68   .+�5r   &   ��  * &TabManageConnectionCredentialsUsername#��[ 6,   $��0��6n   $��0��6~   �� 5s   &   -   5        $��txtCredUser��5s   .   $��1��6A   $��0��6Q   $	��100��6^   $	��0,4q�6j   �� 5u   &    * &TabManageConnectionCredentialsPassword#��[ 6,   $��0��6n   $��1��6~   G� 5v   &   -   5        $��
pwCredPass��5v   2   $��1��6D   $��1��6T   $	��100��6a   $	��0,4q�6m   �� 5x   &    ( $TabManageConnectionCredentialsDomain#��[ 6,   $��0��6l   $��2��6|   �� 5y   &   -   5        $��
txtCredDom��5y   .   $��1��6@   $��2��6P   $	��100��6]   $	��0,4q�6i   �� 5{   &    - )TabManageConnectionCredentialsDescription#��[ 6,   $��0��6q   $��3��6�   �� 5|   &   -   5        $��txtCredDesc��5|   .   $��1��6A   $��3��6Q   $	��100��6^   $	��0,4q�6j   .��6x    � ToolTip 5}   *   _� 5~   .   .��5   2   �� .+�5�   6   ��  1 -TabManageConnectionCredentialsDescriptionText#��[ 6<   5�   3   5�   /   .��5�   ;   5�   '   �� 5�   &   -   5        $��0��5�   -   $��4��6=   $��2��6J    % !TabManageConnectionCredentialsAdd#��[ 6^   6�   5�   #   5�      �� 5�      $��28��6$   $
��Left=�60   $��5,0,0,0q�6K   $	��Top=�6\    ' #TabManageConnectionProtocolSettings#��[ 6t   .   (beRemote.GUI.Controls.ProtocolOptionGrid  5�      -    5          Name$ pogProtocolSettings��5�   5   $��24��6R   $��5,0,0,10q�6a   $
��Left=�6s   $	��353��6�   $
��1400��6�   5�      5�      5�      �� 5�      -!   5        $��btnSave��5�   �     TabManageConnectionSave#��[ 6   $��23��6L   $
��Left=�6X   $��10,320,0,273q�6s   $	��Top=�6�   $	��118��6�    �� 	IsDefault . $
��2000��6�   �� 5�      -"   5        $��	btnCancel��5�   �     TabManageConnectionCancel#��[ 6   $
��Left=�6N    .  $��138,320,0,273q�6{   $	��118��6�   $��23��6�   $	��Top=�6�    �� IsCancel . $
��2001��6�   5�      5�      5�      5�      