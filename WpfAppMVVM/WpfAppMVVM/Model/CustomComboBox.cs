using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfAppMVVM.Model
{
    public class CustomComboBox: ComboBox
    {
        private int caretPosition;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var element = GetTemplateChild("PART_EditableTextBox");
            if (element != null)
            {
                var textBox = (TextBox)element;
                textBox.SelectionChanged += OnDropSelectionChanged;
            }
        }

        private void OnDropSelectionChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (base.IsDropDownOpen && txt.SelectionLength > 0)
            {
                caretPosition = txt.SelectionLength;
                txt.CaretIndex = caretPosition;
            }
            if (txt.SelectionLength == 0 && txt.CaretIndex != 0)
            {
                caretPosition = txt.CaretIndex;
            }
        }
    }
}
