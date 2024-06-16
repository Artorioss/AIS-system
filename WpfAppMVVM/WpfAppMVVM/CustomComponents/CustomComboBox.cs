using System.Collections;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfAppMVVM.CustomComponents
{
    public class CustomComboBox : ComboBox
    {
        public delegate void CustomEventHandler(object sender, EventArgs e);
        public event CustomEventHandler CustomEvent;

        private TextBox _textBox;
        private Type _bufType;
        private DispatcherTimer _timer;

        public CustomComboBox()
        {
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.5); // Интервал полсекунды
            _timer.Tick += Timer_Tick;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var element = GetTemplateChild("PART_EditableTextBox");
            if (element != null)
            {
                _textBox = (TextBox)element;
                _textBox.SelectionChanged += OnDropSelectionChanged;
                _textBox.LostFocus += OnTextBoxLostFocus;
                _textBox.TextChanged += OnTextBoxTextChanged;
            }
            IsTextSearchEnabled = false;
            IsEditable = true;
        }

        private void OnDropSelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (IsDropDownOpen && txt.SelectionLength > 0)
            {
                txt.CaretIndex = txt.SelectionLength;
            }
        }

        private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            SetItem();
        }

        private void SetItem()
        {
            if (SelectedItem == null && !string.IsNullOrEmpty(Text))
            {
                IEnumerable<object> items = ItemsSource as IEnumerable<object>;

                if (items != null && _bufType != null)
                {
                    var item = items.FirstOrDefault(s =>
                    {
                        PropertyInfo prop = _bufType.GetProperty(DisplayMemberPath);
                        if (prop != null)
                        {
                            object propValue = prop.GetValue(s);
                            if (propValue != null)
                            {
                                string propValueString = propValue.ToString();
                                return !string.IsNullOrEmpty(propValueString) && propValueString.ToLower().Contains(Text.ToLower());
                            }
                        }
                        return false;
                    });

                    if (item != null)
                    {
                        SelectedItem = item;
                    }
                    else
                    {
                        var newItem = Activator.CreateInstance(_bufType);
                        PropertyInfo prop = _bufType.GetProperty(DisplayMemberPath);
                        if (prop != null)
                        {
                            prop.SetValue(newItem, Text);
                        }
                        ((IList)ItemsSource).Add(newItem);
                        SelectedItem = newItem;
                    }
                }
            }
        }

        private void OnTextBoxTextChanged(object sender, RoutedEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            // Вызываем событие или выполняем запрос в базу данных
            if (CustomEvent != null)
            {
                CustomEvent(this, EventArgs.Empty);
            }
        }
    }

}
