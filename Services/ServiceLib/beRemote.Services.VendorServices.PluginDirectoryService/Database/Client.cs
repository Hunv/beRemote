using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using beRemote.Services.PluginDirectory.Library.Objects;
using Version = System.Version;

namespace beRemote.Services.VendorServices.PluginDirectoryService.Database
{
    public class Client : IDisposable
    {
        private readonly String _connectionString =
            "Server={0};Database={1};User Id={2};Password={3};";

        private readonly SqlConnection _sqlConnection;

        public Dictionary<Object, BaseObject> ObjectCache = new Dictionary<Object, BaseObject>();

        private DatabaseConfig _databaseConfig;

        public Client(DatabaseConfig config)
        {
            _databaseConfig = config;

            _connectionString = String.Format(_connectionString, _databaseConfig.SqlServer,
                _databaseConfig.SqlDatabase, _databaseConfig.SqlUsername,
                _databaseConfig.SqlPassword);

            _sqlConnection = new SqlConnection(_connectionString);

        }

        public void Open()
        {
            _sqlConnection.Open();
        }

        public void Close()
        {
            _sqlConnection.Close();
            _sqlConnection.Dispose();
        }

        public void Dispose()
        {
            Close();
        }


        private T CastToLibraryObject<T>(DataRow row, Type targetLibraryType, DataColumnCollection columns) where T:BaseObject, new()
        {
            var newObj = new T();
            
            foreach (var propInfo in targetLibraryType.GetProperties())
            {
                try
                {
                    if (columns.Contains(propInfo.Name))
                    {
                        
                        propInfo.SetValue(newObj, row[columns[propInfo.Name]]);
                    }

                    
                }
// ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }
            }

            return newObj;
        }

        private DataTable GetTable(String query)
        {
            return GetTable(query, null);
        }

        private DataTable GetTable(string query, Dictionary<string, object> parameters)
        {
            DataTable table;
            using (var command = new SqlCommand(query, _sqlConnection))
            {
                if (parameters != null)
                {
                    foreach (var kvp in parameters)
                    {
                        command.Parameters.AddWithValue(kvp.Key, kvp.Value);
                    }
                }

                var dataset = new DataSet();
                var adapter = new SqlDataAdapter { SelectCommand = command };
                adapter.Fill(dataset);

                table = dataset.Tables[0];
            }

            return table;
        }
            
        public List<Author> GetAllAuthors()
        {
            var authorsTable = GetTable("SELECT * FROM authors");

            return (from DataRow row in authorsTable.Rows select CastToLibraryObject<Author>(row, typeof (Author), authorsTable.Columns)).ToList();
        }


        /// <summary>
        /// Will get a list of all available plugins
        /// </summary>
        /// <param name="clientVersion"></param>
        /// <returns></returns>
        public List<Plugin> GetAllPlugins()
        {
            var pluginsTable = GetTable("SELECT * FROM plugins");

            //foreach (var row in pluginsTable.Rows)
            //{

            //}
            //return null;
            return (from DataRow row in pluginsTable.Rows select CastToLibraryObject<Plugin>(row, typeof(Plugin), pluginsTable.Columns)).ToList();
        }

        public PluginType GetPluginType(Guid guid)
        {
            if (ObjectCache.ContainsKey(guid))
            {
                return (PluginType)ObjectCache[guid];
            }

            var pTypeTable = GetTable("SELECT * FROM plugin_types WHERE Id = '" + guid + "'");

            var pluginType = CastToLibraryObject<PluginType>(pTypeTable.Rows[0], typeof(PluginType), pTypeTable.Columns);

            ObjectCache.Add(guid, pluginType);

            return pluginType;
        }

        public PluginDirectory.Library.Objects.Version GetVersion(Guid guid)
        {
            if (ObjectCache.ContainsKey(guid))
            {
                return (PluginDirectory.Library.Objects.Version)ObjectCache[guid];
            }

            var versionTable = GetTable("SELECT * FROM versions WHERE Id = '" + guid + "'");

            var version = CastToLibraryObject<PluginDirectory.Library.Objects.Version>(versionTable.Rows[0], typeof(PluginDirectory.Library.Objects.Version), versionTable.Columns);

            ObjectCache.Add(guid, version);

            return version;
        }

