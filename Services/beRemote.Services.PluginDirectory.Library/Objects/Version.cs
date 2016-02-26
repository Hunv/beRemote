using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Services.PluginDirectory.Library.Objects
{
    public class Version : BaseObject
    {
        private Guid _id;
        private String _versionCode;
        private Boolean _isCurrentVersion;
        private String _description;
        private int _major;
        private int _minor;
        private int _build;
        private int _revision;

        public Guid Id { get { return _id; } set { SetField(ref _id, value, "Id"); } }
        public String VersionCode { get { return _versionCode; } set { SetField(ref _versionCode, value, "VersionCode"); } }
        public Boolean IsCurrentVersion { get { return _isCurrentVersion; } set { SetField(ref _isCurrentVersion, value, "IsCurrentVersion"); } }
        public String Description { get { return _description; } set { SetField(ref _description, value, "Description"); } }

        public int Major {get { return _major; } set { SetField(ref _major, value, "Major"); }}
        public int Minor { get { return _minor; } set { SetField(ref _major, value, "Minor"); } }
        public int Build { get { return _build; } set { SetField(ref _build, value, "Build"); } }
        public int Revision { get { return _revision; } set { SetField(ref _revision, value, "Revision"); } }
    }
}
