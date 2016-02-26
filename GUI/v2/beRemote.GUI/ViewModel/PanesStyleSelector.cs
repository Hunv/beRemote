using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace beRemote.GUI.ViewModel
{
    class PanesStyleSelector : StyleSelector
    {
        public Style GeneralStyle
        {
            get;
            set;
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ViewModelTabBase)
                return GeneralStyle;

            return base.SelectStyle(item, container);
        }
    }
}
