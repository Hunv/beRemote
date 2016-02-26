using System;
using System.Security;
using beRemote.Core.Common.Helper;
using beRemote.Core.Common.Helper.CLI;

namespace beRemote.Core.CommandLineInterface
{
    public class CliOptions : AbstractOptions
    {
        #region Credentials
        [CommandLineOption(new string[] { "u", "username", "user" }, true, "Specifies the username")]
        public String Username { get; set; }

        [CommandLineOption(new[] { "p", "password", "pass" }, true, "Specifies the users password, WARNING: This will be present to all plugins")]
        private String _password { get; set; }

        public SecureString Password
        {
            get { return Helper.ConvertToSecureString(_password); }
        } 
        #endregion

        [CommandLineOption(new []{"c", "conn", "cid"}, true, "The connection id to wich beRemote should connect (currently only default protocol is possible)")]
        public String ConnectionSettingId { get; set; }

        [CommandLineOption(new[] { "db-interface" }, true, "The database interface to use for this beRemote instance")]
        public String OverrideDatabaseInterfaceImplFile { get; set; }

        [CommandLineOption(new[] { "db-path" }, true, "The database file to use for this beRemote instance (SQLite only)")]
        public String OverrideDatabaseFile { get; set; }

    }
}
