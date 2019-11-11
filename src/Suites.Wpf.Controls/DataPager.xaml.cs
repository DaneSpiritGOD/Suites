using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

            SetButtonVisible();
            Measure();
        }

        public static readonly DependencyProperty TotalPageProperty =
            DependencyProperty.Register("TotalPage",
                typeof(int),
                typeof(DataPager),
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

            pager.SetButtonVisible();

            if (pager.PageIndex + 1 > pager.TotalPage)
            {
                return;
            }

            pager.ButtonPreviousPage.IsEnabled = pager.PageIndex > 0;
            pager.ButtonNextPage.IsEnabled = pager.PageIndex + 1 < pager.TotalPage;

            pager.Measure();
        }

        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex",
                typeof(int),
                typeof(DataPager),
                new PropertyMetadata(0, OnPageIndexPropertyChanged));
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
            if (pager == null || pager.PageIndex + 1 > pager.TotalPage)
            {
                return;
            }

            pager.ButtonPreviousPage.IsEnabled = pager.PageIndex > 0;
            pager.ButtonNextPage.IsEnabled = pager.PageIndex + 1 < pager.TotalPage;

            pager.Measure();
        }

        private static int GetCurrentPageButtonIndex(DataPager pager)
            => (pager.PageIndex, pager.TotalPage) switch
            {
                var (i, t) when i + 1 > t => -1,
                var (i, t) when t <= 5 || i + 1 <= 3 => i,
                var (i, t) when i + 1 == t => 4,
                var (i, t) when i + 2 == t => 3,
                _ => 2
            };

        private const string More = "...";
        private const string One = "1";
        private const string Two = "2";
        private const string Three = "3";
        private const string Four = "4";
        private const string Five = "5";
        private void Measure()
        {
            var btnIndex = GetCurrentPageButtonIndex(this);
            SetButtonText(this, btnIndex);
            SetButtonBorder(btnIndex);

            static void SetButtonText(DataPager pager, int curBtnIndex)
            {
                var (index, total) = (pager.PageIndex, pager.TotalPage);
                if (total <= 5)
                {
                    pager.ButtonText1.Text = One;
                    pager.ButtonText2.Text = Two;
                    pager.ButtonText3.Text = Three;
                    pager.ButtonText4.Text = Four;
                    pager.ButtonText5.Text = Five;
                    return;
                }
                switch (curBtnIndex)
                {
                    case 0:
                        pager.ButtonText1.Text = (index + 1).ToString();
                        pager.ButtonText2.Text = (index + 2).ToString();
                        pager.ButtonText3.Text = (index + 3).ToString();
                        pager.ButtonText4.Text = (index + 4).ToString();
                        pager.ButtonText5.Text = More;
                        break;
                    case 1:
                        pager.ButtonText1.Text = index.ToString();
                        pager.ButtonText2.Text = (index + 1).ToString();
                        pager.ButtonText3.Text = (index + 2).ToString();
                        pager.ButtonText4.Text = (index + 3).ToString();
                        pager.ButtonText5.Text = More;
                        break;
                    case 2:
                        pager.ButtonText1.Text = index > 2 ? More : (index - 1).ToString();
                        pager.ButtonText2.Text = index.ToString();
                        pager.ButtonText3.Text = (index + 1).ToString();
                        pager.ButtonText4.Text = (index + 2).ToString();
                        pager.ButtonText5.Text = (index + 3 < total) ? More : total.ToString();
                        break;
                    case 3:
                        pager.ButtonText1.Text = More;
                        pager.ButtonText2.Text = (total - 3).ToString();
                        pager.ButtonText3.Text = (total - 2).ToString();
                        pager.ButtonText4.Text = (total - 1).ToString();
                        pager.ButtonText5.Text = total.ToString();
                        break;
                    case 4:
                        pager.ButtonText1.Text = More;
                        pager.ButtonText2.Text = (total - 3).ToString();
                        pager.ButtonText3.Text = (total - 2).ToString();
                        pager.ButtonText4.Text = (total - 1).ToString();
                        pager.ButtonText5.Text = total.ToString();
                        break;
                    default:
                        throw new NotImplementedException();
                };
            }

            void SetButtonBorder(int curBtnIndex)
            {
                var normalColor = Color.FromRgb(216, 216, 216);
                var selectedColor = Color.FromRgb(39, 124, 220);

                Button1.BorderBrush = new SolidColorBrush(curBtnIndex == 0 ? selectedColor : normalColor);
                Button2.BorderBrush = new SolidColorBrush(curBtnIndex == 1 ? selectedColor : normalColor);
                Button3.BorderBrush = new SolidColorBrush(curBtnIndex == 2 ? selectedColor : normalColor);
                Button4.BorderBrush = new SolidColorBrush(curBtnIndex == 3 ? selectedColor : normalColor);
                Button5.BorderBrush = new SolidColorBrush(curBtnIndex == 4 ? selectedColor : normalColor);
            }
        }

        private void SetButtonVisible()
        {
            Button1.Visibility = Visibility.Visible;
            Button2.Visibility = CheckVisible(2);
            Button3.Visibility = CheckVisible(3);
            Button4.Visibility = CheckVisible(4);
            Button5.Visibility = CheckVisible(5);

            Visibility CheckVisible(int total)
                => TotalPage >= total ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Button1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ButtonText1.Text == More)
            {
                PageIndex = Convert.ToInt32(ButtonText2.Text) - 1 - 1;
            }
            else
            {
                PageIndex = Convert.ToInt32(ButtonText1.Text) - 1;
            }
        }

        private void Button2_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => PageIndex = Convert.ToInt32(ButtonText2.Text) - 1;

        private void Button3_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => PageIndex = Convert.ToInt32(ButtonText3.Text) - 1;

        private void Button4_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => PageIndex = Convert.ToInt32(ButtonText4.Text) - 1;

        private void Button5_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => PageIndex = ButtonText5.Text == More ? Convert.ToInt32(ButtonText4.Text)
                : Convert.ToInt32(ButtonText5.Text) - 1;

        private void Button1_MouseEnter(object sender, MouseEventArgs e)
        {
            var button = sender as Border;

            int currentButtonIndex = GetCurrentPageButtonIndex(this);
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
            var button = sender as Border;

            int currentButtonIndex = GetCurrentPageButtonIndex(this);
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

            int currentButtonIndex = GetCurrentPageButtonIndex(this);
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
            => PageIndex -= 1;

        private void ButtonNextPage_Click(object sender, RoutedEventArgs e)
            => PageIndex += 1;
    }
}
