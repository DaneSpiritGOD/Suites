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

        private void DataPager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            var pager = (DataPager)sender;
            Log.AppendText($"You change the pager index to {e.NewValue + 1}/{pager.TotalPage} .\n");
            Log.ScrollToEnd();
        }
    }
}
