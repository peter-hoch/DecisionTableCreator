using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DecisionTableCreator.Converter
{
    public class CoverageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                double dVal = (double) value;
                if (dVal.Equals(double.NaN) || dVal < 0)
                {
                    return "not calculated";
                }
                else
                {
                    return string.Format("{0:F}%", dVal);
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
