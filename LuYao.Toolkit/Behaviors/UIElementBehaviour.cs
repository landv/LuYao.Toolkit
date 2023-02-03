using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LuYao.Toolkit.Behaviors
{
    public class UIElementBehaviour : Behavior<TextBox>
    {
        public bool IsFocused
        {
            get { return (bool)GetValue(IsFocusedProperty); }
            set { SetValue(IsFocusedProperty, value); }
        }

        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.Register("IsFocused", typeof(bool), typeof(UIElementBehaviour), new PropertyMetadata(false));

        protected override void OnAttached()
        {
            base.OnAttached();
            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.GotFocus += AssociatedObject_GotFocus;
                this.AssociatedObject.LostFocus += AssociatedObject_LostFocus;
            }
        }

        private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
        {
            this.IsFocused = false;
        }

        private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
        {
            this.IsFocused = true;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
                this.AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
            }
        }
    }
}
