using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class AddTabEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The tab, that will be presented in the Content-Area
        /// </summary> 
        public Control Tab { get; set; }

        /// <summary>
        /// A defined contentId. Will be set, if no GUID is set
        /// </summary>
        public Guid ContentId { get;set;}
    }
}
