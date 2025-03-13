using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ForTheLife.Converters;

public class SaleToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var lowerSaleColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffea00"));
        var highterSaleColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff5500"));

        var currentSale = (int)value;
        if (currentSale >= 50) return highterSaleColor;

        return lowerSaleColor;       
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
