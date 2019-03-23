using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Suites.Mvvm;
using Suites.Wpf.App.Controls.Pie;
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
        public ObservableCollection<SimpleSeries> Classes { get; set; }
        public DelegateCommand AddA { get; set; }

        public MainWindowViewModel()
        {
            Classes = new ObservableCollection<SimpleSeries>()
            {
                new SimpleSeries{ Title="A",Value=new ObservableDoubleValue(10),Color="red"},
                new SimpleSeries{ Title="B",Value=new ObservableDoubleValue(20),Color="yellow"},
                new SimpleSeries{ Title="C",Value=new ObservableDoubleValue(10),Color="blue"},
            };
            AddA = new DelegateCommand(() =>
            {
                Classes[0].Value.Value++;
                Classes[1].Value.Value += 2;
                Classes[2].Value.Value += 3;
            });

            ReturnCommand = new DelegateCommand(Return);
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
