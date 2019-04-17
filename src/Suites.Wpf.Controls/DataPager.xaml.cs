using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Suites.Wpf.Controls
{
    /// <summary>
    /// DataPager.xaml 的交互逻辑
    /// </summary>
    public partial class DataPager : UserControl
    {

        public DataPager()
        {
            InitializeComponent();

            setButtonVisible();

            int currentPageButtonIndex = getCurrentPageButtonIndex(this);
            setButtonText(this, currentPageButtonIndex);
            SetButtonBorder(currentPageButtonIndex);
        }

        public static readonly DependencyProperty TotalPageProperty =
        DependencyProperty.Register("TotalPage", typeof(int), typeof(DataPager),
            new PropertyMetadata(0, OnTotalPagePropertyChanged));

        public int TotalPage
        {
            get => (int)GetValue(TotalPageProperty);
            set => SetValue(TotalPageProperty, value);
        }

        private static void OnTotalPagePropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var pager = dependencyObject as DataPager;
            if (pager == null)
            {
                return;
            }

            pager.setButtonVisible();

            if (pager.PageIndex + 1 > pager.TotalPage)
            {
                return;
            }

            pager.ButtonPreviousPage.IsEnabled = pager.PageIndex > 0;
            pager.ButtonNextPage.IsEnabled = pager.PageIndex + 1 < pager.TotalPage;

            int currentPageButtonIndex = getCurrentPageButtonIndex(pager);
            setButtonText(pager, currentPageButtonIndex);
            pager.SetButtonBorder(currentPageButtonIndex);
        }

        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(DataPager), new PropertyMetadata(0, OnPageIndexPropertyChanged));
        public int PageIndex
        {
            get => (int)GetValue(PageIndexProperty);
            set => SetValue(PageIndexProperty, value);
        }

        private static void OnPageIndexPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var pager = dependencyObject as DataPager;
            if (pager == null)
            {
                return;
            }

            if (pager.PageIndex + 1 > pager.TotalPage)
            {
                return;
            }

            pager.ButtonPreviousPage.IsEnabled = pager.PageIndex > 0;
            pager.ButtonNextPage.IsEnabled = pager.PageIndex + 1 < pager.TotalPage;

            int currentPageButtonIndex = getCurrentPageButtonIndex(pager);
            setButtonText(pager, currentPageButtonIndex);
            pager.SetButtonBorder(currentPageButtonIndex);
        }

        private static int getCurrentPageButtonIndex(DataPager pager)
        {
            if (pager.PageIndex + 1 > pager.TotalPage)
            {
                return -1;
            }

            if (pager.TotalPage <= 5)
            {
                return pager.PageIndex;
            }

            if (pager.PageIndex + 1 <= 3)
            {
                return pager.PageIndex;
            }

            if (pager.PageIndex + 1 == pager.TotalPage)
            {
                return 4;
            }
            else if (pager.PageIndex + 1 == pager.TotalPage - 1)
            {
                return 3;
            }

            return 2;
        }

        private static void setButtonText(DataPager pager, int currentPageButtonIndex)
        {
            if (pager.TotalPage <= 5)
            {
                pager.ButtonText1.Text = "1";
                pager.ButtonText2.Text = "2";
                pager.ButtonText3.Text = "3";
                pager.ButtonText4.Text = "4";
                pager.ButtonText5.Text = "5";
                return;
            }

            if (currentPageButtonIndex == 0)
            {
                pager.ButtonText1.Text = (pager.PageIndex + 1).ToString();
                pager.ButtonText2.Text = (pager.PageIndex + 2).ToString();
                pager.ButtonText3.Text = (pager.PageIndex + 3).ToString();
                pager.ButtonText4.Text = (pager.PageIndex + 4).ToString();
                pager.ButtonText5.Text = "...";
            }
            else if (currentPageButtonIndex == 1)
            {
                pager.ButtonText1.Text = pager.PageIndex.ToString();
                pager.ButtonText2.Text = (pager.PageIndex + 1).ToString();
                pager.ButtonText3.Text = (pager.PageIndex + 2).ToString();
                pager.ButtonText4.Text = (pager.PageIndex + 3).ToString();
                pager.ButtonText5.Text = "...";
            }
            else if (currentPageButtonIndex == 2)
            {
                pager.ButtonText1.Text = (pager.PageIndex + 1 - 2) > 1 ? "..." : (pager.PageIndex + 1 - 2).ToString();
                pager.ButtonText2.Text = pager.PageIndex.ToString();
                pager.ButtonText3.Text = (pager.PageIndex + 1).ToString();
                pager.ButtonText4.Text = (pager.PageIndex + 2).ToString();
                pager.ButtonText5.Text = (pager.PageIndex + 3 < pager.TotalPage) ? "..." : pager.TotalPage.ToString();
            }
            else if (currentPageButtonIndex == 3)
            {
                pager.ButtonText1.Text = "...";
                pager.ButtonText2.Text = (pager.TotalPage - 3).ToString();
                pager.ButtonText3.Text = (pager.TotalPage - 2).ToString();
                pager.ButtonText4.Text = (pager.TotalPage - 1).ToString();
                pager.ButtonText5.Text = pager.TotalPage.ToString();
            }
            else if (currentPageButtonIndex == 4)
            {
                pager.ButtonText1.Text = "...";
                pager.ButtonText2.Text = (pager.TotalPage - 3).ToString();
                pager.ButtonText3.Text = (pager.TotalPage - 2).ToString();
                pager.ButtonText4.Text = (pager.TotalPage - 1).ToString();
                pager.ButtonText5.Text = pager.TotalPage.ToString();
            }
        }

        private void setButtonVisible()
        {
            Button1.Visibility = Visibility.Visible;
            Button2.Visibility = checkVisible(2);
            Button3.Visibility = checkVisible(3);
            Button4.Visibility = checkVisible(4);
            Button5.Visibility = checkVisible(5);
        }

        private Visibility checkVisible(int total)
            => TotalPage >= total ? Visibility.Visible : Visibility.Collapsed;

        private void SetButtonBorder(int currentPageButtonIndex)
        {
            Color normalColor = Color.FromRgb(216, 216, 216);
            Color selectedColor = Color.FromRgb(39, 124, 220);

            this.Button1.BorderBrush = new SolidColorBrush(currentPageButtonIndex == 0 ? selectedColor : normalColor);
            this.Button2.BorderBrush = new SolidColorBrush(currentPageButtonIndex == 1 ? selectedColor : normalColor);
            this.Button3.BorderBrush = new SolidColorBrush(currentPageButtonIndex == 2 ? selectedColor : normalColor);
            this.Button4.BorderBrush = new SolidColorBrush(currentPageButtonIndex == 3 ? selectedColor : normalColor);
            this.Button5.BorderBrush = new SolidColorBrush(currentPageButtonIndex == 4 ? selectedColor : normalColor);
        }

        private void Button1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.ButtonText1.Text == "...")
            {
                this.PageIndex = Convert.ToInt32(this.ButtonText2.Text) - 1 - 1;
            }
            else
            {
                this.PageIndex = Convert.ToInt32(this.ButtonText1.Text) - 1;
            }
        }

        private void Button2_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => PageIndex = Convert.ToInt32(this.ButtonText2.Text) - 1;

        private void Button3_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => PageIndex = Convert.ToInt32(this.ButtonText3.Text) - 1;

        private void Button4_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => PageIndex = Convert.ToInt32(this.ButtonText4.Text) - 1;

        private void Button5_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ButtonText5.Text == "...")
            {
                PageIndex = Convert.ToInt32(ButtonText4.Text) + 1 - 1;
            }
            else
            {
                PageIndex = Convert.ToInt32(ButtonText5.Text) - 1;
            }
        }

        private void Button1_MouseEnter(object sender, MouseEventArgs e)
        {
            Border button = sender as Border;

            int currentButtonIndex = getCurrentPageButtonIndex(this);
            if (currentButtonIndex == -1)
            {
                return;
            }

            if (button.Name != "Button" + (currentButtonIndex + 1).ToString())
            {
                button.BorderBrush = new SolidColorBrush(Color.FromRgb(91, 156, 228));
            }
        }

        private void Button1_MouseLeave(object sender, MouseEventArgs e)
        {
            Border button = sender as Border;

            int currentButtonIndex = getCurrentPageButtonIndex(this);
            if (currentButtonIndex == -1)
            {
                return;
            }

            if (button.Name != "Button" + (currentButtonIndex + 1).ToString())
            {
                button.BorderBrush = new SolidColorBrush(Color.FromRgb(216, 216, 216));
            }
        }

        private void Button1_MouseMove(object sender, MouseEventArgs e)
        {
            Border button = sender as Border;

            int currentButtonIndex = getCurrentPageButtonIndex(this);
            if (currentButtonIndex == -1)
            {
                return;
            }

            if (button.Name != "Button" + (currentButtonIndex + 1).ToString())
            {
                button.BorderBrush = new SolidColorBrush(Color.FromRgb(91, 156, 228));
            }
        }

        private void ButtonPreviousPage_Click(object sender, RoutedEventArgs e)
            => this.PageIndex -= 1;

        private void ButtonNextPage_Click(object sender, RoutedEventArgs e)
            => this.PageIndex += 1;
    }
}
