using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfAppMVVM.Model
{
    internal class FocusableChangedBehavior : Behavior<UIElement>
    {
        public ICommand FocusChangedCommand
        {
            get { return (ICommand)GetValue(FocusChangedCommandProperty); }
            set { SetValue(FocusChangedCommandProperty, value); }
        }

        public static readonly DependencyProperty FocusChangedCommandProperty =
            DependencyProperty.Register("FocusChangedCommand", typeof(ICommand), typeof(FocusableChangedBehavior), new UIPropertyMetadata(null));


        protected override void OnAttached()
        {
            AssociatedObject.IsKeyboardFocusedChanged += AssociatedObjectFocusChanged;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.IsKeyboardFocusedChanged -= AssociatedObjectFocusChanged;
            base.OnDetaching();
        }

        private void AssociatedObjectFocusChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (FocusChangedCommand != null)
            {
                FocusChangedCommand.Execute(AssociatedObject.IsKeyboardFocused);
            }
        }
    }

}
