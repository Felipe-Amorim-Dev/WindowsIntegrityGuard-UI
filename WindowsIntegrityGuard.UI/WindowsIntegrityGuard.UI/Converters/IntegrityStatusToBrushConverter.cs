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
    public sealed class IntegrityStatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value?.ToString() ?? string.Empty;

            return status switch
            {
                "Íntegro" => new SolidColorBrush(Color.FromRgb(71, 214, 160)),
                "Verificando" => new SolidColorBrush(Color.FromRgb(81, 203, 197)),
                "Corrupção detectada" => new SolidColorBrush(Color.FromRgb(243, 108, 108)),
                "Inconclusivo" => new SolidColorBrush(Color.FromRgb(243, 200, 108)),
                "Falha" => new SolidColorBrush(Color.FromRgb(243, 108, 108)),
                "Indisponível" => new SolidColorBrush(Color.FromRgb(243, 108, 108)),
                _ => new SolidColorBrush(Color.FromRgb(174, 184, 202))
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}