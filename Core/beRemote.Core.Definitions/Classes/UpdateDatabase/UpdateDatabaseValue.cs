namespace beRemote.Core.Definitions.Classes.UpdateDatabase
{
    public class UpdateDatabaseValue
    {
        private string _Columnname = "";
        private string _Value = "";

        /// <summary>
        /// The Name of the new Database Table
        /// </summary>
        public string Columnname
        {
            get { return (_Columnname); }
            set { _Columnname = value; }
        }

        /// <summary>
        /// The Name of the new Database Table
        /// </summary>
        public string Value
        {
            get { return (_Value); }
            set { _Value = value; }
        }
    }
}
