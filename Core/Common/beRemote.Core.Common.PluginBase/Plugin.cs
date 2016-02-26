using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.Common.SimpleSettings;

namespace beRemote.Core.Common.PluginBase
{
    public abstract class Plugin
    {
        public abstract string GetPluginIdentifier();
        private PluginMetadataAttribute _metaData;
        public virtual PluginMetadataAttribute MetaData
        {
            get
            {
                if (_metaData == null)
                {
                    foreach (var attribute in this.GetType().GetCustomAttributes(true))
                    {
                        if (attribute.GetType() == typeof (PluginMetadataAttribute))
                        {
                            _metaData = (PluginMetadataAttribute)attribute;
                        }
                        else if (attribute.GetType().BaseType == typeof (PluginMetadataAttribute))
                        {
                            _metaData = (PluginMetadataAttribute)attribute;
                        }
                    }
                }
                return _metaData;
            }
        }


        public DateTime GetLastModificationDate()
        {
            System.Reflection.Assembly assembly = this.GetType().Assembly;
            var fileInfo = new System.IO.FileInfo(assembly.Location);
            DateTime lastModified = fileInfo.LastWriteTime;

            return lastModified;
        }
        private String _baseDirectory;

        protected Common.SimpleSettings.IniFile Config;
        public void InitiatePlugin(String baseDirectory)
        {
            this._baseDirectory = baseDirectory;

            if (File.Exists(baseDirectory + "\\" + MetaData.PluginIniFile))
                this.Config = new Common.SimpleSettings.IniFile(baseDirectory + "\\" + MetaData.PluginIniFile);
            else
                this.Config = new IniFile();
        }

        //public void Remove()
        //{
        //    //var assemblyFile = new FileInfo(this.GetType().Assembly.Location);
        //    //var pluginDir = new DirectoryInfo(_baseDirectory);
        //    //var trashDir = new DirectoryInfo("plugins\\trash");

        //    //if (false == trashDir.Exists)
        //    //    trashDir.Create();

        //    //pluginDir.MoveTo(trashDir.FullName);
        //    //assemblyFile.MoveTo(trashDir.FullName + "\\" + assemblyFile.Name);
        //}
    }
}
