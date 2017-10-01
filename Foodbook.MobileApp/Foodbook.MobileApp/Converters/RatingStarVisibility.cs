using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.Converters
{
    public class RatingStarVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;


            int rating = 0;
            int starNumber = Int32.Parse(parameter.ToString());

            if (value.GetType() == typeof(double))
            {
                double doubleVal = (double)value;
                rating = (int)doubleVal;
            }
            else if (value.GetType() == typeof(int))
            {
                rating = (int)value;
            }
           

            if (rating >= starNumber)
                return true;
            return false;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}