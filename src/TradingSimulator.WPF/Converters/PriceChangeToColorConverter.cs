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
                    return Brushes.Green; // Zysk
                if (decimalValue < 0)
                    return Brushes.Red;   // Strata
            }
            return Brushes.Black; // Zero lub błąd
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}