using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Services.ServiceLib.Classes.ServicePlugin
{
    public class ListenerActionParameter
    {
        public String Key { get; set; }
        public Object Value { get; set; }

        public T GetValue<T>()
        {
            return (T) Value;
        }
    }
}
