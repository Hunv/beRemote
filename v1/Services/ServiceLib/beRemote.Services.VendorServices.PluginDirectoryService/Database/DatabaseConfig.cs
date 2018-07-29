using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Services.VendorServices.PluginDirectoryService.Database
{
    public class DatabaseConfig
    {
        public String SqlServer { get; set; }
        public String SqlDatabase { get; set; }
        public String SqlUsername { get; set; }
        public String SqlPassword { get; set; }

        public static DatabaseConfig Load(FileInfo configFile)
        {

            var des = new System.Yaml.Serialization.YamlSerializer();
            //var result = des.DeserializeFromFile(ymlConfigFile.FullName, typeof(OXProfile));
            DatabaseConfig conf = null;
            using (var stream = new StreamReader(new FileStream(configFile.FullName, FileMode.Open, FileAccess.Read),
                Encoding.GetEncoding("Windows-1252")))
            {

                var result =
                    des.Deserialize(stream, typeof (DatabaseConfig));
                conf = (DatabaseConfig) result[0];
            }

            return conf;
        }
    }
}
