using System.IO.Compression;
using System.Reflection;
using System.Threading;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Common.PluginBase;
using beRemote.Core.Common.SimpleSettings;
using beRemote.Core.Exceptions;
using beRemote.Core.Exceptions.Plugin;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.Core.ProtocolSystem.ProtocolBase.Interfaces;
using beRemote.GUI.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;

namespace beRemote.MEF
{
    public class MEFLoader
    {
        public enum MEFEventId
        {
            Default = 23000,
            VerboseLogging = 23001
        }

        public MEFLoader()
        {

            RemoveTrash();

            #region Update process
            var baseUpdateDir = new DirectoryInfo("plugins\\updates");

            if (baseUpdateDir.Exists)
            {
                foreach (var pluginContainer in baseUpdateDir.GetFiles("*.bpl"))
                {
                    try
                    {
                        ValidatePluginContainer(pluginContainer);
                                                                                                                                  
                        using (var zip = new ZipArchive(new FileStream(pluginContainer.FullName, FileMode.Open)))
                        {

                            var entry = zip.GetEntry("plugin.definition");

                            if (null != entry)
                            {
                                using (var tr = new System.IO.StreamReader(entry.Open()))
                                {
                                    IniFile definition = new IniFile(tr);

                                    var name = definition.GetValue("plugin", "name");
                                    var version = definition.GetValue("plugin", "version");
                                    var bitness = definition.GetValue("plugin", "bitness");
                                    var installpath = definition.GetValue("plugin", "installpath");
                                    var contentfolder = definition.GetValue("plugin", "contentfolder");

                                    Logger.Info(String.Format("Installing or updating plugin by plugincontainer."));
                                    Logger.Info(String.Format("   Plugin container file: {0}", pluginContainer.FullName));
                                    Logger.Info(String.Format("   Plugin bitness: {0}", bitness));
                                    Logger.Info(String.Format("   Plugin installpath: {0}", installpath));
                                    Logger.Info(String.Format("   Plugin version: {0}", version));
                                    String target = Path.Combine(Environment.GetEnvironmentVariable("appdata"),
                                        "beRemote", "tmp", contentfolder);
                                    ZipFileExtensions.ExtractToDirectory(zip, target);

                                    var contentDir = new DirectoryInfo(Path.Combine(target, contentfolder));
                                    var targetDir = new DirectoryInfo((installpath.StartsWith("\\")) ? installpath.Remove(0, 1) : installpath);

                                    foreach (var file in contentDir.GetFiles())
                                        file.MoveTo(Path.Combine(targetDir.FullName, file.Name));

                                    foreach (var dir in contentDir.GetDirectories())
                                        dir.MoveTo(Path.Combine(targetDir.FullName, dir.Name));

                                    Logger.Info(String.Format("Plugin installed, deleting container"));

                                    Directory.Delete(target, true);
                                }
                            }
                        }

                        pluginContainer.Delete();
                    }
                    catch (Exception ex)
                    {
                        new beRemoteException(beRemoteExInfoPackage.SignificantInformationPackage,
                            "Plugin container is invalid. Removing it from updater.", ex);

                        pluginContainer.Delete();
                    }

                }
            } 
            #endregion
        }

        private void RemoveTrash()
        {
            try
            {
                #region Delete process
                var baseTrashDir = new DirectoryInfo("plugins\\trash");

                if (baseTrashDir.Exists)
                {
                    foreach (var file in baseTrashDir.GetFiles("*.dll"))
                    {
                        String name = file.Name.Replace(file.Extension, "");
                        file.Delete();
                        // get associated plugin folder
                        foreach (var folder in new DirectoryInfo("plugins").GetDirectories())
                        {
                            var pluginDir = new DirectoryInfo(folder.FullName + "\\" + name);
                            if (pluginDir.Exists)
                                pluginDir.Delete(true);


                            foreach (var file2 in folder.GetFiles("*" + name + "*"))
                            {
                                file2.Delete();
                            }
                        }

                        //var protocolDir = new DirectoryInfo("plugins\\protocols\\" + name);
                        //var uiDir = new DirectoryInfo("plugins\\ui\\" + name);

                        //if (pluginDir.Exists)
                        //    pluginDir.Delete(true);

                       
                    }
                    baseTrashDir.Delete(true);
                }

                

                #endregion
            }
            catch (Exception ex)
            {

                new beRemoteException(beRemoteExInfoPackage.SignificantInformationPackage,
                    "You do not have permission to modify the beRemote directory.", ex);
            }
       
        }

        #region Imports

        //[ImportMany("beRemote.Core.ProtocolSystem.ProtocolBase.Protocol", typeof(Protocol))]
        //private IEnumerable<Lazy<Protocol, IProtocolMetadata>> protocols { get; set; }
        [ImportMany(typeof(Protocol))]
        private IEnumerable<Protocol> protocols { get; set; }

        [ImportMany(typeof(UiPlugin))]
        private IEnumerable<UiPlugin> uiplugins { get; set; }

        #endregion
        #region available plugins

        public SortedList<String, Protocol> AvailableProtocols { get; private set; }

        public SortedList<String, UiPlugin> AvailableUiPlugins { get; private set; }

