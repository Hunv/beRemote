using System;

namespace beRemote.Services.PluginDirectory.Library.Objects
{
    public class Author : BaseObject
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String TeamName { get; set; }
        public String Mail { get; set; }
        public String Website { get; set; }
        public String Phone { get; set; }
        public bool ApprovedAuthor { get; set; }
    }
}
