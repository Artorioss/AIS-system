   M S B A M L   `   `   `  ����E  AWpfAppMVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WpfAppMVVM.ViewsWpfAppMVVM.Views  5   	   ,    &WpfAppMVVM.Views.CreatingAccountWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:WpfAppMVVM.Views     5   	     � Title$'  !Выставление счета��5   	   $
��auto��6#   $	��800��61   $��16(�6=   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��auto �6   6,   �� 5      $
��auto �6   6,    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   .+�5   
   �� $(��"Информация о счете��6   $��0��6/   $��2��6<   $��5q�6P   $��5q�6[   .��5      �  5      �� 5      $
��auto �6$   64   �� 5      $
��auto �6$   64   �� 5      $
��auto �6$   64   �� 5      $
��auto �6$   64    5      �� 5      $
��auto �6'   66   �� 5      6)   .+�5!      �� $��№ Счета��6   $��0��6*   $��0,5,5,5q�67   6J   �� 5"      $��0��6   $��1��6(   $��0,5,0,5q�68    �� 
IsEditable$
 True��6I    �� SelectedItem 6[   �� *Transportation.Account_number+ �� Mode$ OneWayToSource=�6�   �� 5$      $��Дата счёта��6   $��1��6-   $��0,5,5,5q�6:   6M   (  "System.Windows.Controls.DatePicker  5%      $��1��6   $��1��6*   $��0,5,0,5q�6:      SelectedDate 6K   �� *Transportation.Invoice_date+$ OneWayToSource=�6�   �� 5'      $!��Название счета��6   $��Center=�61   $��0,5,5,5q�6N   $��2��6_   6n   �� 5(      $��2��6   $��1��6'    �� TextWrapping$
 Wrap=�67   $��0,5,0,5q�6K   6^   5)      5*      �� 5+   
   $��1��6   $
��auto��6   $
��auto��6+   $!��Выставить счет��69   $��2��6R   $��5q�6f   $	��500��6q   6�   5,      5-      