namespace beRemote.Core.Definitions.Classes.UpdateDatabase
{
    /// <summary>
    /// Class to adding a new Column to an Existing Table
    /// </summary>
    public class UpdateDatabaseNewTableColumn
    {
        private string _Tablename = "";
        private UpdateDatabaseTableColumn _Column = new UpdateDatabaseTableColumn();

        /// <summary>
        /// The Name of the new Database Table
        /// </summary>
        public string Tablename
        {
            get { return (_Tablename); }
            set { _Tablename = value; }
        }

        /// <summary>
        /// The new Column
        /// </summary>
        public UpdateDatabaseTableColumn Column
        {
            get { return (_Column); }
            set { _Column = value; }
        }
    }
}
