namespace beRemote.Core.Definitions.Classes.UpdateDatabase
{
    public class UpdateDatabaseDeleteTableColumn
    {
        private string _Tablename = "";
        private string _Columnname = "";

        /// <summary>
        /// Tablename of the Table, where the Column should be deleted from
        /// </summary>
        public string Tablename
        {
            get { return (_Tablename); }
            set { _Tablename = value; }
        }

        /// <summary>
        /// The Columnname, that should be deleted
        /// </summary>
        public string Columnname
        {
            get { return (_Columnname); }
            set { _Columnname = value; }
        }
    }
}
