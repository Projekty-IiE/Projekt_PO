using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace TradingSimulator.WPF.Converters
{
    internal class MessageToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string message && !string.IsNullOrEmpty(message))
            {
                if (message.StartsWith("Success", StringComparison.OrdinalIgnoreCase) ||
                    message.StartsWith("SOLD", StringComparison.OrdinalIgnoreCase))
                {
                    return Brushes.Green;
                }
                else { return Brushes.Red; }
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
