using System;
using System.Globalization;
using System.Windows.Data;

namespace Book.Converters
{
    public class ChapterWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double num && int.TryParse(parameter.ToString(), out int w))
            {
                if (num > 150)
                {
                    var p = (int)(num / w);
                    while (p > 5)
                    {
                        w += 10;
                        p = (int)(num / w);
                    }
                    return (num - p * 5) / p;
                }
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
