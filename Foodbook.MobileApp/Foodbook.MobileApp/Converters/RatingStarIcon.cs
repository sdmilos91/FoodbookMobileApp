using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.Converters
{
    public class RatingStarIcon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int rating = (int)value;
            int starNumber = Int32.Parse(parameter.ToString());

            if (rating >= starNumber)
                return "star.png";
            return "starOff.png";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}