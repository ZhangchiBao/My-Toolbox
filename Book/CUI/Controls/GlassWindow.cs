using Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CUI.Controls
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = "PART_borderFrame", Type = typeof(Border))]
    public class GlassWindow : Window
    {
        static bool IsWin7 = false;
        static bool IsWin10 = false;
        #region 使用系统毛玻璃,Win10下可用
        internal enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }
        internal enum WindowCompositionAttribute
        {
            // ...
            WCA_ACCENT_POLICY = 19
            // ...
        }
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
        internal void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);

            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }
        #endregion
        #region 使用系统毛玻璃,Win7下可用
        [DllImport("DwmApi.dll")]
        static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);
        [DllImport("dwmapi.dll", PreserveSig = false)]
        static extern bool DwmIsCompositionEnabled();

        #endregion
        #region 窗体初始化
        static GlassWindow()
        {
            //启用自定义窗口风格
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GlassWindow), new FrameworkPropertyMetadata(typeof(GlassWindow)));
            //检测当前系统
            Version currentVersion = Environment.OSVersion.Version;
            Version compareToVersion = new Version("6.2");
            if (currentVersion.Major >= 6)
            {
                if (currentVersion.CompareTo(compareToVersion) > 0) //Win10系统
                {
                    IsWin10 = true; IsWin7 = false;
                }
                else if (currentVersion.MajorRevision < 2) //Win8之下的系统
                {
                    IsWin10 = false; IsWin7 = true;
                }
            }
            //用于测试非Win7/win10下的效果，无毛玻璃的透明效果
            //IsWin10 = false; IsWin7 = false;
        }
        public GlassWindow()
        {
            WindowStyle = WindowStyle.None;
            //根据系统不同，设定是否容许透明
            if (IsWin7) //Win7必须不透明，才能使用系统毛玻璃
            {
                AllowsTransparency = false;
            }
            else
            {
                AllowsTransparency = true;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            WindowResizer.Resizable(this);//窗口可调整类
            Border borderTitle = this.Template.FindName("borderTitle", this) as Border;
            if (borderTitle != null)
            {
                //鼠标拖动
                borderTitle.MouseMove += delegate (object sender, MouseEventArgs e)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        WindowResizer.DragMoveEx(this);
                    }
                };
                //鼠标双击事件
                borderTitle.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e)
                {
                    if (this.ResizeMode == ResizeMode.NoResize)
                    {
                        return;
                    }
                    if (e.ClickCount == 2)
                    {
                        MaxWindow();
                    }
                };
                //当状态发生改变和的操作
                this.StateChanged += delegate (object sender, EventArgs e)
                {
                    IsMax = WindowState == System.Windows.WindowState.Maximized;
                    IsNoMax = !IsMax;
                };
            }
            //装载时，需要检测是否为最大化
            this.Loaded += delegate (object sender, RoutedEventArgs e)
            {
                IsMax = WindowState == System.Windows.WindowState.Maximized;
                IsNoMax = !IsMax;
            };

            #region 启用系统毛玻璃效果      
            Border borderFrame = this.Template.FindName("PART_borderFrame", this) as Border;
            if (IsWin10)
            {
                EnableBlur();
                //设置背景绑定               
                Binding backGroundBinding = new Binding();
                backGroundBinding.Source = this;
                backGroundBinding.Path = new PropertyPath("GlassBackgroundBrush");
                borderFrame.SetBinding(Border.BackgroundProperty, backGroundBinding);
            }
            else if (IsWin7)
            {
                IntPtr mainWindowPtr = new WindowInteropHelper(this).Handle;
                HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
                mainWindowSrc.CompositionTarget.BackgroundColor = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);

                MARGINS margins = new MARGINS();
                margins.cxLeftWidth = -1;
                margins.cxRightWidth = -1;
                margins.cyTopHeight = -1;
                margins.cyBottomHeight = -1;

                DwmExtendFrameIntoClientArea(mainWindowSrc.Handle, ref margins);
                //设定内边框间距为0
                BorderThickness = new Thickness(0);
                //设置外边框为0
                borderFrame.BorderThickness = new Thickness(0);
                //设置右侧按钮
                borderTitle.Margin = new Thickness(0);
            }
            else
            {
                //设置背景绑定               
                Binding backGroundBinding = new Binding();
                backGroundBinding.Source = this;
                backGroundBinding.Path = new PropertyPath("GlassBackgroundBrush");
                borderFrame.SetBinding(Border.BackgroundProperty, backGroundBinding);

            }
            #endregion
        }

        #endregion

        #region 内容绑定
        /// <summary>
        /// Identifies the BackgroundContent dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleBackContentProperty = DependencyProperty.Register("TitleBackContent", typeof(object), typeof(GlassWindow));
        /// <summary>
        /// 获取或者设置标题区域背部内容
        /// </summary>
        public object TitleBackContent
        {
            get { return GetValue(TitleBackContentProperty); }
            set { SetValue(TitleBackContentProperty, value); }
        }
        /// <summary>
        /// Defines the ContentSource dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleRightContentProperty = DependencyProperty.Register("TitleRightContent", typeof(object), typeof(GlassWindow));
        /// <summary>
        ///获取或者设置标题区域右侧内容
        /// </summary>
        public object TitleRightContent
        {
            get { return GetValue(TitleRightContentProperty); }
            set { SetValue(TitleRightContentProperty, value); }
        }

        /// <summary>
        /// Defines the ContentSource dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleLeftContentProperty = DependencyProperty.Register("TitleLeftContent", typeof(object), typeof(GlassWindow));
        /// <summary>
        ///获取或者设置标题区域左侧内容
        /// </summary>
        public object TitleLeftContent
        {
            get { return GetValue(TitleLeftContentProperty); }
            set { SetValue(TitleLeftContentProperty, value); }
        }
        #endregion

        #region 窗口操作命令
        //最小化操作
        RelayCommand _MinWindowCommand;
        public RelayCommand MinWindowCommand
        {
            get
            {
                if (_MinWindowCommand == null)
                {
                    _MinWindowCommand = new RelayCommand(MinWindow, CanMin);
                }
                return _MinWindowCommand;
            }
            set
            {
                _MinWindowCommand = value;
            }
        }
        bool CanMin() //是否能够执行
        {
            return this.ResizeMode != ResizeMode.NoResize;
        }
        void MinWindow() //执行命令
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        //最大化操作
        RelayCommand _MaxWindowCommand;
        public RelayCommand MaxWindowCommand
        {
            get
            {
                if (_MaxWindowCommand == null)
                {
                    _MaxWindowCommand = new RelayCommand(MaxWindow, CanMax);
                }
                return _MaxWindowCommand;
            }
            set
            {
                _MaxWindowCommand = value;
            }
        }
        bool CanMax() //是否能够执行
        {
            return this.ResizeMode != ResizeMode.NoResize;
        }
        void MaxWindow() //执行命令
        {
            if (this.WindowState != System.Windows.WindowState.Maximized)
                this.WindowState = System.Windows.WindowState.Maximized;
            else
                this.WindowState = System.Windows.WindowState.Normal;
        }
        //关闭操作
        RelayCommand _CloseWindowCommand;
        public RelayCommand CloseWindowCommand
        {
            get
            {
                if (_CloseWindowCommand == null)
                {
                    _CloseWindowCommand = new RelayCommand(CloseWindow, CanClose);
                }
                return _CloseWindowCommand;
            }
            set
            {
                _CloseWindowCommand = value;
            }
        }
        bool CanClose() //是否能够执行
        {
            return true;
        }
        void CloseWindow() //执行命令
        {
            this.Close();
        }
        #endregion

        #region 窗口按钮属性设定
        [Description("设定最小化按钮的可见性")]
        [Category("控制按钮")]
        public Visibility MinButtonVisibility
        {
            get { return (Visibility)GetValue(MinButtonVisibilityProperty); }
            set
            {
                SetValue(MinButtonVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty MinButtonVisibilityProperty =
          DependencyProperty.Register("MinButtonVisibility", typeof(Visibility), typeof(GlassWindow), new PropertyMetadata(Visibility.Visible));
        [Description("设定最大化按钮的可见性")]
        [Category("控制按钮")]
        public Visibility MaxButtonVisibility
        {
            get { return (Visibility)GetValue(MaxButtonVisibilityProperty); }
            set
            {
                SetValue(MaxButtonVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty MaxButtonVisibilityProperty =
          DependencyProperty.Register("MaxButtonVisibility", typeof(Visibility), typeof(GlassWindow), new PropertyMetadata(Visibility.Visible));
        [Description("设定关闭按钮的可见性")]
        [Category("控制按钮")]
        public Visibility CloseButtonVisibility
        {
            get { return (Visibility)GetValue(CloseButtonVisibilityProperty); }
            set
            {
                SetValue(CloseButtonVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty CloseButtonVisibilityProperty =
          DependencyProperty.Register("CloseButtonVisibility", typeof(Visibility), typeof(GlassWindow), new PropertyMetadata(Visibility.Visible));
        //确定当前窗口是否为最大化
        public bool IsMax
        {
            get { return (bool)GetValue(IsMaxProperty); }
            set
            {
                SetValue(IsMaxProperty, value);
            }
        }
        public static readonly DependencyProperty IsMaxProperty =
          DependencyProperty.Register("IsMax", typeof(bool), typeof(GlassWindow), new PropertyMetadata(false));
        //确定当前窗口是否为正常化
        public bool IsNoMax
        {
            get { return (bool)GetValue(IsNoMaxProperty); }
            set
            {
                SetValue(IsNoMaxProperty, value);
            }
        }
        public static readonly DependencyProperty IsNoMaxProperty =
          DependencyProperty.Register("IsNoMax", typeof(bool), typeof(GlassWindow), new PropertyMetadata(true));
        #endregion
        #region 阴影相关，目前不用
        [Description("窗口边框阴影容纳尺寸")]
        [Category("窗口属性")]
        public Thickness WindowShadowMargin
        {
            get { return (Thickness)GetValue(WindowShadowMarginProperty); }
            set { SetValue(WindowShadowMarginProperty, value); }
        }
        public static readonly DependencyProperty WindowShadowMarginProperty =
          DependencyProperty.Register("WindowShadowMargin", typeof(Thickness), typeof(GlassWindow), new FrameworkPropertyMetadata(new Thickness(10)));
        #endregion
        #region 属于操作系统差异的属性
        [Description("设置窗口透明区域颜色")]
        public Brush GlassBackgroundBrush
        {
            get { return (Brush)GetValue(GlassBackgroundBrushProperty); }
            set { SetValue(GlassBackgroundBrushProperty, value); }
        }

        public static readonly DependencyProperty GlassBackgroundBrushProperty =
            DependencyProperty.Register("GlassBackgroundBrush", typeof(Brush), typeof(GlassWindow), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        #endregion

    }//结束类
}
