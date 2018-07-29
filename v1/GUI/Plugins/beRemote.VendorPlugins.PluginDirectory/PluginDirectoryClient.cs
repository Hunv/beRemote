using System.IO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.Services;
using beRemote.Services.PluginDirectory.Library.Objects;
using Newtonsoft.Json;

namespace beRemote.VendorPlugins.PluginDirectory
{
    public class PluginDirectoryClient : AbstractBeRemoteServiceClient
    {
        public override string ValidClientHash
        {
            get
            {
                return
                    "f9e42f36afd16120244e12d41ba38a2c38ec5f02e376f0543d665023667bfa9c01fb34de6b90572636ae9f95727c4a7f9cec325d616ce877d811aa2ef6393db0";
            }
        }
       
        public PluginDirectoryClient(string baseUrl) : base(baseUrl)
        {
        }

        public List<beRemote.Services.PluginDirectory.Library.Objects.Plugin> GetAllPluginInformation()
        {
            var paras = new Dictionary<String, String>();
            paras.Add("Listener", "Plugin");
            paras.Add("Action", "GetJoinedList");

            String json = ExecuteJsonRequest(GetUri("", paras));
            return JsonConvert.DeserializeObject<List<Services.PluginDirectory.Library.Objects.Plugin>>(json);
        }

      

        public FileInfo DownloadPlugin(Plugin plugin)
        {
            String bitness = (IntPtr.Size == 8) ? "64" : "32";

            var fileInfo =
                ExecuteDownloadRequest(GetUri(String.Format("?Listener=Plugin&Action=DownloadPlugin&PluginId={0}&PluginBitness={1}", plugin.Id, bitness)), plugin.Name + ".bpl");

            return fileInfo;
        }
        
    }
}
