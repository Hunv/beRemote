   M S B A M L   `   `   `  ����P  LbeRemote.GUI.Controls, Version=0.0.3.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:beRemote.GUI.Controls.FolderView beRemote.GUI.Controls.FolderView  5      <    6beRemote.GUI.Controls.FolderView.FolderListItemControl   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      E
folderView.clr-namespace:beRemote.GUI.Controls.FolderView     5      -   5          >�Name$  mainControl��5	      $
��auto��5
      $
��auto��5      $��20��5       � 	Resources 5      C   =beRemote.GUI.Controls.FolderView.IsCollapsedToLetterConverter   IsCollapsedToLetterConv@   :beRemote.GUI.Controls.FolderView.BoolToVisibilityConverter  BoolToVisibilityConv��%:   &	        &	        5   
   62     5   
   6/   5      .��5      �  � ColumnDefinitions 5   
   �� 5      $��16 �6   �� 5      $
��auto �6   $��16��6,   �� 5      $��* �6   .+�5   
   �� -   5        $��0��5      $��Center=�5       y�5      �� *HasSubItems+ �� 	Converter# [  �� ElementName$ mainControl����5"      �� 5#       �� 
TargetType" ��6"   .R�5$      �� $
��None=�5&      $��Center=�5'      $��Center=�5(       � UseLayoutRounding . $��9��5*      $��9��5+      ��5%      �� *IsCollapsed+# [  $ mainControl��5+      5-      5/      �� 50   
   $��1��51      $��Center=�53      ��52      �� *Value.ConnectionIcon+$ mainControl��53      �� 54   
   $��2��55      $��Center=�57      ��56      �� *Value.ConnectionName+$ mainControl��57      58      59      