using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Core.StorageSystem.StorageBase
{
    public class DatabaseTableDefinition
    {
        public DatabaseTableDefinition(string name)
        {
            Name = name;
            Columns = new List<DatabaseColumnDefinition>();
        }

        /// <summary>
        /// The Name of the Table
        /// </summary>
        public string Name { get; set; }

        public List<DatabaseColumnDefinition> Columns {get;set;}
    }
}
