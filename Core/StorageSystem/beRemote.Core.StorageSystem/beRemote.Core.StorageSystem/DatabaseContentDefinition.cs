using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Core.StorageSystem.StorageBase
{
    public class DatabaseContentDefinition
    {
        public DatabaseContentDefinition(string tablename, int dbVersion)
        {
            Tablename = tablename;
            DbVersion = dbVersion;
        }

        /// <summary>
        /// The Table the content belongs to
        /// </summary>
        public string Tablename { get; set; }

        /// <summary>
        /// The content for a row in the table
        /// </summary>
        public Dictionary<string, object> RowContent { get; set; }

        /// <summary>
        /// Defines the Database-Version where this 
        /// </summary>
        public int DbVersion { get; set; }
    }
}
