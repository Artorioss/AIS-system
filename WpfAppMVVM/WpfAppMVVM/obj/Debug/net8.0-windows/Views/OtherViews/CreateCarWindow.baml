   M S B A M L   `   `   `  ����E  AWpfAppMVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:WpfAppMVVM.Views.OtherViewsWpfAppMVVM.Views.OtherViews  5   	   3clr-namespace:WpfAppMVVM.ModelWpfAppMVVM.Model  5   	   I)clr-namespace:WpfAppMVVM.CustomComponentsWpfAppMVVM.CustomComponents  5   	   ;"clr-namespace:WpfAppMVVM.BehaviorsWpfAppMVVM.Behaviors  5	   	   1    +WpfAppMVVM.Views.OtherViews.CreateCarWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;local)clr-namespace:WpfAppMVVM.Views.OtherViews     5   	   1local1clr-namespace:WpfAppMVVM.Model     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  67   @CustomComponents)clr-namespace:WpfAppMVVM.CustomComponents  5   	   2	Behaviors"clr-namespace:WpfAppMVVM.Behaviors  5	   	   -   5        $��Window��5   c     � 
ResizeMode$  NoResize=�6&   $	��240��6<   $	��400��6I   $��16(�6U    � Title 6	   �� *
WindowName+.��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   6,   �� 5      $
��auto �6   6,   .+�5   
   �� $!��Введите данные��6   $��5q�6+   .��5      �  � ColumnDefinitions 5      �� 5      $
��auto �6'   66   �� 5      6)    5      �� 5      $
��auto �6$   64   �� 5      $
��auto �6$   64   �� 5      $
��auto �6$   64   �� 5      $
��auto �6$   64   .+�5       �� $%��Марка автомобиля��6   $��5q�63   6@   0   *WpfAppMVVM.CustomComponents.CustomComboBox  5!      -   5          Name$ ComboBoxBrands��5!   2   $��1��6J   $��5q�6Z    �� DisplayMemberPath$
 Name��6e    �� SelectedItem 6~   �� *Brand+��6�   �� *BrandSource+*  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5"      +  %Microsoft.Xaml.Behaviors.EventTrigger  5#         	EventName$ CustomEvent��6)   *  $Microsoft.Xaml.Behaviors.TriggerBase	   Actions.	 5$      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction  
   Command
 5%      �� *	GetBrands+   CommandParameter 5&      �� *Text+ �� ElementName$ ComboBoxBrands��5'      5)      �� 5+      $��
Номер��6   $��1��6(   $��5q�65   6B   �� 5,      $��1��6   $��2��6'   $��5q�67   ��6B   �� *Number+6\   �� 5.      $��
Тягач��6   $��2��6(   $��5q�65   6B   �� 5/      $��2��6   $��Center=�6(   $��1��6C   $��5q�6S    d� 	IsChecked 6^   �� *	IsTruck+6~   50      51      �� 52   
   $��1��6   $��30��6   $��5q�6*   ��65   �� *
ButtonText+��6U   �� *	CreateCar+��6s   �� $ Window��6�   53      54      