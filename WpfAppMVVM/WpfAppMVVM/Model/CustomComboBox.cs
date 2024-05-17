using System.Collections;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppMVVM.Model
{
    public class CustomComboBox : ComboBox
    {
        public delegate void CustomEventHandler(object sender, EventArgs e);
        public event CustomEventHandler CustomEvent;

        private int caretPosition;
        private bool _freezComboBox;
        private TextBox _textBox;
        private Type _bufType;

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
                caretPosition = txt.SelectionLength;
                txt.CaretIndex = caretPosition;
            }
            if (txt.SelectionLength == 0 && txt.CaretIndex != 0)
            {
                caretPosition = txt.CaretIndex;
            }
        }

        private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            setItem();
        }

        private void setItem()
        {
            if (SelectedItem == null && !string.IsNullOrEmpty(Text))
            {
                IEnumerable<object> items = ItemsSource as IEnumerable<object>;

                if (items != null && _bufType != null)
                {
                    _freezComboBox = true;
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
                    _freezComboBox = false;
                }
            }
        }

        private void OnTextBoxTextChanged(object sender, RoutedEventArgs e)
        {
            if (CustomEvent != null && !_freezComboBox)
            {
                if (_textBox.SelectionStart == 0 && string.IsNullOrEmpty(Text))
                {
                    IsDropDownOpen = false;
                    SelectedItem = null;
                }
                if (SelectedItem == null)
                {
                    CustomEvent(Text, e);
                }
                IsDropDownOpen = ItemsSource == null ? false : ItemsSource.Cast<object>().Count() > 0;
                if (_bufType is null && IsDropDownOpen) _bufType = Items[0].GetType();
            }
        }
    }
}
