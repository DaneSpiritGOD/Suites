using System;
using System.Windows;
using System.Windows.Controls;

namespace Suites.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        public SearchBox()
        {
            InitializeComponent();
        }

        private static readonly Type _thisType = typeof(SearchBox);

        public static readonly DependencyProperty TextProperty = TextBox.TextProperty.AddOwner(_thisType,
            new FrameworkPropertyMetadata(string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty SearchIconVisibilityProperty = DependencyProperty.Register("SearchIconVisibility",
            typeof(Visibility), _thisType, new FrameworkPropertyMetadata(Visibility.Visible));

        public Visibility SearchIconVisibility
        {
            get => (Visibility)GetValue(SearchIconVisibilityProperty);
            set => SetValue(SearchIconVisibilityProperty, value);
        }

        public static readonly DependencyProperty ClearIconVisibilityProperty = DependencyProperty.Register("ClearIconVisibility",
            typeof(Visibility), _thisType, new FrameworkPropertyMetadata(Visibility.Visible));

        public Visibility ClearIconVisibility
        {
            get => (Visibility)GetValue(ClearIconVisibilityProperty);
            set => SetValue(ClearIconVisibilityProperty, value);
        }
    }
}
