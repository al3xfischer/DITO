using System;
using System.Globalization;
using System.Windows.Data;

namespace Client.Converter
{
    public class TimeStampConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeStamp = (DateTime)value;

            if (timeStamp == default) return null;

            return timeStamp;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
