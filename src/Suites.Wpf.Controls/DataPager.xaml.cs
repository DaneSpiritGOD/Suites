using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Suites.Wpf.Controls
{
    public sealed class PageIndexChangedEventArgs : RoutedEventArgs
    {
        internal PageIndexChangedEventArgs(int oldValue, int newValue)
            : base(DataPager.PageIndexChangedEvent)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public int OldValue { get; }
        public int NewValue { get; }
    }
    public delegate void PageIndexChangedEventHandler(object sender, PageIndexChangedEventArgs e);

    /// <summary>
    /// DataPager.xaml 的交互逻辑
    /// </summary>
    public partial class DataPager : UserControl
    {
        #region 常量
        private const string More = "...";
        private const string One = "1";
        private const string Two = "2";
        private const string Three = "3";
        private const string Four = "4";
        private const string Five = "5";

        private static SolidColorBrush CreateConstBrush(Color color)
        {
            var brush = new SolidColorBrush(color);
            brush.Freeze();
            return brush;
        }

        private readonly Brush _normalBrush;
        private readonly Brush _selectedBrush;
        private readonly Brush _hover;
        #endregion 常量

        static DataPager()
        {
            TotalPageProperty = DependencyProperty.Register("TotalPage",
               typeof(int),
               typeof(DataPager),
               new PropertyMetadata(0, OnTotalPagePropertyChanged));

            PageIndexProperty = DependencyProperty.Register("PageIndex",
                    typeof(int),
                    typeof(DataPager),
                    new PropertyMetadata(0, OnPageIndexPropertyChanged));

            PageIndexChangedEvent = EventManager.RegisterRoutedEvent("PageIndexChanged", RoutingStrategy.Bubble, typeof(PageIndexChangedEventHandler), typeof(DataPager));
        }

        public DataPager()
        {
            InitializeComponent();

            _normalBrush = (Brush)FindResource("Normal");
            _selectedBrush = (Brush)FindResource("Selected");
            _hover = (Brush)FindResource("MouseHover");

            SetButtonVisible();
            MeasureAppearance();
        }

        public static readonly DependencyProperty TotalPageProperty;
        public int TotalPage
        {
            get => (int)GetValue(TotalPageProperty);
            set => SetValue(TotalPageProperty, value);
        }
        private static void OnTotalPagePropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var pager = (DataPager)dependencyObject;
            pager.OnValueChanged();
        }

        public static readonly DependencyProperty PageIndexProperty;
        public int PageIndex
        {
            get => (int)GetValue(PageIndexProperty);
            set => SetValue(PageIndexProperty, value);
        }
        private static void OnPageIndexPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            var pager = (DataPager)dependencyObject;
            pager.OnPageIndexPropertyChanged((int)e.OldValue, (int)e.NewValue);
        }

        private void OnPageIndexPropertyChanged(int oldIndex, int newIndex)
        {
            RaiseEvent(new PageIndexChangedEventArgs(oldIndex, newIndex));
            OnValueChanged();
        }

        public static readonly RoutedEvent PageIndexChangedEvent;
        public event PageIndexChangedEventHandler PageIndexChanged
        {
            add
            {
                AddHandler(PageIndexChangedEvent, value);
            }
            remove
            {
                RemoveHandler(PageIndexChangedEvent, value);
            }
        }

        private int CalcSelectedPageButtonIndex()
            => (PageIndex, TotalPage) switch
            {
                var (i, t) when i >= t => -1,
                var (i, t) when t <= 5 || i <= 2 => i,
                var (i, t) when i + 1 == t => 4,
                var (i, t) when i + 2 == t => 3,
                _ => 2
            };

        private void OnValueChanged()
        {
            SetButtonVisible();
            TotalCount.Text = TotalPage.ToString();

            if (PageIndex + 1 > TotalPage)
            {
                return;
            }

            ButtonPreviousPage.IsEnabled = PageIndex > 0;
            ButtonNextPage.IsEnabled = PageIndex + 1 < TotalPage;

            MeasureAppearance();
        }

        private void MeasureAppearance()
        {
            var btnSeletedIndex = CalcSelectedPageButtonIndex();
            SetButtonText(btnSeletedIndex);
            SetButtonBorder(btnSeletedIndex);

            void SetButtonText(int selectedIndex)
            {
                var (index, total) = (PageIndex, TotalPage);
                if (total <= 5)
                {
                    Button1.Content = One;
                    Button2.Content = Two;
                    Button3.Content = Three;
                    Button4.Content = Four;
                    Button5.Content = Five;
                    return;
                }
                switch (selectedIndex)
                {
                    case 0:
                        Button1.Content = (index + 1).ToString();
                        Button2.Content = (index + 2).ToString();
                        Button3.Content = (index + 3).ToString();
                        Button4.Content = (index + 4).ToString();
                        Button5.Content = More;
                        break;
                    case 1:
                        Button1.Content = index.ToString();
                        Button2.Content = (index + 1).ToString();
                        Button3.Content = (index + 2).ToString();
                        Button4.Content = (index + 3).ToString();
                        Button5.Content = More;
                        break;
                    case 2:
                        Button1.Content = index > 2 ? More : (index - 1).ToString();
                        Button2.Content = index.ToString();
                        Button3.Content = (index + 1).ToString();
                        Button4.Content = (index + 2).ToString();
                        Button5.Content = (index + 3 < total) ? More : total.ToString();
                        break;
                    case 3:
                        Button1.Content = More;
                        Button2.Content = (total - 3).ToString();
                        Button3.Content = (total - 2).ToString();
                        Button4.Content = (total - 1).ToString();
                        Button5.Content = total.ToString();
                        break;
                    case 4:
                        Button1.Content = More;
                        Button2.Content = (total - 3).ToString();
                        Button3.Content = (total - 2).ToString();
                        Button4.Content = (total - 1).ToString();
                        Button5.Content = total.ToString();
                        break;
                    default:
                        throw new NotImplementedException();
                };
            }

            void SetButtonBorder(int selectedIndex)
            {
                foreach (var btn in new[] { Button1, Button2, Button3, Button4, Button5 })
                {
                    SetButtonBorderBrush(btn);
                }

                void SetButtonBorderBrush(Button btn)
                    => btn.BorderBrush = GetBrush(GetIndexOfButton(btn));

                Brush GetBrush(int index)
                    => index == selectedIndex ? _selectedBrush : _normalBrush;
            }
        }

        private void SetButtonVisible()
        {
            Button1.Visibility = Visibility.Visible;

            foreach (var btn in new[] { Button2, Button3, Button4, Button5 })
            {
                SetButtonVisible(btn);
            }

            void SetButtonVisible(Button btn)
                => btn.Visibility = CheckVisible(GetIndexOfButton(btn) + 1);
            Visibility CheckVisible(int total)
                => TotalPage >= total ? Visibility.Visible : Visibility.Collapsed;
        }

        private int GetIndexOfButton(Button button)
            => Convert.ToInt32(button.Tag);

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            var button = (Button)sender;

            if (GetIndexOfButton(button) != CalcSelectedPageButtonIndex())
            {
                button.BorderBrush = _normalBrush;
            }
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            var button = (Button)sender;

            if (GetIndexOfButton(button) != CalcSelectedPageButtonIndex())
            {
                button.BorderBrush = _hover;
            }
        }

        private void ButtonPreviousPage_Click(object sender, RoutedEventArgs e)
            => PageIndex -= 1;

        private void ButtonNextPage_Click(object sender, RoutedEventArgs e)
            => PageIndex += 1;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PageIndex = GetClickedPageIndex();

            int GetClickedPageIndex()
                => sender switch
                {
                    var btn when btn == Button1
                        => (string)Button1.Content == More ? Convert.ToInt32(Button2.Content) - 2 : Convert.ToInt32(Button1.Content) - 1,
                    var btn when btn == Button2 => Convert.ToInt32(Button2.Content) - 1,
                    var btn when btn == Button3 => Convert.ToInt32(Button3.Content) - 1,
                    var btn when btn == Button4 => Convert.ToInt32(Button4.Content) - 1,
                    var btn when btn == Button5
                        => (string)Button5.Content == More ? Convert.ToInt32(Button4.Content) : Convert.ToInt32(Button5.Content) - 1,
                    _ => 0
                };
        }

        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Index_Input.Text, out var input) || !IsValid(input))
            {
                return;
            }
            PageIndex = GetRealIndex(input);

            int GetRealIndex(int i) => input - 1;
            bool IsValid(int i) => i >= 1 && i <= TotalPage;
        }
    }
}
