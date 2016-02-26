using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.SessionState;
using beRemote.Core.Common.PluginBase;

namespace beRemote.Services.ServiceLib.Classes.ServicePlugin
{
    public abstract class AbstractServicePlugin : Plugin
    {
        public readonly Dictionary<String, AbstractListener> RegisteredListeners = new Dictionary<String, AbstractListener>();
        public HttpSessionState Session { get; private set; }

        public void InitializeServicePlugin(HttpSessionState session)
        {
            Session = session;

            foreach (var listener in RegisteredListeners)
            {
                listener.Value.InitializeServiceForSession(Session);
            }
        }

        public void RegisterListener(AbstractListener listener)
        {
            RegisteredListeners.Add(listener.Metadata.Id, listener);
        }

        public override string GetPluginIdentifier()
        {
            return MetaData.PluginFullQualifiedName;
        }

        

        /// <summary>
        /// Checks if the the current implementation contains the specified virtual service
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public bool HandlesListener(string listenerName)
        {
            foreach (var listeners in RegisteredListeners)
            {
                if (listeners.Value.Metadata.Listener.Equals(listenerName))
                    return true;
            }
            return false;
        }


        public AbstractListener GetListener(string listenerName)
        {
            foreach (var listener in RegisteredListeners)
            {
                if (listener.Value.Metadata.Listener.Equals(listenerName))
                    return listener.Value;
            }
            return null;
        }

        public DirectoryInfo BaseDirectory
        {
            get
            {
                return new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServicePlugins", this.MetaData.PluginConfigFolder));
            }
        }
    }
}
