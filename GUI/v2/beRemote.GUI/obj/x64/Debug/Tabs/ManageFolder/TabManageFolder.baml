   M S B A M L   `   `   `  ����P  LbeRemote.GUI.Controls, Version=0.0.3.0, Culture=neutral, PublicKeyToken=nullrMclr-namespace:beRemote.GUI.Controls.FolderView;assembly=beRemote.GUI.Controls beRemote.GUI.Controls.FolderView  5      T PbeRemote.Core.Definitions, Version=0.0.3.0, Culture=neutral, PublicKeyToken=nullxRclr-namespace:beRemote.Core.Definitions.Classes;assembly=beRemote.Core.Definitions!beRemote.Core.Definitions.Classes 5      G CbeRemote.GUI, Version=0.0.3.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:beRemote.GUI.Tabs.ManageFolderbeRemote.GUI.Tabs.ManageFolder 5      4   .beRemote.GUI.Tabs.ManageFolder.TabManageFolder   5      \
FolderViewMclr-namespace:beRemote.GUI.Controls.FolderView;assembly=beRemote.GUI.Controls  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      fclassesRclr-namespace:beRemote.Core.Definitions.Classes;assembly=beRemote.Core.Definitions     5      EmanageFolder,clr-namespace:beRemote.GUI.Tabs.ManageFolder     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      -   5        /  )beRemote.Core.Definitions.Classes.TabBase   Name$  mainControl��5         
IconSource$ Images/folder16.png��5
         
IsMultiTab .   � 	Resources 5       �� MergedDictionaries �� Source?  9beRemote.GUI.Tabs.ManageFolder.BoolToBoolInverseConverter   BoolToBoolInverseConv�� 5   
    5      �� 5      $ Language/language.xaml?�6%   �� 5      $" Language/language.de-DE.xaml?�6%   �� 5      $" Language/language.es-ES.xaml?�6%   �� 5      $" Language/language.fr-FR.xaml?�6%   �� 5      $" Language/language.it-IT.xaml?�6%   �� 5      $" Language/language.nl-NL.xaml?�6%   �� 5      $" Language/language.pl-PL.xaml?�6%   �� 5      $" Language/language.ru-RU.xaml?�6%   �� 5      $" Language/language.zh-CN.xaml?�6%   �� 5      $" Language/language.cs-CZ.xaml?�6%   �� 5      $" Language/language.ar-SA.xaml?�6%   �� 5      $" Language/language.bg-BG.xaml?�6%   �� 5      $" Language/language.dk-DK.xaml?�6%   �� 5      $" Language/language.el-GR.xaml?�6%   �� 5      $" Language/language.fa-IR.xaml?�6%   �� 5       $" Language/language.fi-FI.xaml?�6%   �� 5!      $" Language/language.he-IL.xaml?�6%   �� 5"      $" Language/language.hi-IN.xaml?�6%   �� 5#      $" Language/language.hr-HR.xaml?�6%   �� 5$      $" Language/language.hu-HU.xaml?�6%   �� 5%      $" Language/language.ko-KR.xaml?�6%   �� 5&      $" Language/language.nn-NO.xaml?�6%   �� 5'      $" Language/language.se-SE.xaml?�6%   �� 5(      $" Language/language.tr-TR.xaml?�6%   �� 5)      $" Language/language.zh-CN.xaml?�6%   %   &	          5,      66   5-         Header 5/      �� 50   
    �� ResourceKey$ TabTitleManageFolder��6    � DataContext 52      .  (beRemote.GUI.Tabs.ManageFolder.ViewModel  53   
   .  (System.Windows.Interactivity.Interaction	   Triggers	 55      /  )System.Windows.Interactivity.EventTrigger  56   
   
   	EventName$
 Loaded��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 57      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *CmdTabManageFolderLoaded+58      .��5:      � .+�5;   
   ��   TabManageFolderFoldername#��[ 6   $
��Left=�6F   $��	10,10,0,0q�6a   $	��Top=�6t   �� 5<   
   $��23��6   $��
10,26,10,0q�6    �� TextWrapping$
 Wrap=�62   $	��Top=�6e   ��6F   �� *NewFolderName+6e   �� 5>   
     TabManageFolderPublic#��[ 6   $
��Left=�6D   $��	10,54,0,0q�6_   $	��Top=�6r    d� 	IsChecked 6�   �� *
IsPublic+�� 5@   
     TabManageFolderIsRoot#��[ 6   $
��Left=�6D   $��	10,74,0,0q�6_   $	��Top=�6r    6�   �� *IsRoot+�� 5B   
     TabManageFolderParentFolder#��[ 6   $
��Left=�6H   $��	10,94,0,0q�6c   $	��Top=�6v   1   +beRemote.GUI.Controls.FolderView.FolderView  5C   
   $��10,110,10,56q�5D         ItemList 5E      �� *
FolderList+   SelectedValue 5F      �� *SelectedFolder+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�{�5G      �� *IsRoot+ �� 	Converter# [  �� 5J   
     TabManageFolderKeepOpen#��[ 6   $
��Left=�6F   $��	10,0,0,36q�6a   $��Bottom=�6t    6�   �� *
KeepOpen+�� 5L   
   $��
Add Folder��5M      $��	10,0,0,10q�5N      $
��Left=�5O      $	��280��5P      $��21��5Q      $��Bottom=�5R      ��5S      �� *" CmdTabManageFolderAddFolderClick+��5T      ��  �� ElementName$ mainControl��5X      5Y      