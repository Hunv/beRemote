using System;
using System.Collections.Generic;
using System.ComponentModel;
using beRemote.Core.Definitions.Enums.Filter;

namespace beRemote.Core.Definitions.Classes
{
    [Serializable]
    public class FilterClass : INotifyPropertyChanged
    {
        public FilterClass(FilterType fType)
        {
            ConditionType = fType;
        }

        private List<FilterClass> _SubConditions = new List<FilterClass>();
        private string _Description = "";
        private FilterType _ConditionType;
        private string _Attribute = "";
        private object _Value;
        private FilterOperation _Operation;
        private FilterCombination _Combination;
        private bool _IsOr;
        private bool _IsNot;
        private bool _IsLike;
        private long _Id;

        //To Update Properties in the Listbox
        public event PropertyChangedEventHandler PropertyChanged;

        public List<FilterClass> SubConditions
        {
            get { return _SubConditions; }
            set
            {
                _SubConditions = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SubConditions"));
                }
            }
        }
        public string Description
        {
            get { return _Description; }
            set
            {
                if (value == "")
                    _Description = null;
                else
                    _Description = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                }
            }
        }
        public FilterType ConditionType
        {
            get { return _ConditionType; }
            set
            {
                _ConditionType = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ConditionType"));
                    PropertyChanged(this, new PropertyChangedEventArgs("DisplayText"));
                }
            }
        }
        public string Attribute
        {
            get { return _Attribute; }
            set
            {
                if (value == "")
                    _Attribute = null;
                else
                    _Attribute = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Attribute"));
                }
            }
        }
        public object ValueId
        {
            get
            {
                switch (ConditionType)
                {
                    //The the ID of the first crendential with the given Name
                    case FilterType.Credential:
                        if (Value == null)
                            return (0);

                        var uCred = (List<UserCredential>) ValueIdSource; //StorageCore.Core.GetUserCredentialsAll();
                        foreach (var uC in uCred)
                        {
                            if ((IsLike == false && uC.Description == Value.ToString()) || //equal
                                (IsLike && uC.Description.Contains(Value.ToString()))) //Like
                            {
                                return (uC.Id);
                            }
                        }
                        return (0); //if there are no User Credentials

                    case FilterType.Folder:
                        if (Value == null)
                            return (0);

                        var fldrs = (List<Folder>) ValueIdSource; //StorageCore.Core.GetFolders();
                        foreach (var fldr in fldrs)
                        {
                            if ((IsLike == false && fldr.Name == Value.ToString()) || //equal
                                (IsLike && fldr.Name.Contains(Value.ToString()))) //Like
                            {
                                return (fldr.Id);

                            }
                        }
                        return (0); //If there are no folders

                    case FilterType.OperatingSystem:
                        if (Value == null)
                            return (0);

                        var oss = (List<OSVersion>) ValueIdSource;//StorageCore.Core.GetOperatingSystemList();
                        foreach (var os in oss)
                        {
                            if ((IsLike == false && os.DisplayText == Value.ToString()) || //equal
                                (IsLike && os.DisplayText.Contains(Value.ToString()))) //Like
                            {
                                return (os.Id);

                            }
                        }
                        return (0); //If there are no OSes (/something went wrong)

                    case FilterType.Protocol:
                        if (Value == null)
                            return (0);
                        
                        foreach (var aProtocol in (List<string>)ValueIdSource)
                        { 
                            if ((IsLike == false && aProtocol == Value.ToString()) || //equal
                                (IsLike && aProtocol.Contains(Value.ToString()))) //Like
                            {
                                return (aProtocol);
                            }
                        }

                        return ("unknown"); //If there are no protocols
                }

                return _Value;
            }
        }
        public object Value
        {
            get { return _Value; }
            set
            {
                //Empty string = null
                if (value is string && value.ToString() == "")
                    _Value = null;
                else
                    _Value = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                    PropertyChanged(this, new PropertyChangedEventArgs("DisplayText"));
                    PropertyChanged(this, new PropertyChangedEventArgs("DisplayColor"));
                }
            }
        }
        public bool IsOr
        {
            get { return _IsOr; }
            set
            {
                _IsOr = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsOr"));
                    PropertyChanged(this, new PropertyChangedEventArgs("DisplayText"));
                }
            }
        }
        public bool IsNot
        {
            get { return _IsNot; }
            set
            {
                _IsNot = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsNot"));
                    PropertyChanged(this, new PropertyChangedEventArgs("DisplayText"));
                }

            }
        }
        public bool IsLike
        {
            get { return _IsLike; }
            set
            {
                _IsLike = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsLike"));
                    PropertyChanged(this, new PropertyChangedEventArgs("DisplayText"));
                }

            }
        }

        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public FilterOperation Operation
        {
            get
            {
                if (IsLike == true) //LIKE-Modificator
                {
                    if (IsNot == false)
                        return FilterOperation.Like;
                    else
                        return FilterOperation.NotLike;
                }
                else //No special Modificator
                {
                    if (IsNot == false)
                        return FilterOperation.Equal;
                    else
                        return FilterOperation.NotEqual;
                }

            }
            set
            {
                //Switch not possible :(
                if (value == FilterOperation.Equal)
                {
                    IsLike = false;
                    IsNot = false;
                }
                else if (value == FilterOperation.Like)
                {
                    IsLike = true;
                    IsNot = false;
                }
                else if (value == FilterOperation.NotEqual)
                {
                    IsLike = false;
                    IsNot = true;
                }
                else if (value == FilterOperation.NotLike)
                {
                    IsLike = true;
                    IsNot = true;
                }

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsLike"));
                    PropertyChanged(this, new PropertyChangedEventArgs("IsNot"));
                    PropertyChanged(this, new PropertyChangedEventArgs("DisplayText"));
                }

            }
        }

        public FilterCombination Combination
        {
            get
            {
                if (IsOr == true)
                    return FilterCombination.Or;
                else
                    return FilterCombination.And;
            }
            set
            {
                if (value == FilterCombination.And)
                    IsOr = false;
                else
                    IsOr = true;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsOr"));
                    PropertyChanged(this, new PropertyChangedEventArgs("DisplayText"));
                }
            }
        }

        /// <summary>
        /// Contains the Source for getting the ValueId of this filter. (i.e. get the OS-ID of "Microsoft Windows Server 2012 R2")
        /// </summary>
        public object ValueIdSource
        {
            get;
            set;
        }

        public string DisplayText
        {
            get
            {
                if (ConditionType == FilterType.Collection)
                {
                    return ((IsOr ? "OR " : "AND ") + "matches FilterSet #" + (Value == null ? "Undefined" : Value.ToString())); //Or/And
                }
                else
                {
                    return (IsOr ? "OR " : "AND ") + //Or/And
                        ConditionType.ToString() +
                        " is" + (IsNot ? " NOT" : "") + //Name is NOT
                        (IsLike ? " like " : " equal to ") + //like / equal to
                        (Value == null ? "-missing-" : "\"" + Value.ToString() + "\""); //"test"
                }
            }
        }


        public System.Windows.Media.SolidColorBrush DisplayColor
        {
            get
            {
                if (IsValidValue == false)
                {
                    return (System.Windows.Media.Brushes.Red);
                }
                else
                {
                    return (System.Windows.Media.Brushes.Black);
                }

            }
        }

        public bool IsValidValue
        {
            get
            {
                switch (ConditionType)
                {
                    //Check for numeric value
                    case FilterType.Port:
                        if (this.Value == null)
                            return (false);

                        Int32 ignore;
                        if (Int32.TryParse(this.Value.ToString(), out ignore) == false)
                        {
                            return (false);
                        }
                        break;

                    //Strings must be non-emtpy
                    case FilterType.Credential:
                    case FilterType.Description:
                    case FilterType.Folder:
                    case FilterType.Host:
                    case FilterType.Name:
                    case FilterType.OperatingSystem:
                    case FilterType.Protocol:
                    case FilterType.ProtocolSetting:
                        if (this.Value == null || this.Value.ToString() == "")
                        {
                            return (false);
                        }
                        break;
                }
                return (true);
            }
        }


        public FilterClass Filter { get { return this; } }
    }
}
