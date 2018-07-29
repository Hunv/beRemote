using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace beRemote.Services.ServiceLib.Classes.ServicePlugin
{
    public abstract class AbstractListenerAction
    {
        public Dictionary<String, ListenerActionParameter> AdditionalParameters = new Dictionary<string, ListenerActionParameter>();
        private ListenerActionAttribute _metaData;
        public ListenerActionAttribute Metadata
        {
            get
            {
                if (_metaData == null)
                {
                    foreach (var attribute in this.GetType().GetCustomAttributes(true))
                    {
                        if (attribute.GetType() == typeof(ListenerActionAttribute))
                        {
                            _metaData = (ListenerActionAttribute)attribute;
                        }
                        else if (attribute.GetType().BaseType == typeof(ListenerActionAttribute))
                        {
                            _metaData = (ListenerActionAttribute)attribute;
                        }
                    }
                }
                return _metaData;
            }
        }

        /// <summary>
        /// The parent object that has registered this action to itself
        /// </summary>
        public AbstractListener Listener { get; private set; }

        public AbstractListenerAction(AbstractListener parentListener)
        {
            Listener = parentListener;
        }

        /// <summary>
        /// Triggers the action that should be executed
        /// </summary>
        /// <param name="context">The HttpContext from wich this action is triggered</param>
        public abstract void ExecuteAction(ExecutionContext context);

        public String Serialize(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialize<T>(String input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }
    }
}
