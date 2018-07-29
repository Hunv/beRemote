using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Web;
using beRemote.Services.ServiceLib.Classes.ServicePlugin;

namespace beRemote.Services.WebService
{
    public class MefLoader
    {
        [ImportMany(typeof(AbstractServicePlugin))]
        private IEnumerable<AbstractServicePlugin>  ServicePlugins { get; set; }

        public Dictionary<String, AbstractServicePlugin> ListenerPluginAssociation = new Dictionary<string, AbstractServicePlugin>();

        public void LoadServicePlugins(DirectoryInfo direcotry, HttpContext context)
        {
            if(null == direcotry)
                throw new ArgumentNullException();
            if(false == direcotry.Exists)
                throw new ArgumentException("Target directory does not exist");
            
            #region AppDomain appender

            AppDomain.CurrentDomain.AppendPrivatePath(direcotry.FullName);
            foreach (var dll_file in direcotry.GetFiles("*.dll"))
            {
                String file_name = dll_file.Name.Replace(".dll", "");

                if (direcotry.GetDirectories(file_name).Count() > 0)
                {
                    DirectoryInfo dir = direcotry.GetDirectories(file_name)[0];

                    AppDomain.CurrentDomain.AppendPrivatePath(dir.FullName + "\\libs");
                }
                else
                {

                }


            } 
            #endregion

            var dirCatalog = new DirectoryCatalog(direcotry.FullName);
            var assemblyCataLog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var aggregateCatalog = new AggregateCatalog();

            aggregateCatalog.Catalogs.Add(dirCatalog);
            aggregateCatalog.Catalogs.Add(assemblyCataLog);

            var container = new CompositionContainer(aggregateCatalog);
            
            try
            {
                container.ComposeParts(this);

                foreach (var servicePlugin in ServicePlugins)
                {
                    servicePlugin.InitializeServicePlugin(context.Session);

                    //foreach(var a in servicePlugin.RegisteredListeners)

                    context.Response.Headers.Add(servicePlugin.MetaData.PluginFullQualifiedName, servicePlugin.MetaData.PluginName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if one of the registered plugins handles the given listener/service
        /// </summary>
        /// <param name="listenerName"></param>
        /// <returns></returns>
        internal bool HandlesListener(string listenerName)
        {
            foreach (var service in ServicePlugins)
            {
                if (service.HandlesListener(listenerName))
                {
                    return true;
                }
            }
            return false;
        }

        internal AbstractServicePlugin GetPluginThatHandlesListener(string serviceName)
        {
            foreach (var service in ServicePlugins)
            {
                if (service.HandlesListener(serviceName))
                {
                    return service;
                }
            }
            return null;
        }
    }
}