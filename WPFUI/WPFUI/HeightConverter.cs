using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPFUI
{
    public class HeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double gridHeight)
            {
                // Subtract heights of fixed elements:
                // - Stats Cards: 90px (Height) + 24px (Margin)
                // - Control Panel: 82px (Height) + 24px (Margin)
                // - Header and Search: ~60px (Margin: 24px top/bottom + content)
                // - TextBlock: ~40px (Padding + content)
                double fixedHeight = 90 + 24 + 82 + 24 + 60 + 40; // Total ~320px
                return Math.Max(200, gridHeight - fixedHeight); // Minimum height 200
            }
            return 200.0; // Fallback
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
