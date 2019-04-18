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
    public partial class Paging : UserControl
    {
        public delegate dynamic PageQuery(int page, int size);

        private const int MIN_PAGE_INDEX = 1;
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

        public Paging()
        {
            InitializeComponent();
        }

        //每页显示多少条
        private int _pageSize = 10;

        //当前是第几页
        private int _pageIndex = 1;

        //DataGrid控件对象
        private DataGrid _grdList;

        //最大页数
        private int _maxIndex = 1;

        //一共多少条
        private int _allNum;

        //查询
        public event PageQuery Query;

        #region 属性
        private static readonly Type _thisType = typeof(Paging);
        public static readonly DependencyProperty TotalPageProperty =
            DependencyProperty.Register("TotalPage", typeof(int), _thisType,
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
            //var pager = (Paging)dependencyObject;
            //if (pager == null)
            //{
            //    return;
            //}

            //pager.setButtonVisible();

            //if (pager.PageIndex + 1 > pager.TotalPage)
            //{
            //    return;
            //}

            //pager.ButtonPreviousPage.IsEnabled = pager.PageIndex > 0;
            //pager.ButtonNextPage.IsEnabled = pager.PageIndex + 1 < pager.TotalPage;

            //int currentPageButtonIndex = getCurrentPageButtonIndex(pager);
            //setButtonText(pager, currentPageButtonIndex);
            //pager.SetButtonBorder(currentPageButtonIndex);
        }

        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), _thisType, new PropertyMetadata(0, OnPageIndexPropertyChanged));
        public int PageIndex
        {
            get => (int)GetValue(PageIndexProperty);
            set => SetValue(PageIndexProperty, value);
        }

        private static void OnPageIndexPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            //var pager = (Paging)dependencyObject;
            //if (pager == null)
            //{
            //    return;
            //}

            //if (pager.PageIndex + 1 > pager.TotalPage)
            //{
            //    return;
            //}

            //pager.ButtonPreviousPage.IsEnabled = pager.PageIndex > 0;
            //pager.ButtonNextPage.IsEnabled = pager.PageIndex + 1 < pager.TotalPage;

            //int currentPageButtonIndex = getCurrentPageButtonIndex(pager);
            //setButtonText(pager, currentPageButtonIndex);
            //pager.SetButtonBorder(currentPageButtonIndex);
        }

        public int MaxPageIndex
        {
            get => GetMaxPageIndex(TotalPage, MIN_PAGE_INDEX);
        }

        private int CoercePageIndex(int pageIndex)
        {
            return Math.Max(Math.Min(pageIndex, MaxPageIndex), MIN_PAGE_INDEX);
        }
        #endregion 属性

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="total"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public void ShowPages(DataGrid grd, int total, int pageIndex, int pageSize)
        {
            _grdList = grd;
            _pageSize = pageSize;
            _pageIndex = pageIndex;
            _allNum = total;
            //SetMaxIndex();
            DisplayPagingInfo();
            if (_maxIndex > 1)
            {
                pageGo.IsEnabled = true;
                btnGo.IsEnabled = true;
            }
        }

        /// <summary>
        /// 画每页显示等数据
        /// </summary>
        private void DisplayPagingInfo()
        {
            if (_pageIndex == 1)
            {
                btnPrev.IsEnabled = false;
            }
            else
            {
                btnPrev.IsEnabled = true;
            }
            if (_pageIndex == _maxIndex)
            {
                btnNext.IsEnabled = false;
            }
            else
            {
                btnNext.IsEnabled = true;
            }

            var first = 1;
            var last = _maxIndex;
            grid.Children.Clear();
            if (_maxIndex <= 8)
            {
                for (var i = first; i <= last; i++)
                {
                    NumberDisplay(i);
                }
            }
            else
            {
                if (_pageIndex < 5)   //选中页位于前三页   1~3
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
                if (_pageIndex > 5 && _pageIndex < last - 3)  //选中页位于中间页码 
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
                        else if (i >= _pageIndex - 2 && i <= _pageIndex + 2)
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
                else if (_pageIndex > last - 5)   //选中页位于最后三页
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
                else if (_pageIndex == 5)
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
                        else if (i >= _pageIndex - 2 && i <= _pageIndex + 2)
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
            tbl.Click += new RoutedEventHandler(tbl_Click);
            if (i == _pageIndex)
            {
                tbl.IsEnabled = false;
            }

            Grid.SetColumn(tbl, grid.ColumnDefinitions.Count - 1);
            Grid.SetRow(tbl, 0);
            grid.Children.Add(tbl);
        }

        private double CalcPageButtonWidth(int pageIndex)
        {
            if (0 < pageIndex && pageIndex < 10)
            {
                return 22;
            }
            else if (10 <= pageIndex && pageIndex < 100)
            {
                return 32;
            }
            else if (100 <= pageIndex && pageIndex < 1000)
            {
                return 36;
            }
            else if (1000 <= pageIndex && pageIndex < 10000)
            {
                return 44;
            }
            else if (10000 <= pageIndex && pageIndex < 100000)
            {
                return 52;
            }
            else if (100000 <= pageIndex && pageIndex < 1000000)
            {
                return 58;
            }
            else
            {
                return 65;
            }
        }
        private void CreateNewPageButton(int pageIndex, double width)
        {
            var cdf = new ColumnDefinition();
            grid.ColumnDefinitions.Add(cdf);
            var pageButton = new Button
            {
                BorderThickness = new Thickness(1),
                Height = 26,
                Width = CalcPageButtonWidth(pageIndex),
                Content = (pageIndex + 1).ToString(),
                Style = (Style)FindResource("PageTextBlock3"),
                IsEnabled = pageIndex != _pageIndex
            };

            pageButton.Click += tbl_Click;

            Grid.SetColumn(pageButton, grid.ColumnDefinitions.Count - 1);
            grid.Children.Add(pageButton);
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
        private void tbl_Click(object sender, RoutedEventArgs e)
        {
            var text = ((ContentControl)e.Source).Content as string;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var index = int.Parse(text);
            _pageIndex = index;
            if (index > _maxIndex)
            {
                _pageIndex = _maxIndex;
            }

            if (index < 1)
            {
                _pageIndex = 1;
            }
        }
    }
}
