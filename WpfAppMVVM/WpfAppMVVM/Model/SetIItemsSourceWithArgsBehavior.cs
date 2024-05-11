using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static WpfAppMVVM.Model.CustomComboBox;

namespace WpfAppMVVM.Model
{
    internal class SetIItemsSourceWithArgsBehavior: Behavior<UIElement>
    {
        public ICommand SetItemsCommand
        {
            get { return (ICommand)GetValue(SetItemsCommandProperty); }
            set { SetValue(SetItemsCommandProperty, value); }
        }

        public static readonly DependencyProperty SetItemsCommandProperty =
            DependencyProperty.Register("SetItemsCommand", typeof(ICommand), typeof(SetIItemsSourceWithArgsBehavior), new UIPropertyMetadata(null));


        protected override void OnAttached()
        {
            //AssociatedObject.CustomEvent += new TextCompositionEventHandler(AssociatedObjectKeyUp);
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            //AssociatedObject.TextInput -= new TextCompositionEventHandler(AssociatedObjectKeyUp);
            base.OnDetaching();
        }

        private void AssociatedObjectKeyUp(object sender, EventArgs e)
        {
            if (SetItemsCommand != null)
            {
                SetItemsCommand.Execute(e);
            }
        }
    }
}
