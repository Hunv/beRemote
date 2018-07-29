namespace beRemote.Core.Definitions.Classes.UpdateDatabase
{
    public class UpdateDatabaseDeleteTable
    {
        private string _Tablename = "";

        /// <summary>
        /// The Table to flush
        /// </summary>
        public string Tablename
        {
            get { return (_Tablename); }
            set { _Tablename = value; }
        }
    }
}
