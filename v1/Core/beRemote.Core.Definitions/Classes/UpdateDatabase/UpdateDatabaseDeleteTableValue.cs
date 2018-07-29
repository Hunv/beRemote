using System.Collections.Generic;

namespace beRemote.Core.Definitions.Classes.UpdateDatabase
{
    public class UpdateDatabaseDeleteTableValue
    {
        private string _Tablename = "";
        private List<UpdateDatabaseValue> _WhereClause = new List<UpdateDatabaseValue>();

        /// <summary>
        /// Tablename of the Table, where the Values should be deleted from
        /// </summary>
        public string Tablename
        {
            get { return (_Tablename); }
            set { _Tablename = value; }
        }

        /// <summary>
        /// Defines the where-Clause
        /// </summary>
        public List<UpdateDatabaseValue> WhereClause
        {
            get { return (_WhereClause); }
            set { _WhereClause = value; }
        }
    }
}
