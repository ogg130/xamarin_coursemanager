using System;
using System.Globalization;
using Xamarin.Forms;

namespace CourseManager.Converters
{
    public class DateTimeToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateWithTime = (DateTime)value;
            return dateWithTime.Date.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}