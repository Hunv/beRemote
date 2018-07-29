using System.Collections.Generic;

namespace beRemote.Core.Definitions.Classes.UpdateDatabase
{
    public class UpdateDatabaseNewTableValue
    {
        private string _Tablename = "";
        private List<UpdateDatabaseValue> _Values = new List<UpdateDatabaseValue>();

        /// <summary>
        /// Tablename of the Talbe where the new Values should be added to
        /// </summary>
        public string Tablename
        {
            get { return (_Tablename); }
            set { _Tablename = value; }
        }

        /// <summary>
        /// The Value
        /// </summary>
        public List<UpdateDatabaseValue> Values
        {
            get { return (_Values); }
            set { _Values = value; }
        }
    }
}
