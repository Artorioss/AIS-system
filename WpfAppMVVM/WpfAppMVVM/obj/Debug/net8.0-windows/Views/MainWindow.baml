   M S B A M L   `   `   `  ����E  AWpfAppMVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WpfAppMVVM.ViewsWpfAppMVVM.Views  5   	   =#clr-namespace:WpfAppMVVM.ViewModelsWpfAppMVVM.ViewModels  5   	   !    WpfAppMVVM.Views.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:WpfAppMVVM.Views     5   	   2vm#clr-namespace:WpfAppMVVM.ViewModels     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  68     � Title$  Главное окно��5	   	   $	��550��6   $
��1000��6+   $��16(�68    � DataContext 5      /  )WpfAppMVVM.ViewModels.MainWindowViewModel  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   6,   �� 5      $��* �6   6)   �� 5      $
��0.1* �6   $��50��6*   6;   .+�5   
   c� $��0��6   .@�5      �� $��Фильтр��6   6'   �� 5      $	��150��6    �� DisplayMemberPath$
 Name��6q   ��6&   �� *StateOrders+ �� SelectedItem 6J   �� *SelectedState+6�   �� 5      $��	10,5,10,5q�6   6-   �� 5      $��Справочники��6   $��0,0,0,0q�6+   ��6<   �� *ShowReferencesBook+6e   �� 5      $��	10,5,10,5q�6   6-   �� 5      $��
Месяц��6   6&   �� 5      $	��150��6    6&   �� *SelectedMonth+��6M   �� *Months+6n   �� 5      $��Год��6   6$   �� 5      $	��100��6   ��6&   �� *Years+ 6D   �� *SelectedYear+6m   5       &   System.Windows.Controls.DataGrid  5"   
   -   5          Name$ dataGridView��5"      $��1��6)      AutoGenerateColumns$ False��66      
IsReadOnly$
 True��6R   ��6d   �� *ItemsSource+ �� Mode$ TwoWay=� 6�   �� *TransportationDTO+	   Columns	 5#      0  *System.Windows.Controls.DataGridTextColumn  5$      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Дата��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.8* 6S   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *DateLoading+6b     5%      $
 Заказчик��6%   $ * 6X    67   �� *CustomerName+6d     5&      $
 Водитель��6%   $ * 6V    67   �� *
DriverName+6b     5'      $
 Маршрут��6%   $ 2* 6T    66   �� *	RouteName+6a     5(      $(
 "Сумма от заказчика��6%   $
 0.5* 6[    6A   �� *Price+6j     5)      $
 Оплата��6%   $
 0.5* 65    6B   �� *PaymentToDriver+6h     5*      $
 Дельта��6%   $
 0.5* 65    6B   �� *Delta+6^     5+      $+
 %Компания перевозчик��6%   $ * 6k    6B   �� *TransportCompanyName+6w    � ContextMenu 5-      �� 5.      .@�5/      m� $4��.Выставить счет заказчику��6   m� 50      $2��,Выставить счет водителю��6   m� 51      $6��0Копировать название счета��6    m� Command 6B   �� *CopyCommand+m� 52      $!��Удалить заявку��6    67   �� *DeleteCommand+53      *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 55      +  %Microsoft.Xaml.Behaviors.EventTrigger  56         	EventName$ MouseDoubleClick��6!   *	  $Microsoft.Xaml.Behaviors.TriggerBase 	  Actions. 57      2
  ,Microsoft.Xaml.Behaviors.InvokeCommandAction
   
  Command 58      �� *
EditData+59        5:      $ Sorting��6!   . 5;      
   5<      �� *SortCommand+5=      5?      �� 5A   
   $)��#Добавить перевозку��6   $��5q�6.   $��2��69   ��6F   �� *CreateTransportation+6q   5B      5C      