using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace beRemote.GUI.ViewModel
{
    public class PanesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ViewModelGeneralTemplate
        {
            get;
            set;
        }
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var itemAsLayoutContent = item as LayoutContent;

            if (item is ViewModelTabBase)
                return ViewModelGeneralTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
