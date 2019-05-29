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
    public class CWindow : Window
    {
        static CWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CWindow), new FrameworkPropertyMetadata(typeof(CWindow)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Loaded += CWindow_Loaded;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Background")
            {
                //var current = ((Brush)e.NewValue).CloneCurrentValue();
                var currentColor = ((SolidColorBrush)e.NewValue).Color;
                var colorString = currentColor.ToString();
                if (SupportTranslucent)
                {
                    colorString = "#CC" + colorString.Substring(3);
                }
                else
                {
                    colorString = "#FF" + colorString.Substring(3);
                }

                //if (SupportTranslucent)
                //{
                //    current.Opacity = 0.8;
                //}
                //else
                //{
                //    current.Opacity = 1;
                //}
                if (setFromControl)
                {
                    setFromControl = false;
                    //Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorString));
                }
                setFromControl = true;
            }
            base.OnPropertyChanged(e);
        }

        private void CWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var current = Background.CloneCurrentValue();
            if (SupportTranslucent)
            {
                current.Opacity = 0.8;
            }
            else
            {
                current.Opacity = 1;
            }
            Background = current;
        }

        public bool SupportTranslucent
        {
            get { return (bool)GetValue(SupportTranslucentProperty); }
            set { SetValue(SupportTranslucentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SupportTranslucent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SupportTranslucentProperty =
            DependencyProperty.Register("SupportTranslucent", typeof(bool), typeof(CWindow), new PropertyMetadata(default(bool), OnSupportTranslucentChanged));
        private bool setFromControl;

        private static void OnSupportTranslucentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CWindow window)
            {
                if (window.IsLoaded)
                {
                    var currentBackground = window.Background.CloneCurrentValue();
                    if ((bool)e.NewValue)
                    {
                        currentBackground.Opacity = 0.8;
                    }
                    else
                    {
                        currentBackground.Opacity = 1;
                    }
                    window.Background = currentBackground;
                }
            }
        }
    }
}
