using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace WindowsIntegrityGuard.UI.Converters
{
    public sealed class IntegrityStatusToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value?.ToString() ?? string.Empty;

            return status switch
            {
                "Íntegro" => new SolidColorBrush(Color.FromRgb(42, 67, 61)),
                "Verificando" => new SolidColorBrush(Color.FromRgb(41, 63, 72)),
                "Corrupção detectada" => new SolidColorBrush(Color.FromRgb(72, 44, 50)),
                "Inconclusivo" => new SolidColorBrush(Color.FromRgb(70, 61, 42)),
                "Falha" => new SolidColorBrush(Color.FromRgb(72, 44, 50)),
                "Indisponível" => new SolidColorBrush(Color.FromRgb(72, 44, 50)),
                _ => new SolidColorBrush(Color.FromRgb(42, 52, 80))
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}