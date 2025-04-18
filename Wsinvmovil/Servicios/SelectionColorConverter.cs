using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsinvmovil.Servicios
{
    public class SelectionColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string[] colors = (parameter as string)?.Split('|') ?? new[] { "#FFFF00", "#FFFFFF" };
            var selected = Colors.Yellow;
            var unselected = Colors.White;

            if (Color.TryParse(colors[0], out var c1)) selected = c1;
            if (colors.Length > 1 && Color.TryParse(colors[1], out var c2)) unselected = c2;

            return value is bool isSelected && isSelected ? selected : unselected;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
