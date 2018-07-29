namespace beRemote.Core.Definitions.Classes.UpdateDatabase
{
    public class UpdateDatabaseTableColumn
    {
        private string _Name = "";
        private string _Type = "";
        private string _Default = "";
        private bool _AutoIncrement = false;
        private bool _Identification = false;

        /// <summary>
        /// The Name of the Column
        /// </summary>
        public string Name
        {
            get { return (_Name); }
            set { _Name = value; }
        }

        /// <summary>
        /// The Type of the Column
        /// </summary>
        public string Type
        {
            get { return (_Type); }
            set { _Type = value; }
        }

        /// <summary>
        /// The Default value for the Column
        /// </summary>
        public string Default
        {
            get { return (_Default); }
            set { _Default = value; }
        }

        /// <summary>
        /// Is it AutoIncremented? Will set Identification to true, if true
        /// </summary>
        public bool AutoIncrement
        {
            get { return (_AutoIncrement); }
            set { _AutoIncrement = value; Identification = true; }
        }

        /// <summary>
        /// Is it Identification-Column?
        /// </summary>
        public bool Identification
        {
            get { return (_Identification); }
            set { _Identification = value; }
        }

    }
}
