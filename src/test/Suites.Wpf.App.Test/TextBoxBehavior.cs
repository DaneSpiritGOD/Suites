using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace Suites.Wpf.App.Test
{
    public class TextBoxBehavior : Behavior<TextBox>
    {
        public string SearchKey
        {
            get => (string)GetValue(SearchKeyProperty);
            set => SetValue(SearchKeyProperty, value);
        }

        // Using a DependencyProperty as the backing store for SearchKey.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchKeyProperty = DependencyProperty.Register(
            "SearchKey",
            typeof(string),
            typeof(TextBoxBehavior),
            new UIPropertyMetadata(default));

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
        }

        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.WriteLine(SearchKey);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
        }
    }
}
