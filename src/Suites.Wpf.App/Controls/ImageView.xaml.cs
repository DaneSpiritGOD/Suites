using System;
using System.Text;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Suites.Wpf.Controls
{
    /// <summary>
    /// ImageView.xaml 的交互逻辑
    /// </summary>
    public partial class ImageView : UserControl, INotifyPropertyChanged
    {
        const double power = 1.03;
        const double powerNeg = 1 / power;
        const double minScaleValue = 0.01;

        public ImageView()
        {
            InitializeComponent();
        }

        #region Event
        //private void scrollViewer_Loaded(object sender, RoutedEventArgs e)
        //{
        //image.Width = scrollViewer.ActualWidth;
        //image.Height = scrollViewer.ActualHeight;
        //}

        private bool _mouseLBPressed;
        private Point _prevMouseRelativeLocation;

        /// <summary>
        /// 鼠标按下时的事件，启用捕获鼠标位置并把坐标赋值给mouseXY.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            image.CaptureMouse();
            _mouseLBPressed = true;
            _prevMouseRelativeLocation = e.GetPosition(scrollViewer);
        }

        /// <summary>
        /// 鼠标松开时的事件，停止捕获鼠标位置。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            image.ReleaseMouseCapture();
            _mouseLBPressed = false;
        }

        private bool isControlKeyPressed()
        {
            return !CtrlNeeded || (Keyboard.Modifiers == ModifierKeys.Control);
        }

        /// <summary>
        /// 鼠标移动时的事件，当鼠标按下并移动时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(image);
            var renderScale = (Source as BitmapSource).PixelHeight / image.ActualHeight;
            genStatusBarContent(point, renderScale, Source as BitmapSource);

            if (_mouseLBPressed && e.LeftButton == MouseButtonState.Pressed && isControlKeyPressed())
            {
                var positionRelatedToImageContainer = e.GetPosition(scrollViewer);

                //2017/6/6：
                //防止将图片移出容器，无法移回
                if (positionRelatedToImageContainer.X <= 0 ||
                    positionRelatedToImageContainer.Y <= 0 ||
                    positionRelatedToImageContainer.X > scrollViewer.ActualWidth ||
                    positionRelatedToImageContainer.Y > scrollViewer.ActualHeight
                    )
                    return;

                translateTransform.X -= _prevMouseRelativeLocation.X - positionRelatedToImageContainer.X;
                translateTransform.Y -= _prevMouseRelativeLocation.Y - positionRelatedToImageContainer.Y;
                _prevMouseRelativeLocation = positionRelatedToImageContainer;
            }
        }

        public static bool IsPointInControl(FrameworkElement control, Point point)
        {
            var controlWidth = control.ActualWidth;
            var controlHeight = control.ActualHeight;

            if (point.X > controlWidth ||
                point.X < 0 ||
                point.Y > controlHeight ||
                point.Y < 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 鼠标滑轮事件，得到坐标，放缩函数和滑轮指数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //将Handled设为True，则会将PreviewMouseWheel隧道事件终结，该事件不会冒泡到ScrollViewer上
            //但是这只针对于事件源控件为Image的情况，如果鼠标在ScrollViewer上进行滚动事件，该事件无论如何都不会传递到Image控件
            //因为Image是ScrollViewer的子元素，事件源为ScrollViewer，事件节点在ScrollViewer
            //2016.10.27 无论如何，(Preview)MouseWheel的目标节点为ScrollViewer，所以捕捉事件应该放到ScrollViewer中
            //2016.11.1 经过继承ScrollViewer的OnPreviewMouseWheel发现，Preview滚轮事件被ScrollViewer下的Grid处理，所以当然不会触发Image的（Preview/）MouseWheel，无语！
            if (!isControlKeyPressed()) return;
            e.Handled = true;

            //判断鼠标是否在Image控件有效区域内，如果不是不进行有效行为
            var pointRelatedToImageControl = e.GetPosition(image);//这里获得的是Image控件原始尺寸对应的坐标,不会缩小放大
            if (!IsPointInControl(image, pointRelatedToImageControl)) return;

            //var delta = e.Delta * 0.001;            
            double powerRT = System.Math.Sign(e.Delta) > 0 ? power : powerNeg;

            //if (scaleTransform.ScaleX + delta < 0.01) return;
            if (scaleTransform.ScaleX * powerRT < minScaleValue) return;

            var p1 = scaleTransform.Transform(pointRelatedToImageControl);//获得缩放前的实际坐标，相对于固定坐标系

            //scaleTransform.ScaleX += delta;
            //scaleTransform.ScaleY += delta;
            scaleTransform.ScaleX *= powerRT;
            scaleTransform.ScaleY *= powerRT;

            var p2 = scaleTransform.Transform(pointRelatedToImageControl);//获得缩放后的实际坐标，相对于固定坐标系

            //p1、p2相减即为图像的呈现相对于固定坐标系的位移
            translateTransform.X -= p2.X - p1.X;
            translateTransform.Y -= p2.Y - p1.Y;
        }
        #endregion Event

        #region Property
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageView), new PropertyMetadata(null));

        public bool CtrlNeeded
        {
            get { return (bool)GetValue(CtrlNeededProperty); }
            set { SetValue(CtrlNeededProperty, value); }
        }

        public static readonly DependencyProperty CtrlNeededProperty =
            DependencyProperty.Register("CtrlNeeded", typeof(bool), typeof(ImageView), new PropertyMetadata(true));

        #region StatusBar
        public Visibility ShowStatusBar
        {
            get
            {
                return (Visibility)GetValue(ShowStatusBarProperty);
            }
            set
            {
                SetValue(ShowStatusBarProperty, value);
            }
        }

        public static readonly DependencyProperty ShowStatusBarProperty =
            DependencyProperty.Register(
                "ShowStatusBar",
                typeof(Visibility),
                typeof(ImageView),
                new PropertyMetadata(Visibility.Collapsed));

        private void genStatusBarContent(Point p, double scale, BitmapSource bs)
        {
            if (ShowStatusBar != Visibility.Visible)
            {
                LocationContent = string.Empty;
                ColorContent = string.Empty;
                SizeContent = string.Empty;
                return;
            }

            var x = Math.Max(0, Math.Min((int)(p.X * scale), bs.PixelWidth - 1));
            var y = Math.Max(0, Math.Min((int)(p.Y * scale), bs.PixelHeight - 1));
            var value = bs.CopyPixel(x, y);
            LocationContent = genBracketPair(LocationHeader, x, y);
            ColorContent = genBracketPair(ColorHeader, value.Red, value.Green, value.Blue);
            SizeContent = genBracketPair(SizeHeader, bs.PixelWidth, bs.PixelHeight, bs.GetChannelCount());
        }

        private static string genBracketPair<T>(string head, params T[] list)
        {
            var sb = new StringBuilder();
            sb.Append($"{head}:(");
            for (var i = 0; i < list.Length - 1; ++i)
            {
                sb.Append($"{list[i]},");
            }
            sb.Append($"{list[list.Length - 1]})");
            return sb.ToString();
        }

        private static T[] genBracketPairParams<T>(int num, T item)
        {
            var items = new T[num];
            for (var i = 0; i < num; ++i)
            {
                items[i] = item;
            }
            return items;
        }

        const string SizeHeader = "S";
        string _sizeContent = genBracketPair(SizeHeader, genBracketPairParams(3, '-'));
        public string SizeContent
        {
            get { return _sizeContent; }
            set { SetProperty(ref _sizeContent, value); }
        }

        const string ColorHeader = "C";
        string _colorContent = genBracketPair(ColorHeader, genBracketPairParams(3, '-'));
        public string ColorContent
        {
            get { return _colorContent; }
            set { SetProperty(ref _colorContent, value); }
        }

        const string LocationHeader = "L";
        string _locationContent = genBracketPair(LocationHeader, genBracketPairParams(2, '-'));
        public string LocationContent
        {
            get { return _locationContent; }
            set { SetProperty(ref _locationContent, value); }
        }
        #endregion
        #endregion Property

        #region INotifyPropertyChanged Snippet
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void SetProperty<T>(ref T item, T value, [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(item, value))
            {
                item = value;
                OnPropertyChanged(propertyName);
            }
        }
        #endregion
    }
}
