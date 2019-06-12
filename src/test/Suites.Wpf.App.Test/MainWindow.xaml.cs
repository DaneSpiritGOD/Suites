using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Suites.Wpf.Controls;

namespace Suites.Wpf.App.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs2 e)
        {
            Trace.WriteLine(e.NewText);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Width_Button.Content = "1";
        }
    }
}
