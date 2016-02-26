using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.Common.LogSystem;

namespace beRemote.Core.StorageSystem.StorageBase
{
    class DatabaseDefinition
    {
        private static readonly List<DatabaseTableDefinition> MaskTables = new List<DatabaseTableDefinition>();
        private static readonly List<DatabaseContentDefinition> DefaultContent = new List<DatabaseContentDefinition>();

        #region Fill Definitions

        private static void FillDefinitions()
        {
            FillTableDefintions();
            FillContent();
        }

        private static void FillTableDefintions()
        {
            //Connectionoptions
            var dtdConnectionoptions = new DatabaseTableDefinition("connectionoptions");
            dtdConnectionoptions.Columns.Add(new DatabaseColumnDefinition("id", typeof(Int64), 0, true, null, true, true, true));
            dtdConnectionoptions.Columns.Add(new DatabaseColumnDefinition("connectionsettingid", typeof(Int64), 0, true, null, false, false, false));
            dtdConnectionoptions.Columns.Add(new DatabaseColumnDefinition("optionname", typeof(String), 100, true, null, false, false, false));
            dtdConnectionoptions.Columns.Add(new DatabaseColumnDefinition("value", typeof(String), 100, true, null, false, false, false));
            MaskTables.Add(dtdConnectionoptions);

            //Connections
            var dtdConnections = new DatabaseTableDefinition("connections");
            dtdConnections.Columns.Add(new DatabaseColumnDefinition("id", typeof(Int64), 0, true, null, true, true, true));
            dtdConnections.Columns.Add(new DatabaseColumnDefinition("host", typeof(String), 256, true, null, false, false, false));
            dtdConnections.Columns.Add(new DatabaseColumnDefinition("name", typeof(String), 100, true, null, false, false, false));
            dtdConnections.Columns.Add(new DatabaseColumnDefinition("description", typeof(String), 2000, true, null, false, false, false));
            dtdConnections.Columns.Add(new DatabaseColumnDefinition("folder", typeof(Int64), 0, true, null, false, false, false));
            dtdConnections.Columns.Add(new DatabaseColumnDefinition("sortorder", typeof(Int16), 0, true, null, false, false, false));
            dtdConnections.Columns.Add(new DatabaseColumnDefinition("os", typeof(Int16), 0, true, null, false, false, false));
            dtdConnections.Columns.Add(new DatabaseColumnDefinition("owner", typeof(Int64), 0, true, null, false, false, false));
            dtdConnections.Columns.Add(new DatabaseColumnDefinition("ispublic", typeof(Boolean), 0, true, null, false, false, false));
            dtdConnections.Columns.Add(new DatabaseColumnDefinition("vpn", typeof(Int16), 0, true, 0, false, false, false));
            MaskTables.Add(dtdConnections);

            //Connectionsetting
            var dtdConnectionsetting = new DatabaseTableDefinition("connectionsetting");
            dtdConnectionsetting.Columns.Add(new DatabaseColumnDefinition("id", typeof(Int64), 0, true, null, true, true, true));
            dtdConnectionsetting.Columns.Add(new DatabaseColumnDefinition("connectionid", typeof(Int64), 0, true, null, false, false, false));
            dtdConnectionsetting.Columns.Add(new DatabaseColumnDefinition("protocol", typeof(String), 100, true, null, false, false, false));
            dtdConnectionsetting.Columns.Add(new DatabaseColumnDefinition("port", typeof(Int32), 0, true, null, false, false, false));
            dtdConnectionsetting.Columns.Add(new DatabaseColumnDefinition("credentialid", typeof(Int64), 0, true, null, false, false, false));
            MaskTables.Add(dtdConnectionsetting);

            //credentials
            var dtdCredentials = new DatabaseTableDefinition("credentials");
            dtdCredentials.Columns.Add(new DatabaseColumnDefinition("id", typeof(Int64), 0, true, null, true, true, true));
            dtdCredentials.Columns.Add(new DatabaseColumnDefinition("username", typeof(String), 100, true, null, false, false, false));
            dtdCredentials.Columns.Add(new DatabaseColumnDefinition("password", typeof(Byte[]), 512, true, null, false, false, false));
            dtdCredentials.Columns.Add(new DatabaseColumnDefinition("domain", typeof(String), 100, true, null, false, false, false));
            dtdCredentials.Columns.Add(new DatabaseColumnDefinition("owner", typeof(Int64), 0, true, null, false, false, false));
            dtdCredentials.Columns.Add(new DatabaseColumnDefinition("description", typeof(String), 100, true, null, false, false, false));
            MaskTables.Add(dtdCredentials);

            //data
            var dtdData = new DatabaseTableDefinition("data");
            dtdData.Columns.Add(new DatabaseColumnDefinition("id", typeof(Int64), 0, true, null, true, true, true));
            dtdData.Columns.Add(new DatabaseColumnDefinition("data", typeof(Byte[]), 0, true, null, false, false, false));
            MaskTables.Add(dtdData);

            //filter
            var dtdFilter = new DatabaseTableDefinition("filter");
            dtdFilter.Columns.Add(new DatabaseColumnDefinition("id", typeof(Int64), 0, true, null, true, true, true));
            dtdFilter.Columns.Add(new DatabaseColumnDefinition("condition", typeof(String), 0, true, null, false, false, false));
            dtdFilter.Columns.Add(new DatabaseColumnDefinition("isnegative", typeof(Boolean), 0, true, 0, false, false, false));
            dtdFilter.Columns.Add(new DatabaseColumnDefinition("isor", typeof(Boolean), 0, true, 0, false, false, false));
            dtdFilter.Columns.Add(new DatabaseColumnDefinition("islike", typeof(Boolean), 0, true, 0, false, false, false));
            dtdFilter.Columns.Add(new DatabaseColumnDefinition("phrase", typeof(Byte[]), 0, true, null, false, false, false));
            dtdFilter.Columns.Add(new DatabaseColumnDefinition("filtersetid", typeof(Int64), 0, true, null, false, false, false));
            dtdFilter.Columns.Add(new DatabaseColumnDefinition("sortorder", typeof(Int16), 0, true, null, false, false, false));
            dtdFilter.Columns.Add(new DatabaseColumnDefinition("description", typeof(String), 200, true, string.Empty, false, false, false));
            MaskTables.Add(dtdFilter);

            //filterset
            var dtdFilterset = new DatabaseTableDefinition("filterset");
            dtdFilterset.Columns.Add(new DatabaseColumnDefinition("id", typeof(Int64), 0, true, null, true, true, true));
            dtdFilterset.Columns.Add(new DatabaseColumnDefinition("title", typeof(String), 0, true, null, false, false, false));
            dtdFilterset.Columns.Add(new DatabaseColumnDefinition("owner", typeof(Int64), 0, true, null, false, false, false));
            dtdFilterset.Columns.Add(new DatabaseColumnDefinition("public", typeof(Boolean), 0, true, null, false, false, false));
            dtdFilterset.Columns.Add(new DatabaseColumnDefinition("parent", typeof(Int64), 0, true, 0, false, false, false));
            dtdFilterset.Columns.Add(new DatabaseColumnDefinition("hide", typeof(Boolean), 0, true, false, false, false, false));
            MaskTables.Add(dtdFilterset);

            //folder
            var dtdFolder = new DatabaseTableDefinition("folder");
            dtdFolder.Columns.Add(new DatabaseColumnDefinition("id", typeof(Int64), 0, true, null, true, true, true));
            dtdFolder.Columns.Add(new DatabaseColumnDefinition("foldername", typeof(String), 100, true, null, false, false, false));
            dtdFolder.Columns.Add(new DatabaseColumnDefinition("parent", typeof(Int64), 0, true, null, false, false, false));
            dtdFolder.Columns.Add(new DatabaseColumnDefinition("sortorder", typeof(Int32), 0, true, null, false, false, false));
            dtdFolder.Columns.Add(new DatabaseColumnDefinition("owner", typeof(Int64), 0, true, null, false, false, false));
            dtdFolder.Columns.Add(new DatabaseColumnDefinition("ispublic", typeof(Boolean), 0, true, null, false, false, false));
            MaskTables.Add(dtdFolder);

            //licenses
            var dtdLicenses = new DatabaseTableDefinition("licenses");
            dtdLicenses.Columns.Add(new DatabaseColumnDefinition("firstname", typeof(String), 100, true, null, false, false, false));
            dtdLicenses.Columns.Add(new DatabaseColumnDefinition("lastname", typeof(String), 100, true, null, false, false, false));
            dtdLicenses.Columns.Add(new DatabaseColumnDefinition("email", typeof(String), 100, true, null, false, false, false));
            dtdLicenses.Columns.Add(new DatabaseColumnDefinition("secret", typeof(String), 100, true, null, false, false, false));
            dtdLicenses.Columns.Add(new DatabaseColumnDefinition("userid", typeof(Int64), 0, true, null, false, false, false));
            MaskTables.Add(dtdLicenses);

            //operatingsystems
            var dtdOperatingsystems = new DatabaseTableDefinition("operatingsystems");
            dtdOperatingsystems.Columns.Add(new DatabaseColumnDefinition("id", typeof(Int16), 0, true, null, true, true, true));
            dtdOperatingsystems.Columns.Add(new DatabaseColumnDefinition("family", typeof(String), 100, true, null, false, false, false));
            dtdOperatingsystems.Columns.Add(new DatabaseColumnDefinition("distribution", typeof(String), 100, true, null, false, false, false));
            dtdOperatingsystems.Columns.Add(new DatabaseColumnDefinition("version", typeof(String), 100, true, null, false, false, false));
            MaskTables.Add(dtdOperatingsystems);

            //settings
            var dtdSettings = new DatabaseTableDefinition("settings");
            dtdSettings.Columns.Add(new DatabaseColumnDefinition("setting", typeof(String), 100, false, null, false, true, false));
            dtdSettings.Columns.Add(new DatabaseColumnDefinition("value", typeof(String), 100, false, null, false, false, false));
            MaskTables.Add(dtdSettings);

            //user
            var dtdUser = new DatabaseTableDefinition("user");
            dtdUser.Columns.Add(new DatabaseColumnDefinition("id", typeof(Int64), 0, true, null, true, true, true));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("name", typeof(String), 256, true, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("password", typeof(Byte[]), 512, true, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("winname", typeof(String), 256, true, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("lastmachine", typeof(String), 256, true, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("superadmin", typeof(Boolean), 0, true, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("lastlogin", typeof(DateTime), 0, true, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("logincount", typeof(Int64), 0, true, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("lastlogout", typeof(DateTime), 0, true, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("defaultfolder", typeof(Int64), 0, true, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("defaultprotocol", typeof(String), 100, true, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("heartbeat", typeof(DateTime), 0, true, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("updatemode", typeof(Int64), 0, true, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("proxyenabled", typeof(Boolean), 0, true, false, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("proxyhost", typeof(String), 256, true, "", false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("proxycredentials", typeof(Int64), 0, true, 0, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("proxyport", typeof(Int64), 0, true, 8080, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("proxyflags", typeof(Int64), 0, true, 0, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("salt1", typeof(Byte[]), 512, false, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("salt2", typeof(Byte[]), 512, false, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("salt3", typeof(Byte[]), 512, false, null, false, false, false));
            dtdUser.Columns.Add(new DatabaseColumnDefinition("deletequickconnect", typeof(Boolean), 0, true, false, false, false, false));
            MaskTables.Add(dtdUser);

            //userhistory
            var dtdUserhistory = new DatabaseTableDefinition("userhistory");
            dtdUserhistory.Columns.Add(new DatabaseColumnDefinition("id", typeof(Int64), 0, true, null, true, true, true));
            dtdUserhistory.Columns.Add(new DatabaseColumnDefinition("userid", typeof(Int64), 0, true, null, false, false, false));
            dtdUserhistory.Columns.Add(new DatabaseColumnDefinition("connectionsettingid", typeof(Int64), 0, true, null, false, false, false));
            dtdUserhistory.Columns.Add(new DatabaseColumnDefinition("pointoftime", typeof(DateTime), 0, true, null, false, false, false));
            MaskTables.Add(dtdUserhistory);

            //uservisuals
            var dtdUservisuals = new DatabaseTableDefinition("uservisuals");
            dtdUservisuals.Columns.Add(new DatabaseColumnDefinition("userid", typeof(Int64), 0, true, null, true, true, false));
            dtdUservisuals.Columns.Add(new DatabaseColumnDefinition("mainwindowx", typeof(Int16), 0, true, null, false, false, false));
            dtdUservisuals.Columns.Add(new DatabaseColumnDefinition("mainwindowy", typeof(Int16), 0, true, null, false, false, false));
            dtdUservisuals.Columns.Add(new DatabaseColumnDefinition("mainwindowmax", typeof(Boolean), 0, true, null, false, false, false));
            dtdUservisuals.Columns.Add(new DatabaseColumnDefinition("mainwindowwidth", typeof(Int16), 0, true, null, false, false, false));
            dtdUservisuals.Columns.Add(new DatabaseColumnDefinition("mainwindowheight", typeof(Int16), 0, true, null, false, false, false));
            dtdUservisuals.Columns.Add(new DatabaseColumnDefinition("ribbonexpanded", typeof(Boolean), 0, true, false, false, false, false));
            dtdUservisuals.Columns.Add(new DatabaseColumnDefinition("expandednodes", typeof(String), 0, true, null, false, false, false));
            dtdUservisuals.Columns.Add(new DatabaseColumnDefinition("statusbarsetting", typeof(Int64), 0, true, null, false, false, false));
            dtdUservisuals.Columns.Add(new DatabaseColumnDefinition("favorites", typeof(String), 0, true, "", false, false, false));
            dtdUservisuals.Columns.Add(new DatabaseColumnDefinition("gridlayout", typeof(String), 0, true, null, false, false, false));
            dtdUservisuals.Columns.Add(new DatabaseColumnDefinition("ribbonqat", typeof(String), 0, true, "", false, false, false));
            MaskTables.Add(dtdUservisuals);

            //vpn
            var dtdVpn = new DatabaseTableDefinition("vpn");
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("id", typeof(Int64), 0, true, null, true, true, true));
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("type", typeof(Int16), 0, true, null, false, false, false));
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("parameter1", typeof(String), 100, false, null, false, false, false));
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("parameter2", typeof(String), 100, false, null, false, false, false));
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("parameter3", typeof(String), 100, false, null, false, false, false));
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("parameter4", typeof(String), 100, false, null, false, false, false));
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("parameter5", typeof(String), 100, false, null, false, false, false));
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("parameter6", typeof(String), 100, false, null, false, false, false));
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("parameter7", typeof(String), 100, false, null, false, false, false));
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("parameter8", typeof(String), 100, false, null, false, false, false));
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("parameter9", typeof(String), 100, false, null, false, false, false));
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("parameter10", typeof(String), 100, false, null, false, false, false));
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("owner", typeof(Int64), 0, true, null, false, false, false));
            dtdVpn.Columns.Add(new DatabaseColumnDefinition("name", typeof(String), 100, true, null, false, false, false));
            MaskTables.Add(dtdVpn);
        }

        private static void FillContent()
        {
            var dbc1 = new DatabaseContentDefinition("settings", 10);
            dbc1.RowContent = new Dictionary<string, object>
                         {
                             {"setting", "'heartbeat'"},
                             {"value", "'60000'"}
                         };
            DefaultContent.Add(dbc1);

            var dbc2 = new DatabaseContentDefinition("settings", 10);
            dbc2.RowContent = new Dictionary<string, object>
                         {
                             {"setting", "'maintmode'"},
                             {"value", "'0'"}
                         };
            DefaultContent.Add(dbc2);

            var dbc3 = new DatabaseContentDefinition("settings", 10);
            dbc3.RowContent = new Dictionary<string, object>
                         {
                             {"setting", "'useraccountcreation'"},
                             {"value", "'1'"}
                         };
            DefaultContent.Add(dbc3);

            var dbc4 = new DatabaseContentDefinition("settings", 10);
            dbc4.RowContent = new Dictionary<string, object>
                         {
                             {"setting", "'ribbonimg'"},
                             {"value", "'0'"}
                         };
            DefaultContent.Add(dbc4);

            var dbc5 = new DatabaseContentDefinition("settings", 10);
            dbc5.RowContent = new Dictionary<string, object>
                         {
                             {"setting", "'version'"},
                             {"value", "'0'"} //The Database-Version when the table was created
                         };
            DefaultContent.Add(dbc5);

            var dbc6 = new DatabaseContentDefinition("settings", 10);
            dbc6.RowContent = new Dictionary<string, object>
                         {
                             {"setting", "'guid'"},
                             {"value", "'" + Guid.NewGuid() + "'"}
                         };
            DefaultContent.Add(dbc6);

            #region OSes
            //Definition:
            //WindowsClient:    1000-1999
            //WindowsServer:    2000-2999
            //Hyper-V:          3000-3999
            //OSx:              4000-4999
            //Ubuntu:           5000-5999
            //Debian            6000-6999
            //OpenSuse          7000-7999
            //FreeBSD           8000-8999
            //Cisco iOS         9000-9999
            //Each new version increases the Number by 10 (if there are intermediate-releases that matters)
            //Each OS start at x100 (if there are older releases to attatch later)
            


            var dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "0"},
                             {"family", "'Undefined'"},
                             {"distribution", "'Operating'"},
                             {"version", "'System'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "1"},
                             {"family", "'Website'"},
                             {"distribution", "''"},
                             {"version", "''"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "1160"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows'"},
                             {"version", "'10'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "1150"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows'"},
                             {"version", "'8.1'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "1140"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows'"},
                             {"version", "'8'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "1130"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows'"},
                             {"version", "'7'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "1120"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows'"},
                             {"version", "'Vista'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "1110"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows'"},
                             {"version", "'XP'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "1100"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows'"},
                             {"version", "'2000'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "2170"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows Server'"},
                             {"version", "'10'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "2160"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows Server'"},
                             {"version", "'2012 R2'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "2150"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows Server'"},
                             {"version", "'2012'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "2140"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows Server'"},
                             {"version", "'2008 R2'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "2130"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows Server'"},
                             {"version", "'2008'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "2120"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows Server'"},
                             {"version", "'2003 R2'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "2110"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows Server'"},
                             {"version", "'2003'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "2100"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Windows Server'"},
                             {"version", "'2000'"}
                         };
            DefaultContent.Add(dbcOs);

            
            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "3100"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Hyper-V'"},
                             {"version", "'2008'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "3110"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Hyper-V'"},
                             {"version", "'2008 R2'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "3120"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Hyper-V'"},
                             {"version", "'2012'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "3130"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Hyper-V'"},
                             {"version", "'2012 R2'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "3140"},
                             {"family", "'Microsoft'"},
                             {"distribution", "'Hyper-V'"},
                             {"version", "'10'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "4100"},
                             {"family", "'Apple'"},
                             {"distribution", "'Mac OS X'"},
                             {"version", "'10.5 Leopard'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "4110"},
                             {"family", "'Apple'"},
                             {"distribution", "'Mac OS X'"},
                             {"version", "'10.6 Snow Leopard'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "4120"},
                             {"family", "'Apple'"},
                             {"distribution", "'Mac OS X'"},
                             {"version", "'10.7 Lion'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "4130"},
                             {"family", "'Apple'"},
                             {"distribution", "'Mac OS X'"},
                             {"version", "'10.8 Mountain Lion'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "4140"},
                             {"family", "'Apple'"},
                             {"distribution", "'Mac OS X'"},
                             {"version", "'10.9 Mavericks'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "4150"},
                             {"family", "'Apple'"},
                             {"distribution", "'Mac OS X'"},
                             {"version", "'10.10 Yosemite'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "5100"},
                             {"family", "'Linux'"},
                             {"distribution", "'Ubuntu'"},
                             {"version", "'7'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "5110"},
                             {"family", "'Linux'"},
                             {"distribution", "'Ubuntu'"},
                             {"version", "'8'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "5120"},
                             {"family", "'Linux'"},
                             {"distribution", "'Ubuntu'"},
                             {"version", "'9'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "5130"},
                             {"family", "'Linux'"},
                             {"distribution", "'Ubuntu'"},
                             {"version", "'10'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "5140"},
                             {"family", "'Linux'"},
                             {"distribution", "'Ubuntu'"},
                             {"version", "'11'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "5150"},
                             {"family", "'Linux'"},
                             {"distribution", "'Ubuntu'"},
                             {"version", "'12'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "5160"},
                             {"family", "'Linux'"},
                             {"distribution", "'Ubuntu'"},
                             {"version", "'13'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "5170"},
                             {"family", "'Linux'"},
                             {"distribution", "'Ubuntu'"},
                             {"version", "'14'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "5180"},
                             {"family", "'Linux'"},
                             {"distribution", "'Ubuntu'"},
                             {"version", "'15'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "6100"},
                             {"family", "'Linux'"},
                             {"distribution", "'Debian'"},
                             {"version", "'4'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "6110"},
                             {"family", "'Linux'"},
                             {"distribution", "'Debian'"},
                             {"version", "'5'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "6120"},
                             {"family", "'Linux'"},
                             {"distribution", "'Debian'"},
                             {"version", "'6'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "6130"},
                             {"family", "'Linux'"},
                             {"distribution", "'Debian'"},
                             {"version", "'7'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "7100"},
                             {"family", "'Linux'"},
                             {"distribution", "'OpenSuse'"},
                             {"version", "'10'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "7110"},
                             {"family", "'Linux'"},
                             {"distribution", "'OpenSuse'"},
                             {"version", "'11'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "7120"},
                             {"family", "'Linux'"},
                             {"distribution", "'OpenSuse'"},
                             {"version", "'12'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "7130"},
                             {"family", "'Linux'"},
                             {"distribution", "'OpenSuse'"},
                             {"version", "'13'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "8100"},
                             {"family", "'Linux'"},
                             {"distribution", "'FreeBSD'"},
                             {"version", "'6'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "8110"},
                             {"family", "'Linux'"},
                             {"distribution", "'FreeBSD'"},
                             {"version", "'7'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "8120"},
                             {"family", "'Linux'"},
                             {"distribution", "'FreeBSD'"},
                             {"version", "'8'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "8130"},
                             {"family", "'Linux'"},
                             {"distribution", "'FreeBSD'"},
                             {"version", "'9'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "9100"},
                             {"family", "'Cisco'"},
                             {"distribution", "'iOS'"},
                             {"version", "'12'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "9110"},
                             {"family", "'Cisco'"},
                             {"distribution", "'iOS'"},
                             {"version", "'13'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "9120"},
                             {"family", "'Cisco'"},
                             {"distribution", "'iOS'"},
                             {"version", "'14'"}
                         };
            DefaultContent.Add(dbcOs);

            dbcOs = new DatabaseContentDefinition("operatingsystems", 10);
            dbcOs.RowContent = new Dictionary<string, object>
                         {
                             {"id", "9130"},
                             {"family", "'Cisco'"},
                             {"distribution", "'iOS'"},
                             {"version", "'15'"}
                         };
            DefaultContent.Add(dbcOs);

            #endregion



        }

        #endregion

        #region check state
        /// <summary>
        /// Checks, if the Database is Up-To-Date
        /// </summary>
        /// <returns></returns>
        public static bool UpdateDatabaseStatus(bool checkOnly)
        {
            //Load the Table- and Content-Definitions
            if (MaskTables.Count == 0)
                FillDefinitions();
            
            var dbTables = StorageCore.Core.SchemaGetTableNames();
            var checkResult = true;
            
            foreach (var aMaskTable in MaskTables)
            {
                //Check if table exists; if so, check the table
                if (!dbTables.Contains(aMaskTable.Name))
                {
                    if (checkOnly)
                    {
                        Logger.Log(LogEntryType.Warning, "Database-Check: Table " + aMaskTable.Name + " not exists", "StorageCore");
                        checkResult = false;
                        continue;
                    }

                    StorageCore.Core.SchemaAddTable(aMaskTable);
                }

                //Table exists, so check it (even if it was created recently)
                if (CheckTable(aMaskTable, checkOnly) == false)
                {
                    Logger.Log(LogEntryType.Warning, "Database-Check: Table " + aMaskTable.Name + " check failed", "StorageCore");
                    checkResult = false;
                }
            }

            //Return true, if Value-Updates are successful or false if not
            if (checkOnly)
                return (checkResult);
            
            return (UpdateDefaultValues());
        }

        /// <summary>
        /// Adds Values to the Database, that are new to this version
        /// </summary>
        /// <returns></returns>
        private static bool UpdateDefaultValues()
        {
            //Get Database-Version
            var dbVersion = 0;
            Int32.TryParse(StorageCore.Core.GetSetting("version"), out dbVersion);

            var newDbVersion = 0;
            foreach (var dcd in DefaultContent)
            {
                if (dcd.DbVersion <= dbVersion) 
                    continue;

                //Add value here
                StorageCore.Core.SchemaAddRow(dcd);

                //Get the new expected DB-Version
                if (newDbVersion < dcd.DbVersion)
                    newDbVersion = dcd.DbVersion;
            }

            if (newDbVersion > dbVersion)
            {
                Logger.Log(LogEntryType.Info, "Database-Check: New content was added.", "StorageCore");

                var dcd = new DatabaseContentDefinition("settings", newDbVersion);
                dcd.RowContent = new Dictionary<string, object>
                                 {
                                     {"setting", "'version'"},
                                     {"value", "'" + newDbVersion + "'"}
                                 };
                StorageCore.Core.SchemaUpdateRow(dcd);
            }
            

            return (true);
        }

        /// <summary>
        /// Check if a Table is up to Date
        /// </summary>
        /// <param name="maskTable">The table to check</param>
        /// <param name="checkOnly"></param>
        /// <returns></returns>
        private static bool CheckTable(DatabaseTableDefinition maskTable, bool checkOnly)
        {
            var dbTable = StorageCore.Core.SchemaGetTable(maskTable.Name);

            //if the table exists, check each column
            foreach (var aMaskColumn in maskTable.Columns)
            {
                //Check if colum exists
                var foundColumn = false;
                for (var i = 0; i < dbTable.Rows.Count; i++)
                {
                    if (dbTable.Rows[i]["name"].ToString() != aMaskColumn.Name) 
                        continue;

                    foundColumn = true;
                    break;
                }
                if (foundColumn == false)
                {
                    if (checkOnly)
                    {
                        Logger.Log(LogEntryType.Warning, "Database-Check: Column " + aMaskColumn.Name + " in " + maskTable.Name + " not exists", "StorageCore");
                        return (false); //Table not up to date                        
                    }
                    //Create column
                    StorageCore.Core.SchemaAddColumn(aMaskColumn, maskTable.Name);
                }

                //Check if the columns are OK
                if (CheckColumns(aMaskColumn, dbTable) == false)
                {
                    Logger.Log(LogEntryType.Warning, "Database-Check: Column " + aMaskColumn.Name + " in " + maskTable.Name + " check failed.", "StorageCore");
                    //continue;
                }

            }

            return (true);
        }

        /// <summary>
        /// Checks if all properties of a column are matching the definition
        /// </summary>
        /// <param name="maskColumn"></param>
        /// <param name="dbTable"></param>
        /// <returns></returns>
        private static bool CheckColumns(DatabaseColumnDefinition maskColumn, DataTable dbTable)
        {
            for (var i = 0; i < dbTable.Rows.Count; i++)
            {
                var dR = dbTable.Rows[i];

                //Check if it is the right column
                if (dR["name"].ToString() != maskColumn.Name)
                    continue;

                var successfulCheck = true;

                //Check if type matches. If Autoincrement is aktivated, type INTEGER is OK
                if (StorageCore.Core.SchemaConvertDatatype(dR["type"]) != maskColumn.Type &&
                    StorageCore.Core.SchemaConvertDatatype(dR["type"]) != typeof(Int32) && maskColumn.Autoincrement)
                {
                    successfulCheck = false;
                    Logger.Log(LogEntryType.Warning, string.Format("Database-Check: Column {0} in {1} check failed. {4} {2} not matches {4} {3}",
                        maskColumn.Name, dbTable.TableName, StorageCore.Core.SchemaConvertDatatype(dR["type"]), maskColumn.Type, "Type"));
                }

                if (StorageCore.Core.SchemaConvertBoolean(dR["notnull"]) != maskColumn.NotNull)
                {
                    successfulCheck = false;
                    Logger.Log(LogEntryType.Warning, string.Format("Database-Check: Column {0} in {1} check failed. {4} {2} not matches {4} {3}",
                        maskColumn.Name, dbTable.TableName, StorageCore.Core.SchemaConvertDatatype(dR["notnull"]), maskColumn.NotNull, "NotNull"));
                }

                if (dR["dflt_value"].ToString() != "System.Object" &&
                    dR["dflt_value"].ToString() != "System.DbNull" &&
                    dR["dflt_value"].ToString() != maskColumn.Default.ToString() &&
                    (dR["dflt_value"].ToString() != "''" && maskColumn.Default.ToString() == ""))
                {
                    successfulCheck = false;
                    Logger.Log(LogEntryType.Warning, string.Format("Database-Check: Column {0} in {1} check failed. {4} {2} not matches {4} {3}",
                        maskColumn.Name, dbTable.TableName, StorageCore.Core.SchemaConvertDatatype(dR["dflt_value"]), maskColumn.Default, "DefaultValue"));
                }

                if (StorageCore.Core.SchemaConvertBoolean(dR["pk"]) != maskColumn.IsPrimaryKey)
                {
                    successfulCheck = false;
                    Logger.Log(LogEntryType.Warning, string.Format("Database-Check: Column {0} in {1} check failed. {4} {2} not matches {4} {3}",
                        maskColumn.Name, dbTable.TableName, StorageCore.Core.SchemaConvertDatatype(dR["pk"]), maskColumn.IsPrimaryKey, "PrimaryKey"));
                }

                return (successfulCheck);
            }
            
            return (false);
        }
        #endregion

        
    }
}
