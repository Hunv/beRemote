using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.SessionState;

namespace beRemote.Services.ServiceLib.Classes.ServicePlugin
{
    public abstract class AbstractListener
    {

        public List<AbstractListenerAction> ListenerActions = new List<AbstractListenerAction>();

        public HttpSessionState Session { get; private set; }

        public AbstractServicePlugin ServicePlugin { get; private set; }

        public AbstractListener(AbstractServicePlugin parentPlugin)
        {
            ServicePlugin = parentPlugin;
        }

        public void InitializeServiceForSession(HttpSessionState session)
        {
            Session = session;
        }

        private ListenerMetadataAttribute _metaData;
        public ListenerMetadataAttribute Metadata
        {
            get
            {
                if (_metaData == null)
                {
                    foreach (var attribute in this.GetType().GetCustomAttributes(true))
                    {
                        if (attribute.GetType() == typeof(ListenerMetadataAttribute))
                        {
                            _metaData = (ListenerMetadataAttribute)attribute;
                        }
                        else if (attribute.GetType().BaseType == typeof(ListenerMetadataAttribute))
                        {
                            _metaData = (ListenerMetadataAttribute)attribute;
                        }
                    }
                }
                return _metaData;
            }
        }

        public bool HandlesAction(String actionName)
        {
            return GetAction(actionName) != null;
        }

        public AbstractListenerAction GetAction(String actionName)
        {
            return ListenerActions.FirstOrDefault(action => action.Metadata.ActionName.Equals(actionName));
        }

        public void Execute(string actionName, ExecutionContext context)
        {
            GetAction(actionName).ExecuteAction(context);
        }

        public DirectoryInfo BaseDirectory
        {
            get { return this.ServicePlugin.BaseDirectory; }
        }
    }
}