        #endregion
        #region Metadata

        private SortedList<String, ProtocolMetadataAttribute> protocolMetadata { get; set; }
        private SortedList<String, PluginMetadataAttribute> uipluginMetadata { get; set; }

        #endregion
        #region Public Metadata access

        public ProtocolMetadataAttribute GetProtocolMetadata(String fullQualifiedName)
        {
            return protocolMetadata[fullQualifiedName];
        }
        public PluginMetadataAttribute GetUiPluginMetadata(String fullQualifiedName)
        {
            return uipluginMetadata[fullQualifiedName];
        }

        #endregion

        public void LoadProtocols()
        {
            var baseDirectory = new DirectoryInfo("plugins\\protocols");
            AvailableProtocols = new SortedList<string, Protocol>();
            protocolMetadata = new SortedList<string, ProtocolMetadataAttribute>();

            Logger.Info("[MEF] Loading Protocols...");
            if (false == baseDirectory.Exists)
            {
                new beRemoteException(beRemoteExInfoPackage.SignificantInformationPackage,
                    "Protocol base directory does not exist.");

                return;
            }

            var aggregateCatalog = new AggregateCatalog();

            foreach (var file in baseDirectory.GetFiles("*.dll"))
            {
                var assemblyCatalog = new AssemblyCatalog(Assembly.LoadFile(file.FullName));

                try
                {
                    assemblyCatalog.Parts.ToArray();
                    aggregateCatalog.Catalogs.Add(assemblyCatalog);
                }
                catch (Exception ex) 
                {
                    Logger.Error(String.Format("[MEF] Error while loading assembly '{0}'", file.FullName), (int)MEFEventId.Default);
                    Logger.Error(ex.ToString(), (int)MEFEventId.Default);
                }
            }

            //var directoryCatalog = new DirectoryCatalog(baseDirectory.FullName);



            var container = new CompositionContainer(aggregateCatalog);
            
            try
            {
                Logger.Verbose("[MEF] Adding assembly search paths", (int)MEFEventId.VerboseLogging);
                AppDomain.CurrentDomain.AppendPrivatePath(baseDirectory.FullName);
                foreach (var dll_file in baseDirectory.GetFiles("*.dll"))
                {
                    String file_name = dll_file.Name.Replace(".dll", "");

                    if (baseDirectory.GetDirectories(file_name).Count() > 0)
                    {
                        DirectoryInfo dir = baseDirectory.GetDirectories(file_name)[0];

                        AppDomain.CurrentDomain.AppendPrivatePath(dir.FullName + "\\libs");
                    }
                    else
                    {

                    }


                }

                

                Logger.Verbose("[MEF] Composing parts...", (int)MEFEventId.VerboseLogging);
                container.ComposeParts(this);
                Logger.Verbose("[MEF] ... composing done!", (int)MEFEventId.VerboseLogging);
                Logger.Verbose("[MEF] Found " + protocols.Count().ToString() + " protocol(s)", (int)MEFEventId.VerboseLogging);
                foreach (Protocol protocol in protocols)
                {
                    Logger.Verbose("[MEF] ProtocolMetadata:", (int)MEFEventId.VerboseLogging);
                    Logger.Verbose(String.Format("[MEF] Full qualified name: {0}", protocol.MetaData.PluginFullQualifiedName), (int)MEFEventId.VerboseLogging);
                    Logger.Verbose(String.Format("[MEF] Name: {0}", protocol.MetaData.PluginName), (int)MEFEventId.VerboseLogging);
                    Logger.Verbose(String.Format("[MEF] Version: {0}", protocol.MetaData.PluginVersionCode), (int)MEFEventId.VerboseLogging);
                    Logger.Verbose(String.Format("[MEF] IniFile: {0}", protocol.MetaData.PluginIniFile), (int)MEFEventId.VerboseLogging);

                    try
                    {
                        protocol.InitiatePlugin(Path.Combine(baseDirectory.FullName, protocol.MetaData.PluginConfigFolder));
                        protocol.InitiateProtocol(Path.Combine(baseDirectory.FullName, protocol.MetaData.PluginConfigFolder));
                        protocol.TestProtocol();
                        protocol.SetProtocolIdentifier(protocol.MetaData.PluginFullQualifiedName);

                        protocolMetadata.Add(protocol.MetaData.PluginFullQualifiedName, (ProtocolMetadataAttribute)protocol.MetaData);

                        AvailableProtocols.Add(protocol.MetaData.PluginFullQualifiedName, protocol);
                    }
                    catch (Exception ex)
                    {
                        new PluginException(new beRemoteExInfoPackage(new System.Diagnostics.StackTrace(), Core.Exceptions.Definitions.ExceptionUrgency.SIGNIFICANT), "[MEF] Error loading protocol in MEFLoader", ex);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Warning(String.Format("{0}", ex));
            }
            finally
            {
                Logger.Info(String.Format("[MEF] Successfully loaded {0} plugin(s)", AvailableProtocols.Count));
            }
        }

        public void LoadUIPlugins()
        {

            try
            {

                Logger.Info("[MEF] Loading UI Plugins...");
                var baseDirectory = new DirectoryInfo("plugins\\ui");
                AvailableUiPlugins = new SortedList<string, UiPlugin>();
                uipluginMetadata = new SortedList<string, PluginMetadataAttribute>();
                if (false == baseDirectory.Exists)
                {
                    new beRemoteException(beRemoteExInfoPackage.SignificantInformationPackage,
                        "Plugin base directory does not exist.");

                    return;
                }
                var container = new CompositionContainer(new DirectoryCatalog(baseDirectory.FullName));

                Logger.Verbose("[MEF] Adding assembly search paths", (int)MEFEventId.VerboseLogging);
                foreach (var dll_file in baseDirectory.GetFiles("*.dll"))
                {
                    String file_name = dll_file.Name.Replace(".dll", "");

                    if (baseDirectory.GetDirectories(file_name).Count() > 0)
                    {
                        DirectoryInfo dir = baseDirectory.GetDirectories(file_name)[0];

                        AppDomain.CurrentDomain.AppendPrivatePath(dir.FullName + "\\libs");
                    }
                    else
                    {

                    }


                }

                Logger.Verbose("[MEF] Composing parts...", (int)MEFEventId.VerboseLogging);
                container.ComposeParts(this);
                Logger.Verbose("[MEF] ... composing done!", (int)MEFEventId.VerboseLogging);
                Logger.Verbose("[MEF] Found " + uiplugins.Count().ToString() + " UI plugins(s)", (int)MEFEventId.VerboseLogging);
                foreach (var uiplugin in uiplugins)
                {
                    Logger.Verbose("[MEF] UIPluginMetadata:", (int)MEFEventId.VerboseLogging);
                    Logger.Verbose(String.Format("[MEF] Full qualified name: {0}", uiplugin.MetaData.PluginFullQualifiedName), (int)MEFEventId.VerboseLogging);
                    Logger.Verbose(String.Format("[MEF] Name: {0}", uiplugin.MetaData.PluginName), (int)MEFEventId.VerboseLogging);
                    Logger.Verbose(String.Format("[MEF] Version: {0}", uiplugin.MetaData.PluginVersionCode), (int)MEFEventId.VerboseLogging);

                    try
                    {
                        uiplugin.InitiatePlugin(Path.Combine(baseDirectory.FullName, uiplugin.MetaData.PluginConfigFolder));
                        uiplugin.InitiateUiPlugin(Path.Combine(baseDirectory.FullName, uiplugin.MetaData.PluginConfigFolder));
                        //uiplugin.Value.TestProtocol();
                        //uiplugin.Value.SetProtocolIdentifier(protocol.Metadata.ProtocolFullQualifiedName);

                        uipluginMetadata.Add(uiplugin.MetaData.PluginFullQualifiedName, uiplugin.MetaData);

                        AvailableUiPlugins.Add(uiplugin.MetaData.PluginFullQualifiedName, uiplugin);
                    }
                    catch (Exception ex)
                    {
                        new PluginException(beRemoteExInfoPackage.SignificantInformationPackage, "[MEF] Error loading UI plugin in MEFLoader", ex);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Warning(String.Format("{0}", ex));
            }
            finally
            {
                Logger.Info(String.Format("[MEF] Successfully loaded {0} ui plugin(s)", AvailableProtocols.Count));
            }
        }

        private String GetPluginLibsFolder(String fullQualifiedName)
        {
            return fullQualifiedName + "\\libs";
        }

        public void ValidatePluginContainer(FileInfo pluginContainer)
        {
            if (false == pluginContainer.Exists)
                new beRemoteException(beRemoteExInfoPackage.SignificantInformationPackage,
                    "Plugin container file not found",
                    new FileNotFoundException("Plugin container file not found", pluginContainer.FullName));


            using (var zip = new ZipArchive(new FileStream(pluginContainer.FullName, FileMode.Open)))
            {

                var entry = zip.GetEntry("plugin.definition");

                if (null != entry)
                {
                    using (var tr = new System.IO.StreamReader(entry.Open()))
                    {
                        IniFile definition = new IniFile(tr);

                        try
                        {
                            var vals = new List<String>();

                            vals.Add(definition.GetValue("plugin", "name"));
                            vals.Add(definition.GetValue("plugin", "version"));
                            vals.Add(definition.GetValue("plugin", "bitness"));
                            vals.Add(definition.GetValue("plugin", "installpath"));
                            vals.Add(definition.GetValue("plugin", "contentfolder"));

                            foreach (var val in vals)
                                if (String.Empty.Equals(val))
                                    throw new beRemoteException(beRemoteExInfoPackage.SignificantInformationPackage,
                                        "Definition file not valid. Empty values present");
                        }
                        catch (Exception ex)
                        {
                            throw new beRemoteException(beRemoteExInfoPackage.SignificantInformationPackage,
                                "Plugin container is not valid. Definition file not valid", ex);

                        }
                    }
                }
                else
                {
                    // fail
                    throw new beRemoteException(beRemoteExInfoPackage.SignificantInformationPackage,
                        "Plugin container is not valid. Definition file not found");
                }
            }
        }
    }
}
