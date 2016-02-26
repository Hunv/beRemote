using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace beRemote.GUI.Controls.FolderView
{
    [ValueConversion(typeof(bool), typeof(ImageSource))]
    public class IsCollapsedToLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool) value)
                return GetIcon("pack://application:,,,/beRemote.GUI.Controls;component/Controls/FolderView/Images/advance.png");

            return GetIcon("pack://application:,,,/beRemote.GUI.Controls;component/Controls/FolderView/Images/collapse.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("No, dude.");
        }


        #region GetIcon
        /// <summary>
        /// Gets the given Icon as a BitmapFrame for an ImageSource
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private ImageSource GetIcon(string url)
        {
            //Load the public-overlay-Icon (small guy in the bottom right corner)
            var iconUri = new Uri(url, UriKind.RelativeOrAbsolute);
            var iconBitmap = BitmapFrame.Create(iconUri);
            iconBitmap.Freeze();
            return(iconBitmap);
        }
        #endregion
    }
}
