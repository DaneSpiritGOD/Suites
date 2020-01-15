using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Suites.Mvvm;
using BindableBase = Suites.Mvvm.BindableBase;

namespace Suites.Wpf.App.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
            MainWindow = window;
            MainWindow.Show();
        }
    }

    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value, () =>
            {
                Trace.WriteLine($"Current Value: {_searchText}");
            });
        }

        public ICommand ReturnCommand { get; }
        private void Return()
            => Trace.WriteLine("return key pressed.");
    }
}
