   M S B A M L   `   `   `  ����T  PbeRemote.Core.Definitions, Version=0.0.3.0, Culture=neutral, PublicKeyToken=nullxRclr-namespace:beRemote.Core.Definitions.Classes;assembly=beRemote.Core.Definitions!beRemote.Core.Definitions.Classes  5       beRemote.GUI<   6beRemote.GUI.Tabs.ManageCredential.TabManageCredential   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      fclassesRclr-namespace:beRemote.Core.Definitions.Classes;assembly=beRemote.Core.Definitions     5      -   5        $	��558��5	      $	��250��6   /   )beRemote.Core.Definitions.Classes.TabBase    
IconSource$  Images/key16.png��5       � 	Resources 5       �� MergedDictionaries �� Source   Flat �� Property �� Value �� Triggers P� Property P� Value  DataGridCellCentering	 �� 
TargetType*  $System.Windows.Controls.DataGridCell
 �� 
TargetType�� 5   
    5      �� 5      $ Language/language.xaml?�6%   �� 5      $" Language/language.de-DE.xaml?�6%   �� 5      $" Language/language.es-ES.xaml?�6%   �� 5      $" Language/language.fr-FR.xaml?�6%   �� 5      $" Language/language.it-IT.xaml?�6%   �� 5      $" Language/language.nl-NL.xaml?�6%   �� 5      $" Language/language.pl-PL.xaml?�6%   �� 5      $" Language/language.ru-RU.xaml?�6%   �� 5      $" Language/language.zh-CN.xaml?�6%   �� 5      $" Language/language.cs-CZ.xaml?�6%   �� 5      $" Language/language.ar-SA.xaml?�6%   �� 5      $" Language/language.bg-BG.xaml?�6%   �� 5      $" Language/language.dk-DK.xaml?�6%   �� 5      $" Language/language.el-GR.xaml?�6%   �� 5      $" Language/language.fa-IR.xaml?�6%   �� 5      $" Language/language.fi-FI.xaml?�6%   �� 5       $" Language/language.he-IL.xaml?�6%   �� 5!      $" Language/language.hi-IN.xaml?�6%   �� 5"      $" Language/language.hr-HR.xaml?�6%   �� 5#      $" Language/language.hu-HU.xaml?�6%   �� 5$      $" Language/language.ko-KR.xaml?�6%   �� 5%      $" Language/language.nn-NO.xaml?�6%   �� 5&      $" Language/language.se-SE.xaml?�6%   �� 5'      $" Language/language.tr-TR.xaml?�6%   �� 5(      $" Language/language.zh-CN.xaml?�6%   %9  &	        &	 k    �� 5,      .�5-      ��  � �� 67   Z� �� 5.       � �� 68   Z�  5/      P� 50       �@J�IsMouseOver . . �51      ��  � �� 6?   Z� �� 52       � �� 6@   Z� �� 53       � ��$
 Bold"�6?   54      P� 55       �@J�	IsFocused . . �56      ��  � ��$
 Bold"�6?   57      59      �� 5<      "	  62   .�5=      ��  � �� 5>      �� 5?      "
  6*   .R�5@      � #��z��6#   .+�5A   "   �� $��Center=�63   5B      5C      5E      5F      5G         Header 5I      �� 5J   
    �� ResourceKey$ TabTitleManageCredential��6   .��5M      � .+�5N   
   &   System.Windows.Controls.DataGrid  -   5        $��
dgExisting��5N   D      AutoGenerateColumns . $��12,162,12,12q�6.      CanUserAddRows .     	RowHeight$ 24Q�6m      VerticalGridLinesBrush �	#50000000   HorizontalGridLinesBrush �	#50000000   SelectionMode$ Single=�6�      	CellStyle# [ 6�    �� SelectedItem 69  �� *SelectedItem+ �� Mode$ TwoWay=�   Columns 5O      4  .System.Windows.Controls.DataGridTemplateColumn  5P      ,  &System.Windows.Controls.DataGridColumn   Header$ Password��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ 60 6;      
IsReadOnly .    CellTemplate 5Q      �� 5R      .R�5S      �� -   5        #��[  5S   %   .��5T   "   � .+�5U   &   �� $��0,0,0,0q�6H   $
��Left=�6Y   $	��Top=�6t   ��6,   �� *
KeyImage+6t   �� 5V   &   $��	16,-5,0,0q�6O   $
��Left=�6b   $	��Top=�6}   $��25��6�   ��6,   �� *PasswordStatus+6�   5W   #   5X      5Y      5[        5\      $ Delete��6)   $ 50 69    .  5]      �� 5^      .R�5_      �� -   5        #��[  5_   ?   .��5`   "   �� $2��,/Tabs/ManageCredential/Images/redcross16.png��6(   5a      5b      5d      5f      �� 5h   
   -   5        $��label1��5h   k   $��Existing Credentials:��6   $��28��60   $
