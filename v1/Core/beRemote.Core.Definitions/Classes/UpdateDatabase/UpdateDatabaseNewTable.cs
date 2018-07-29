using System.Collections.Generic;

namespace beRemote.Core.Definitions.Classes.UpdateDatabase
{
    public class UpdateDatabaseNewTable
    {
        private string _Tablename = "";
        private List<UpdateDatabaseTableColumn> _Columns = new List<UpdateDatabaseTableColumn>();

        /// <summary>
        /// The Name of the new Database Table
        /// </summary>
        public string Tablename
        {
            get { return (_Tablename); }
            set { _Tablename = value; }
        }

        /// <summary>
        /// The Columns of the new Table
        /// </summary>
        public List<UpdateDatabaseTableColumn> Columns
        {
            get { return (_Columns); }
            set { _Columns = value; }
        }
    }
}
