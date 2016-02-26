namespace beRemote.Core.Definitions.Classes
{
    public class Folder
    {
        private long _Id = 0;
        private string _Name = "";
        private long _ParentId = 0;
        private int _SortOrder = 0;
        private int _Owner = 0;
        private bool _IsPublic = false;

        public Folder(long Id, string Name, long ParentId, int SortOrder, int Owner, bool IsPublic)
        {
            _Id = Id;
            _Name = Name;
            _ParentId = ParentId;
            _SortOrder = SortOrder;
            _Owner = Owner;
            _IsPublic = IsPublic;
        }

        public long getId() { return (_Id); }
        public string getName() { return (_Name); }
        public long getParentId() { return (_ParentId); }
        public int getSortOrder() { return (_SortOrder); }
        public int getOwner() { return (_Owner); }
        public bool getIsPublic() { return (_IsPublic); }

        public long Id { get { return _Id; } set { _Id = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public long ParentId { get { return _ParentId; } set { _ParentId = value; } }
        public int SortOrder { get { return _SortOrder; } set { _SortOrder = value; } }
        public int Owner { get { return _Owner; } set { _Owner = value; } }
        public bool IsPublic { get { return _IsPublic; } set { _IsPublic = value; } }
    }
}
