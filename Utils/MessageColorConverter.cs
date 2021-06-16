using DatasheetViewer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DatasheetViewer.Utils
{
   public class MessageColorConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is MessageType msgT)
         {
            if (msgT == MessageType.Error)
            {
               return new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else if (msgT == MessageType.Warning)
            {
               return new SolidColorBrush(Color.FromRgb(255, 190, 0));
            }
            else if (msgT == MessageType.Finished)
            {
               return new SolidColorBrush(Color.FromRgb(48, 255, 64));
            }
            else
            {
               return new SolidColorBrush(Color.FromRgb(50, 50, 50));
            }
         }
         return new SolidColorBrush(Color.FromRgb(50, 50, 50));
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}
