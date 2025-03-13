using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ForTheLife.Converters;

public class ImageUrlConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var baseImagesUri = Path.Combine(Environment.CurrentDirectory, "Resources\\Images");

        var defaultImageName = "picture.png";
        var defaultImagePath = Path.Combine(baseImagesUri, defaultImageName);

        var imageName = value as string;
        if (string.IsNullOrWhiteSpace(imageName)) return defaultImagePath;

        var imagePath = Path.Combine(baseImagesUri, imageName);
        return imagePath;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
