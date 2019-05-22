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

namespace CUI.Controls
{
    public class BusyBox : Control
    {
        static BusyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyBox), new FrameworkPropertyMetadata(typeof(BusyBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public string BusyContent
        {
            get { return (string)GetValue(BusyContentProperty); }
            set { SetValue(BusyContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BusyContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BusyContentProperty =
            DependencyProperty.Register("BusyContent", typeof(string), typeof(BusyBox), new PropertyMetadata(default(string)));


        public double RingWidth
        {
            get { return (double)GetValue(RingWidthProperty); }
            set { SetValue(RingWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RingWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RingWidthProperty =
            DependencyProperty.Register("RingWidth", typeof(double), typeof(BusyBox), new PropertyMetadata(default(double)));

        public double RingHeight
        {
            get { return (double)GetValue(RingHeightProperty); }
            set { SetValue(RingHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RingHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RingHeightProperty =
            DependencyProperty.Register("RingHeight", typeof(double), typeof(BusyBox), new PropertyMetadata(default(double)));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(BusyBox), new PropertyMetadata(default(bool), OnIsActiveChanged));

        private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BusyBox box)
            {
                if (e.NewValue is bool isActive)
                {
                    box.Visibility = isActive ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
    }
}
