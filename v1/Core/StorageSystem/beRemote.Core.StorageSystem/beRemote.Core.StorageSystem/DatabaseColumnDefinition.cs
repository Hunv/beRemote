using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Core.StorageSystem.StorageBase
{
    public class DatabaseColumnDefinition
    {
        public DatabaseColumnDefinition(string name, Type type, int typeLenght, bool notNull, object defaultValue, bool isIndex, bool isPrimaryKey, bool autoincrement)
        {
            Name = name;
            Type = type;
            TypeLenght = typeLenght;
            NotNull = notNull;
            Default = defaultValue;
            IsIndex = isIndex;
            IsPrimaryKey = isPrimaryKey;
            Autoincrement = autoincrement;
        }

        /// <summary>
        /// The Name of the Table
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the Column
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// If a type can have a lenght, the lenght is defined here
        /// </summary>
        public int TypeLenght { get; set; }

        /// <summary>
        /// Is a null-value allowed?
        /// </summary>
        public bool NotNull { get; set; }

        /// <summary>
        /// The Default value of this column; must be type of property Type
        /// </summary>
        public object Default { get; set; }

        /// <summary>
        /// Is this column an index-column?
        /// </summary>
        public bool IsIndex { get; set; }

        /// <summary>
        /// Is this column the primaryKey?
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Is this column the Autoincremented?
        /// </summary>
        public bool Autoincrement { get; set; }
    }
}