��Left=�6<   $��
28,139,0,0q�6W   $	��Top=�6y   �� 5i   
   -   5        $��	groupBox1��5i   M   $��Add Credentials��6   $	��122��6,   $��
12,12,12,0q�69   $	��Top=�6^   .��5j      � .+�5k      �� -   5        $��label2��5k   c   $��Username��6   $��28��6+   $
��Left=�67   $��6,6,0,0q�6R   $	��Top=�6q   �� 5l      -   5        $��txtUsername��5l   S   $��23��6   $
��Left=�6&   $��6,29,0,0q�6A   $	��Top=�6f   $	��120��6~   $	��100��6�   �� 5m      -	   5        $��label3��5m   e   $��Password��6   $��28��6+   $
��Left=�67   $��	141,6,0,0q�6R   $	��Top=�6s   �� 5n      -
   5        $��label4��5n   n   $��Domain (optional)��6   $��28��64   $
��Left=�6@   $��	267,6,0,0q�6[   $	��Top=�6|   �� 5o      -   5        $��	txtDomain��5o   U   $��23��6   $
��Left=�6&   $��
267,29,0,0q�6A   $	��Top=�6f   $	��120��6~   $	��300��6�   �� 5p      -   5        $��btnAdd��5p   b   $	��Add��6   $��23��6'   $
��Left=�63   $��
393,28,0,0q�6N   $	��Top=�6p   $	��123��6�   $	��500��6�   G� 5q      -   5        $��
pbPassword��5q   Y   $��23��6   $
��Left=�6*   $��
141,29,0,0q�6E   $	��Top=�6k   $	��120��6�   $	��200��6�   �� 5r      -   5        $��label5��5r   j   $��Credentialname��6   $��28��61   $
��Left=�6=   $��6,48,0,0q�6X   $	��Top=�6x   �� 5s      -   5        $��txtDescription��5s   S   $��23��6   $
��Left=�6&   $��6,71,0,0q�6A   $	��Top=�6i   $	��381��6�   $	��400��6�   5t      5u      �� 5v   
   -   5        $��image1��5v   K   $��20��6   $
��Left=�6   $��
12,143,0,0q�67   $
��Fill=�6Y   $	��Top=�6h   $��20��6�   $4��./Tabs/ManageCredential/Images/infobubble24.png��6�    � ToolTip$. ((Double-)Click to edit your credentials.��6�   �� 5x   
   -   5        $��brdChangePassword��5x   �   
�������$��1q�6&   $��76��6:   $
��Left=�6F   $��233,228,0,0q�6a   $	��Top=�6v   $	��147��6�   ._�5y      � -   5        ���	#FFE2E2E2.+�5z      �� $��Your new password:��6   $��28��65   $
��Left=�6A   $��-1,0,0,0q�6\   $	��Top=�6n   G� 5{      -   5        $��pbChangePassword��5{   *   $��24��6   $��	6,21,6,26q�6B   6U   �� 5|      -   5        $��Clear Password��5|      $��23��62   $
��Left=�6>   $��6,48,0,0q�6Y   $	��Top=�6k   $	��133��6�   6�   5}      5~      5      5�      