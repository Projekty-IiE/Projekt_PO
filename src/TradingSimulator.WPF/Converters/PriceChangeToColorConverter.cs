using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TradingSimulator.WPF.Converters
{
    public class PriceChangeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                if (decimalValue > 0)
                    return Brushes.Green; // Profit
                if (decimalValue < 0)
                    return Brushes.Red;   // Loss
            }
            return Brushes.Black; // No change; error
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}