using Prism.Commands;
using Prism.Mvvm;
using Suites.Wpf.App.Controls.Pie;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
        }
    }
}