        public Author GetAuthor(Guid guid)
        {
            if (ObjectCache.ContainsKey(guid))
            {
                return (Author)ObjectCache[guid];
            }

            var authorTable = GetTable("SELECT * FROM authors WHERE Id = '" + guid + "'");
                
            var author = CastToLibraryObject<Author>(authorTable.Rows[0], typeof(Author), authorTable.Columns);

            ObjectCache.Add(guid, author);

            return author;
        }

        public Group[] GetPluginGroups(Guid guid)
        {
            var groupAssignmentsTable = GetTable("SELECT * FROM group_assignments WHERE PluginId = '" + guid +  "'");

            var groupList = new List<Group>();

            foreach (var groupId in from DataRow row in groupAssignmentsTable.Rows select (Guid)row["GroupId"])
            {
                if(false == ObjectCache.ContainsKey(groupId))
                {
                    var groupTable = GetTable("SELECT * FROM groups WHERE Id = '" + groupId + "'");

                    var group = CastToLibraryObject<Group>(groupTable.Rows[0], typeof (Group), groupTable.Columns);

                    ObjectCache.Add(groupId, group);
                }

                groupList.Add((Group)ObjectCache[groupId]);
            }

            return groupList.ToArray();
        }

        public SearchTerm[] GetPluginSearchTerms(Guid guid)
        {
            var searchTermTable = GetTable("SELECT * FROM searchterms WHERE PluginId = '" + guid + "'");

            var searchTermList = new List<SearchTerm>();

            foreach (DataRow row in searchTermTable.Rows)
            {
                var termId = (Guid)row["Id"];
                if (false == ObjectCache.ContainsKey(termId))
                {
                    var term = CastToLibraryObject<SearchTerm>(row, typeof(SearchTerm), searchTermTable.Columns);
                    ObjectCache.Add(termId, term);
                }

                searchTermList.Add((SearchTerm)ObjectCache[termId]);
            }

            //foreach (var termId in from DataRow row in searchTermTable.Rows select (Guid)row["Id"])
            //{
            //    if (false == ObjectCache.ContainsKey(termId))
            //    {
            //        var term = CastToLibraryObject<SearchTerm>(row, typeof(SearchTerm), searchTermTable.Columns);
            //    }

            //    searchTermList.Add((SearchTerm)ObjectCache[termId]);
            //}
            return searchTermList.ToArray();
        }

        public String GetDbConfigValue(String configKey)
        {
            var configTable = GetTable("SELECT * FROM dbconfig WHERE configkey = '" + configKey + "'");

            if (configTable.Rows.Count == 0)
                return "no such key";
            else
            {
                return configTable.Rows[0]["configvalue"].ToString();
            }
        }

        internal Plugin GetPlugin(string id)
        {
            foreach (var plg in GetAllPlugins())
            {
                if (plg.Id.Equals(new Guid(id)))
                    return plg;
            }

            throw new Exception("Plugin with id " + id + " not found");
        }

        public List<Plugin> GetUseablePlugins(Version version, String channel)
        {
            String sql = "SELECT * FROM plugins left join versions on plugins.RequiredBeRemoteVersionId = versions.Id WHERE Channel like @Channel AND Major=@Major AND Minor=@Minor AND Build=@Build AND Revision=@Revision";

            var parameters = new Dictionary<String, object>();
            parameters.Add("@Channel", channel.ToLower());
            parameters.Add("@Major", version.Major);
            parameters.Add("@Minor", version.Minor);
            parameters.Add("@Build", version.Build);
            parameters.Add("@Revision", version.Revision);

            var pluginsTable = GetTable(sql, parameters);

            //foreach (var row in pluginsTable.Rows)
            //{

            //}
            //return null;
            return (from DataRow row in pluginsTable.Rows select CastToLibraryObject<Plugin>(row, typeof(Plugin), pluginsTable.Columns)).ToList();
        }



        internal string GetProvisioningPath(Guid provisioningId)
        {
            String sql = "SELECT * FROM provisioning WHERE Id = '" + provisioningId + "'";

            var pluginsTable = GetTable(sql);

            try
            {
                return pluginsTable.Rows[0]["provisioningpath"].ToString();
            }
            catch (Exception)
            {
                return "Given provisioning id is not valid.";
            }
        }
    }
}