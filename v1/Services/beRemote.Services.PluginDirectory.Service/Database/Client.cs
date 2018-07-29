using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using beRemote.Services.PluginDirectory.Library.Objects;

namespace beRemote.Services.PluginDirectory.Service.Database
{
    public class Client : IDisposable
    {
        private readonly String _connectionString =
            "Server={0};Database={1};User Id={2};Password={3};";

        private readonly SqlConnection _sqlConnection;

        public Dictionary<Object, BaseObject> ObjectCache = new Dictionary<Object, BaseObject>();

        public Client()
        {
            _connectionString = String.Format(_connectionString, Properties.Settings.Default.SQLServer,
                Properties.Settings.Default.SQLDatabase, Properties.Settings.Default.SQLUser,
                Properties.Settings.Default.SQLPasswordObfuscated);

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
            DataTable table;
            using (var command = new SqlCommand(query, _sqlConnection))
            {
                var dataset = new DataSet();
                var adapter = new SqlDataAdapter {SelectCommand = command};
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

        public Library.Objects.Version GetVersion(Guid guid)
        {
            if (ObjectCache.ContainsKey(guid))
            {
                return (Library.Objects.Version)ObjectCache[guid];
            }

            var versionTable = GetTable("SELECT * FROM versions WHERE Id = '" + guid + "'");

            var version = CastToLibraryObject<Library.Objects.Version>(versionTable.Rows[0], typeof(Library.Objects.Version), versionTable.Columns);

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
    }
}