using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PanelHidingIssue
{
    internal class BoolToGridRowColumnSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = parameter as string;
            var unitType = GridUnitType.Star;

            if (param != null && string.Compare(param, "Auto", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                unitType = GridUnitType.Auto;
            }

            return ((bool)value == true) ? new GridLength(1, unitType) : new GridLength(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
