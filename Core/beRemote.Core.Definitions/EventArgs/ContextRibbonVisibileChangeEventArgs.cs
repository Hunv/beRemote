using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.Core.Definitions.EventArgs
{
    public class ContextRibbonVisibileChangeEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// A List of Contextribbons, that should be made visible
        /// </summary>
        public List<string> ShowContextRibbon { get; set; }

        /// <summary>
        /// A List of Contextribbons, that should be made invisible
        /// </summary>
        public List<string> HideContextRibbon { get; set; }
    }
}
