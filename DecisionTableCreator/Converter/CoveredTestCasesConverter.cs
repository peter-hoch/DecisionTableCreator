using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DecisionTableCreator.Converter
{
    public class CoveredTestCasesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long || value is int)
            {
                long lVal = (long)value;
                if (lVal < 0)
                {
                    return "not calculated";
                }
                else
                {
                    return string.Format("{0}", lVal);
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
