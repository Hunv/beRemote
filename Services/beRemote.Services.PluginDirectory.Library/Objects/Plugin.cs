using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Services.PluginDirectory.Library.Objects
{
    public class Plugin : BaseObject
    {
        private Version _pluginVersion;
        private Boolean _isInstalled;
        private Boolean _isProvisioned;
        private Boolean _updateAvailable;
        private Boolean _isObjectVisible;

        public Guid Id { get; set; }
        public String Name { get; set; }
        public String FullQuallifiedAssemblyName { get; set; }
        public Guid TypeId { get; set; }
        public String Description { get; set; }
        public Guid AuthorId { get; set; }
        public Guid VersionId { get; set; }
        public Guid ProvisioningId{ get; set; }
        public Guid RequiredBeRemoteVersionId { get; set; }
        public Boolean IsObjectVisible { get { return _isObjectVisible; } set { SetField(ref _isObjectVisible, value, "IsObjectVisible"); } }

        public Author Author = null;
        public PluginType PluginType  = null;
        public Version PluginVersion { get { return _pluginVersion; } set { SetField(ref _pluginVersion, value, "PluginVersion"); } }
        public Version RequiredBeRemoteVersion = null;
        public Group[] Groups = null;
        public SearchTerm[] SearchTerms = null;
        public Boolean IsInstalled { get { return _isInstalled; } set { SetField(ref _isInstalled, value, "IsInstalled"); } }
        public Boolean IsProvisioned { get { return _isProvisioned; } set { SetField(ref _isProvisioned, value, "IsProvisioned"); } }
        public Boolean UpdateAvailable { get { return _updateAvailable; } set { SetField(ref _updateAvailable, value, "UpdateAvailable"); } }
        public DateTime LastChanged { get; set; }

        public bool IsUpdateable { get; set; }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Name, FullQuallifiedAssemblyName);
        }
    }
}
