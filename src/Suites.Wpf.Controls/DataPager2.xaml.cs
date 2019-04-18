using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Suites.Wpf.Controls
{
    /// <summary>
    /// 分页控件   事件驱动
    /// </summary>
    public partial class DataPager2 : UserControl
    {
        /// <summary>
        /// 最小页码索引
        /// </summary>
        private const int MIN_PAGE_INDEX = 0;

        /// <summary>
        /// PageIndex默认值
        /// </summary>
        private const int DEFAULT_INITIALED_PAGE_INDEX = 0;

        /// <summary>
        /// TotalPage默认值
        /// </summary>
        private const int DEFAULT_INITIALED_TOTAL_PAGE = 0;
        private static int GetMaxPageIndex(int totalPageCount, int minPageIndex)
        {
            return Math.Max(minPageIndex + totalPageCount - 1, minPageIndex);
        }

        private static bool IsValidPageIndex(int pageIndex, int minPageIndex, int maxPageIndex)
        {
            return pageIndex <= maxPageIndex && pageIndex >= minPageIndex;
        }

        private static int CoercePageIndex(int pageIndex, int minPageIndex, int maxPageIndex)
        {
            return Math.Max(Math.Min(pageIndex, maxPageIndex), MIN_PAGE_INDEX);
        }

        public DataPager2()
        {
            InitializeComponent();
        }

        #region 属性
        private static readonly Type _thisType = typeof(DataPager2);
        public static readonly DependencyProperty TotalPageProperty =
            DependencyProperty.Register("TotalPage", typeof(int), _thisType,
                new PropertyMetadata(DEFAULT_INITIALED_PAGE_INDEX,
                    (d, e) => ((DataPager2)d).OnTotalPagePropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int TotalPage
        {
            get => (int)GetValue(TotalPageProperty);
            set => SetValue(TotalPageProperty, value);
        }

        /// <summary>
        /// 总页数阈值1，当总页数小于该阈值时，展示所有页码按钮
        /// </summary>
        private const int MAX_PAGE_BUTTON = 10;

        private Button CreateNewPageButton(int pageIndex, int columnIndex)
        {
            var pageButton = new Button
            {
                BorderThickness = new Thickness(1),
                Height = 26,
                Padding = new Thickness(3, 0, 3, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = GetPageBttonContentFromPageIndex(pageIndex, columnIndex),
                Tag = pageIndex,
                Style = (Style)FindResource("PageTextBlock3"),
                IsEnabled = pageIndex != PageIndex
            };

            pageButton.Click += pageIndexButton_Click;
            Grid.SetColumn(pageButton, columnIndex);

            return pageButton;
        }

        private int GetPageBttonContentFromPageIndex(int pageIndex, int columnIndex)
        {
            return MIN_PAGE_INDEX == 0 ? pageIndex + 1 : pageIndex;
        }

        private void RefreshBttonEnabled(int pageIndex, int totalPage)
        {
            var goButtonEnabled = GetGoButtonEnabled();
            pageGo.IsEnabled = goButtonEnabled;
            btnGo.IsEnabled = goButtonEnabled;

            btnPrev.IsEnabled = GetPrevButtonEnabled();
            btnNext.IsEnabled = GetNextButtonEnabled();

            bool GetGoButtonEnabled() => totalPage > 1;
            bool GetPrevButtonEnabled() => pageIndex != MIN_PAGE_INDEX;
            bool GetNextButtonEnabled() => pageIndex != totalPage;
        }

        private void OnTotalPagePropertyChanged(int oldTotalPage, int newTotalPage)
        {
            if (oldTotalPage == newTotalPage)
                return;

            RefreshBttonEnabled(PageIndex, newTotalPage);

            grid.ColumnDefinitions.Clear();
            grid.Children.Clear();

            var count = Math.Max(MAX_PAGE_BUTTON, newTotalPage);
            for (var index = 0; index < count; ++index)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.Children.Add(CreateNewPageButton(index, index));
            }


        }

        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), _thisType,
                new PropertyMetadata(DEFAULT_INITIALED_TOTAL_PAGE,
                    (d, e) => ((DataPager2)d).OnPageIndexPropertyChanged((int)e.OldValue, (int)e.NewValue),
                    (d, v) => ((DataPager2)d).CoercePageIndexChanged((int)v)));

        public int PageIndex
        {
            get => (int)GetValue(PageIndexProperty);
            set => SetValue(PageIndexProperty, value);
        }

        private void OnPageIndexPropertyChanged(int oldValue, int newValue)
        {
            if (oldValue == newValue)
                return;

            RefreshBttonEnabled(newValue, TotalPage);


        }

        private int CoercePageIndexChanged(int newValue)
        {
            return CoercePageIndex(newValue);
        }

        public int MaxPageIndex
        {
            get => GetMaxPageIndex(TotalPage, MIN_PAGE_INDEX);
        }

        private int CoercePageIndex(int pageIndex)
        {
            return CoercePageIndex(pageIndex, MIN_PAGE_INDEX, MaxPageIndex);
        }
        #endregion 属性        

        /// <summary>
        /// 画每页显示等数据
        /// </summary>
        private void RefreshUI()
        {
            var goButtonEnabled = GetGoButtonEnabled();
            pageGo.IsEnabled = goButtonEnabled;
            btnGo.IsEnabled = goButtonEnabled;

            btnPrev.IsEnabled = GetPrevButtonEnabled();
            btnNext.IsEnabled = GetNextButtonEnabled();

            var first = 1;
            var last = TotalPage;

            if (IsTotalPageLowerEqualThan8())
            {
                DisplayAllPageButton(TotalPage);
            }
            else
            {
                if (PageIndex < 5) //选中页位于前三页   1~3
                {
                    for (var i = first; i <= last; i++)   //初始化分页
                    {
                        if (i < 6)
                        {
                            NumberDisplay(i);
                        }
                        else if (i == 6)
                        {
                            EllipsisDisplay();
                        }
                        else if (i == last)
                        {
                            NumberDisplay(i);
                        }
                    }
                }
                if (PageIndex > 5 && PageIndex < last - 3)  //选中页位于中间页码 
                {
                    for (var i = first; i <= last; i++)   //初始化分页
                    {
                        if (i == 1)
                        {
                            NumberDisplay(i);
                        }
                        else if (i == 2)
                        {
                            EllipsisDisplay();
                        }
                        else if (i >= PageIndex - 2 && i <= PageIndex + 2)
                        {
                            NumberDisplay(i);
                        }
                        else if (i == last - 1)
                        {
                            EllipsisDisplay();
                        }
                        else if (i == last)
                        {
                            NumberDisplay(i);
                        }
                    }
                }
                else if (PageIndex > last - 5)   //选中页位于最后三页
                {
                    for (var i = first; i <= last; i++)   //初始化分页
                    {
                        if (i == 1)
                        {
                            NumberDisplay(i);
                        }
                        else if (i == 2)
                        {
                            EllipsisDisplay();
                        }
                        else if (i > last - 5)
                        {
                            NumberDisplay(i);
                        }
                    }
                }
                else if (PageIndex == 5)
                {
                    for (var i = first; i <= last; i++)   //初始化分页
                    {
                        if (i == 1)
                        {
                            NumberDisplay(i);
                        }
                        else if (i == 2)
                        {
                            EllipsisDisplay();
                        }
                        else if (i == last - 1)
                        {
                            EllipsisDisplay();
                        }
                        else if (i >= PageIndex - 2 && i <= PageIndex + 2)
                        {
                            NumberDisplay(i);
                        }

                        else if (i == last)
                        {
                            NumberDisplay(i);
                        }
                    }
                }
            }

            bool GetGoButtonEnabled() => TotalPage > 1;
            bool IsTotalPageLowerEqualThan8() => TotalPage <= MAX_PAGE_BUTTON;
            bool GetPrevButtonEnabled() => PageIndex != MIN_PAGE_INDEX;
            bool GetNextButtonEnabled() => PageIndex != TotalPage;

            void ReInitialGrid2()
            {
                grid.ColumnDefinitions.Clear();
                grid.Children.Clear();
            }

            void DisplayAllPageButton(int count)
            {
                ReInitialGrid2();
                for (var i = 0; i <= count - 1; i++)
                {
                    CreateNewPageButton2(i);
                }
            }

            void CreateNewPageButton2(int pageIndex)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.Children.Add(CreateNewPageButton(pageIndex, pageIndex));
            }
        }

        /// <summary>
        /// 画显示数据模板
        /// </summary>
        /// <param name="i"></param>
        private void NumberDisplay(int i)
        {
            var cdf = new ColumnDefinition();
            grid.ColumnDefinitions.Add(cdf);
            var tbl = new Button
            {
                BorderThickness = new Thickness(1),
                Height = 26
            };
            if (0 < i && i < 10)
            {
                tbl.Width = 22;
            }
            else if (10 <= i && i < 100)
            {
                tbl.Width = 32;
            }
            else if (100 <= i && i < 1000)
            {
                tbl.Width = 36;
            }
            else if (1000 <= i && i < 10000)
            {
                tbl.Width = 44;
            }
            else if (10000 <= i && i < 100000)
            {
                tbl.Width = 52;
            }
            else if (100000 <= i && i < 1000000)
            {
                tbl.Width = 58;
            }
            else
            {
                tbl.Width = 65;
            }
            tbl.Content = i.ToString();
            tbl.Style = FindResource("PageTextBlock3") as Style;
            tbl.Click += pageIndexButton_Click;
            if (i == PageIndex)
            {
                tbl.IsEnabled = false;
            }

            Grid.SetColumn(tbl, grid.ColumnDefinitions.Count - 1);
            Grid.SetRow(tbl, 0);
            grid.Children.Add(tbl);
        }

        /// <summary>
        /// 画显示数据模板
        /// </summary>
        private void EllipsisDisplay()
        {
            var cdf = new ColumnDefinition();
            grid.ColumnDefinitions.Add(cdf);
            var tbk = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5, 0, 0, 10),
                Width = 12,
                Height = 12,
                Text = "…"
            };

            Grid.SetColumn(tbk, grid.ColumnDefinitions.Count - 1);
            Grid.SetRow(tbk, 0);
            grid.Children.Add(tbk);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex <= MIN_PAGE_INDEX)
            {
                return;
            }

            PageIndex--;
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex >= MaxPageIndex)
            {
                return;
            }

            PageIndex++;
        }

        /// <summary>
        /// 跳转到多少页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(pageGo.Text, out var index))
            {
                PageIndex = CoercePageIndex(index);
            }
            pageGo.Clear();
        }

        /// <summary>
        /// 分页数字的点击触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pageIndexButton_Click(object sender, RoutedEventArgs e)
        {
            PageIndex = (int)((Button)sender).Tag;
        }
    }
}
