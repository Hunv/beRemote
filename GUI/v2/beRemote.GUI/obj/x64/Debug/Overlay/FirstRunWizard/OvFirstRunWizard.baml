   M S B A M L   `   `   `  ����G  CbeRemote.GUI, Version=0.0.3.0, Culture=neutral, PublicKeyToken=nullm;clr-namespace:beRemote.GUI.Overlay.FirstRunWizard.Converter-beRemote.GUI.Overlay.FirstRunWizard.Converter  5      :    4beRemote.GUI.Overlay.FirstRunWizard.OvFirstRunWizard   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Q	converter;clr-namespace:beRemote.GUI.Overlay.FirstRunWizard.Converter     5      -   5          �Name$  mainControl��5
      $	��700��5      $	��500��5	       � 	Resources 5       �� MergedDictionaries �� SourceF   @beRemote.GUI.Overlay.FirstRunWizard.Converter.IsLastTabConverter   IsLastTabConvI   CbeRemote.GUI.Overlay.FirstRunWizard.Converter.IsNotLastTabConverter  IsNotLastTabConvJ   DbeRemote.GUI.Overlay.FirstRunWizard.Converter.IsNotFirstTabConverter  IsNotFirstTabConv�� 5   
    5      �� 5      $ Language/language.xaml?�6%   �� 5      $" Language/language.de-DE.xaml?�6%   �� 5      $" Language/language.es-ES.xaml?�6%   �� 5      $" Language/language.fr-FR.xaml?�6%   �� 5      $" Language/language.it-IT.xaml?�6%   �� 5      $" Language/language.nl-NL.xaml?�6%   �� 5      $" Language/language.pl-PL.xaml?�6%   �� 5      $" Language/language.ru-RU.xaml?�6%   �� 5      $" Language/language.zh-CN.xaml?�6%   �� 5      $" Language/language.cs-CZ.xaml?�6%   �� 5      $" Language/language.ar-SA.xaml?�6%   �� 5      $" Language/language.bg-BG.xaml?�6%   �� 5      $" Language/language.dk-DK.xaml?�6%   �� 5      $" Language/language.el-GR.xaml?�6%   �� 5      $" Language/language.fa-IR.xaml?�6%   �� 5      $" Language/language.fi-FI.xaml?�6%   �� 5       $" Language/language.he-IL.xaml?�6%   �� 5!      $" Language/language.hi-IN.xaml?�6%   �� 5"      $" Language/language.hr-HR.xaml?�6%   �� 5#      $" Language/language.hu-HU.xaml?�6%   �� 5$      $" Language/language.ko-KR.xaml?�6%   �� 5%      $" Language/language.nn-NO.xaml?�6%   �� 5&      $" Language/language.se-SE.xaml?�6%   �� 5'      $" Language/language.tr-TR.xaml?�6%   �� 5(      $" Language/language.zh-CN.xaml?�6%   %W   &	        &	      &	 &       5+      6+     5,      6.     5-      6/   5.      .+�50      � ���	#FFE5E5E5 � RowDefinitions 51   
   �� 52      $��* �6   �� 53      $��30 �6   .+�56   
   �� -   5        $��tabMain��59       �� TabStripPlacement$
 Left=�57      $��0��58      $
��Left=�5;      ���	#FFE5E5E5 �� SelectedIndex 5:      �� *SelectedTabIndex+ �� ElementName$ mainControl�� �� Mode$ TwoWay=�.@�5?      �� ��5@      �� 5A      	 �� Orientation$	 
Horizontal=�6!   .+�5B      �� $��Images/infobubble16.png��6    $
��None=�6A   �� 5C        WizFirstRunTab1#��[ 6$   $��3,0,0,0q�6L   5D      .��5F      �  5G      �� 5H      $��30 �6(   �� 5I      $��* �6(   .+�5K      �� $��Images/infobubble256.png��6   $��2��6>   $
��None=�6O   $
��Left=�6^   $	��Top=�6y   $��10,10q�6�   
 J� Opacity$	
 0.2Q�6�   �� 5M        WizFirstRunTab1Text1#��[ 6   $��0��6L   �� 5N        WizFirstRunTab1Text2#��[ 6    $��1��6M   $
��Wrap=�6Z   $��5q�6n   5P      5Q      �� 5S      ��5T      �� 5U      $	 
Horizontal=�6!   .+�5V      �� $��Images/directions16.png��6    $
��None=�6A   �� 5W        WizFirstRunTab2#��[ 6$   $��3,0,0,0q�6L   5X      .��5Z      �  5[      �� 5\      $
��auto �6(   �� 5]      $
��auto �6(   �� 5^      $��* �6(   .+�5a      �� $��Images/directions256.png��6   $��3��6>   $
��None=�6O   $
��Left=�6^   $	��Top=�6y   $��10,10q�6�   $	
 0.2Q�6�   �� 5c      $
��Wrap=�6      WizFirstRunTab2Text1#��[ 64   $��5q�6a   $��0��6l   �� 5d      $��1��6!   $	 Vertical=�6.   .+�5e      	� $��5q�6&     WizFirstRunTab2ModeMu#��[ 61    	� 	GroupName$ 	rbgrpMode��6�    d� 	IsChecked 6b   �� *MultiUserModeSelected+$ mainControl��6�   �� 5f      $
��Wrap=�6$    $	  WizFirstRunTab2ModeMuDescription#��[	 68   $��25,0,5,5q�6q   5g      �� 5h      $��2��6!   $	 Vertical=�6.   .+�5i      	� $��5q�6&    
 WizFirstRunTab2ModeSu#��[
 61   $ 	rbgrpMode��6b    6x   �� *SingleUserModeSelected+$ mainControl���� 5j      $
��Wrap=�6$    $  WizFirstRunTab2ModeSuDescription#��[ 68   $��25,0,5,5q�6q   5k      5l      5m      �� 5�      ��5�      �� 5�      $	 
Horizontal=�6!   .+�5�      �� $��Images/key16.png��6    $
��None=�6:   �� 5�        WizFirstRunTab4#��[ 6$   $��3,0,0,0q�6L   5�      .��5�      � .+�5�      �� $��Images/key256.png��6   $
��None=�67   $
��Left=�6F   $	��Top=�6a   $��10,10q�6y   $	
 0.2Q�6�   �� 5�      $
��Left=�6    $��	10,10,0,0q�6;   $
��Wrap=�6N    $  WizFirstRunTab4SpecifySuperadmin#��[ 6b   $	��Top=�6�   G� 5�      -   5        $��pbSuperadmin��5�   t   $
��Left=�6"   $��	10,26,0,0q�6=   $	��Top=�6P   $	��200��6h   6t   �� 5�      $
��Left=�6    $��	10,49,0,0q�6;   $
��Wrap=�6N    / +WizFirstRunTab4SpecifySuperadminDescription#��[ 6b   $	��Top=�6�   $	��524��6�   5�      5�      �� 5�      ��5�      �� 5�      $	 
Horizontal=�6!   .+�5�      �� $��Images/flag_green16.png��6    $
��None=�6A   �� 5�        WizFirstRunTab5#��[ 6$   $��3,0,0,0q�6L   5�      .��5�      � .+�5�      �� $��Images/flag_green256.png��6   $
��None=�6>   $
��Left=�6M   $	��Top=�6h   $��10,10q�6�   $	
 0.2Q�6�   �� 5�      $
��Left=�6    $��	10,10,0,0q�6;   $
��Wrap=�6N     WizFirstRunTab5Text#��[ 6b   $	��Top=�6�   5�      5�      5�      �� 5�   
   $��Right=�6   $��1��62   $	 
Horizontal=�6?   .+�5�      �� $��Right=�5�      $��80��5�      $��3q�5�        WizFirstRunBack#��[ 5�      {�5�      �� *SelectedIndex+$ tabMain�� �� 	Converter# [ ��5�      �� *
CmdBtnBack+$ mainControl���� 5�      $��Right=�5�      $��80��5�      $��3q�5�        WizFirstRunNext#��[ 5�      {�5�      �� *SelectedIndex+$ tabMain��# [  �� ConverterParameter$ 3����5�      �� *
CmdBtnNext+$ mainControl���� 5�      $��Right=�5�      $	��100��5�      $��3q�5�        WizFirstRunFinish#��[ 5�      {�5�      �� *SelectedIndex+$ tabMain��# [  $ 3����5�      �� *CmdBtnFinish+$ mainControl��5�      5�      5�      