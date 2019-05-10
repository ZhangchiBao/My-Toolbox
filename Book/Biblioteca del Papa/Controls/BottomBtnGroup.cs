using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Biblioteca_del_Papa.Controls
{
    public class BottomBtnGroup : Control
    {
        static BottomBtnGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BottomBtnGroup), new FrameworkPropertyMetadata(typeof(BottomBtnGroup)));
        }

        /// <summary>
        /// 取消命令
        /// </summary>
        [Description("取消命令")]
        [Category("Action")]
        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(BottomBtnGroup), new PropertyMetadata(null));

        /// <summary>
        /// 确定命令
        /// </summary>
        [Description("确定命令")]
        [Category("Action")]
        public ICommand ConfirmCommand
        {
            get { return (ICommand)GetValue(ConfirmCommandProperty); }
            set { SetValue(ConfirmCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConfirmCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfirmCommandProperty =
            DependencyProperty.Register("ConfirmCommand", typeof(ICommand), typeof(BottomBtnGroup), new PropertyMetadata(null));

        /// <summary>
        /// 取消按钮内容
        /// </summary>
        [Description("取消按钮内容")]
        public object CancelButtonContent
        {
            get { return (object)GetValue(CancelButtonContentProperty); }
            set { SetValue(CancelButtonContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelButtonContentProperty =
            DependencyProperty.Register("CancelButtonContent", typeof(object), typeof(BottomBtnGroup));

        /// <summary>
        /// 确定按钮内容
        /// </summary>
        [Description("确定按钮内容")]
        public object ConfirmButtonContent
        {
            get { return (object)GetValue(ConfirmButtonContentProperty); }
            set { SetValue(ConfirmButtonContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConfirmButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfirmButtonContentProperty =
            DependencyProperty.Register("ConfirmButtonContent", typeof(object), typeof(BottomBtnGroup));

        /// <summary>
        /// 按钮间距
        /// </summary>
        [Description("按钮间距")]
        public Thickness ButtonMargin
        {
            get { return (Thickness)GetValue(ButtonMarginProperty); }
            set { SetValue(ButtonMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonMarginProperty =
            DependencyProperty.Register("ButtonMargin", typeof(Thickness), typeof(BottomBtnGroup), new PropertyMetadata(new Thickness(5, 5, 5, 5)));

        [Description("按钮样式")]
        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(BottomBtnGroup));


    }
}
