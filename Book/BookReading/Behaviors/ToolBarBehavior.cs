using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace BookReading.Behaviors
{
    public class ToolBarBehavior : DependencyObject
    {
        //protected override void OnAttached()
        //{
        //    base.OnAttached();
        //    this.AssociatedObject.Loaded += AssociatedObject_Loaded;
        //}

        //private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (sender is ToolBar toolBar)
        //    {
        //        var obj = toolBar.Template.FindName("OverflowGrid", toolBar);
        //        if (obj is FrameworkElement overflowGrid)
        //        {
        //            overflowGrid.Visibility = OverflowGridVisibility;
        //        }
        //    }
        //}

        //public Visibility OverflowGridVisibility
        //{
        //    get { return (Visibility)GetValue(OverflowGridVisibilityProperty); }
        //    set { SetValue(OverflowGridVisibilityProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for OverflowGridVisibility.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty OverflowGridVisibilityProperty =
        //    DependencyProperty.Register("OverflowGridVisibility", typeof(Visibility), typeof(ToolBarBehavior), new PropertyMetadata(default(Visibility), OnOverflowGridVisibilityChanged));

        //private static void OnOverflowGridVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is ToolBar toolBar)
        //    {
        //        var obj = toolBar.Template.FindName("OverflowGrid", toolBar);
        //        if (obj is FrameworkElement overflowGrid)
        //        {
        //            overflowGrid.Visibility = (Visibility)e.NewValue;
        //        }
        //    }
        //}

        public static Visibility GetOverflowGridVisibility(DependencyObject obj)
        {
            return (Visibility)obj.GetValue(OverflowGridVisibilityProperty);
        }

        public static void SetOverflowGridVisibility(DependencyObject obj, Visibility value)
        {
            obj.SetValue(OverflowGridVisibilityProperty, value);
        }

        // Using a DependencyProperty as the backing store for OverflowGridVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverflowGridVisibilityProperty =
            DependencyProperty.RegisterAttached("OverflowGridVisibility", typeof(Visibility), typeof(ToolBarBehavior), new PropertyMetadata(Visibility.Visible, OnOverflowGridVisibilityChanged));

        private static void OnOverflowGridVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ToolBar toolBar)
            {
                try
                {
                    var obj = toolBar.Template.FindName("OverflowGrid", toolBar);
                    toolBar.Loaded -= ToolBar_Loaded;
                    if (obj is FrameworkElement overflowGrid)
                    {
                        overflowGrid.Visibility = GetOverflowGridVisibility(d);
                    }
                }
                catch
                {
                    toolBar.Loaded += ToolBar_Loaded;
                }
            }
        }

        private static void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ToolBar toolBar)
            {
                var obj = toolBar.Template.FindName("OverflowGrid", toolBar);
                if (obj is FrameworkElement overflowGrid)
                {
                    overflowGrid.Visibility = GetOverflowGridVisibility(toolBar);
                }
            }
        }
    }
}
