using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using beRemote.Core;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.Definitions.Enums.Filter;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.GUI.Tabs.ManageFilter
{
    /// <summary>
    /// Interaction logic for ContentTabAbout.xaml
    /// </summary>
    public partial class TabManageFilter
    {
        public TabManageFilter()
        {
            ProtocolList = Kernel.GetAvailableProtocols().Values.ToList();
            InitializeComponent();
        }

        #region Private Variables
        private ObservableCollection<FilterClass> _Filter = new ObservableCollection<FilterClass>();
        private List<FilterSet> _FilterSetList = new List<FilterSet>();
        private long _SelectedFilterSetId; //Contains the FilterSetId of the currently selected Filter
        private List<ConnectionHost> _PreviewResult = new List<ConnectionHost>(); //The result of a Preview
        private List<string> _ComboBoxContent = new List<string>(); //The list, that the Combobox of the currently selected filter uses        
        #endregion

        #region public Properties
        public ObservableCollection<FilterClass> FilterList 
        {
            get { return _Filter; } 
            set 
            {
                _Filter = value;
                RaisePropertyChanged("FilterList");
                RaisePropertyChanged("DisplayColor");
            } 
        }

        public List<FilterSet> FilterSetList 
        {
            get { return _FilterSetList; } 
            set
            {
                _FilterSetList = value;
                try
                {
                    RaisePropertyChanged("FilterSetList");
                }
                catch
                { }
            } 
        }

        public List<ConnectionHost> PreviewResult
        {
            get { return _PreviewResult; }
            set
            {
                _PreviewResult = value;
                RaisePropertyChanged("ReviewResult");
            }
        }

        public List<string> ComboBoxContent
        {
            get { return _ComboBoxContent; }
            set { _ComboBoxContent = value; }
        }


        private bool _SelectedFilterIsString;
        public bool SelectedFilterIsString
        {
            get { return _SelectedFilterIsString; }
            set
            {
                _SelectedFilterIsString = value;

                if (value)
                {
                    SelectedFilterIsBoolean = false;
                    SelectedFilterIsFilterSet = false;
                    SelectedFilterIsProtocol = false;
                }

                RaisePropertyChanged("SelectedFilterIsString");
            }
        }

        private bool _SelectedFilterIsBoolean;
        public bool SelectedFilterIsBoolean
        {
            get { return _SelectedFilterIsBoolean; }
            set
            {
                _SelectedFilterIsBoolean = value;

                if (value)
                {
                    SelectedFilterIsString = false;
                    SelectedFilterIsFilterSet = false;
                    SelectedFilterIsProtocol = false;
                }

                RaisePropertyChanged("SelectedFilterIsBoolean");
            }
        }

        private bool _SelectedFilterIsFilterSet;
        public bool SelectedFilterIsFilterSet
        {
            get { return _SelectedFilterIsFilterSet; }
            set
            {
                _SelectedFilterIsFilterSet = value;

                if (value)
                {
                    SelectedFilterIsString = false;
                    SelectedFilterIsBoolean = false;
                    SelectedFilterIsProtocol = false;
                }

                RaisePropertyChanged("SelectedFilterIsFilterSet");
            }
        }

        private bool _SelectedFilterIsProtocol;
        public bool SelectedFilterIsProtocol
        {
            get { return _SelectedFilterIsProtocol; }
            set
            {
                _SelectedFilterIsProtocol = value;

                if (value)
                {
                    SelectedFilterIsString = false;
                    SelectedFilterIsBoolean = false;
                    SelectedFilterIsFilterSet = false;
                }

                RaisePropertyChanged("SelectedFilterIsProtocol");
            }
        }

        #region ProtocolList
        public List<Protocol> ProtocolList { get; set; }
        #endregion
        #endregion

        #region Public Methods
        public void AddFilter(FilterType fType)
        {
            #region Conditions to collection
            if (lstFilter.SelectedValue != null && ((FilterClass)lstFilter.SelectedValue).ConditionType == FilterType.Collection)
            {
                switch (fType)
                {
                    case FilterType.Name:
                    case FilterType.Host:
                    case FilterType.Description:
                    case FilterType.OperatingSystem:
                    case FilterType.Folder:
                    case FilterType.Protocol:
                    case FilterType.Credential:
                    case FilterType.Public:
                    case FilterType.Port:
                    case FilterType.ProtocolSetting:
                    case FilterType.Connection:
                    case FilterType.Collection:
                        var newFilter = new FilterClass(fType);
                        ((FilterClass)lstFilter.SelectedValue).SubConditions.Add(newFilter);
                        break;
                }
            }
            #endregion
            #region Conditions to root
            else
            {
                switch (fType)
                {
                    case FilterType.Name:
                    case FilterType.Host:
                    case FilterType.Description:
                    case FilterType.OperatingSystem:
                    case FilterType.Folder:
                    case FilterType.Protocol:
                    case FilterType.Credential:
                    case FilterType.Public:
                    case FilterType.Port:
                    case FilterType.ProtocolSetting:
                    case FilterType.Connection:
                    case FilterType.Collection:
                        var newFilter = new FilterClass(fType);
                        FilterList.Add(newFilter);
                        break;
                }
            }

            RaisePropertyChanged("FilterList");
            lstFilter.SelectedIndex = lstFilter.Items.Count - 1;
            
            #endregion
        }
        #endregion
        
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            RaisePropertyChanged("FilterList");
        }

        private void btnRefreshPreview_Click(object sender, RoutedEventArgs e)
        {
            //Get the Preview Connections and show it in the Listbox for the Preview
            if (lstFilterOverview.SelectedValue != null)
            {
                PreviewResult = StorageCore.Core.GetFilterResult((FilterSet)lstFilterOverview.SelectedValue, ProtocolList.Select(p => p.GetProtocolIdentifer()).ToList());
            }
        }

        private void btnSetSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        /// <summary>
        /// Saves the current configuration
        /// </summary>
        private void Save()
        {
            //Save the current FilterList
            foreach (FilterSet fS in FilterSetList)
            {
                if (FilterValidation(fS))
                {

                    if (fS.Id == _SelectedFilterSetId && fS.Id != 0 && _SelectedFilterSetId != 0)
                    {
                        fS.Filter = FilterList.ToList();
                    }

                    StorageCore.Core.ModifyFilterSet(fS);
                                        
                    OnConnectionFilterChanged(new RoutedEventArgs());
                }
                else
                {
                    MessageBox.Show("FilterSet \"" + fS.Title + "\" has an invalid filter. Please check it and save again" + Environment.NewLine + "Your settings were NOT saved!", "Couldn't save filterset", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Checks if all Filter in a FilterSet are valid
        /// </summary>
        /// <param name="fS"></param>
        /// <returns></returns>
        private bool FilterValidation(FilterSet fS)
        {
            foreach (FilterClass fC in fS.Filter)
            {
                if (FilterValidation(fC) == false)
                {
                    return (false);
                }
            }
            return (true);
        }

        /// <summary>
        /// Checks if all Filter in a FilterClass are valid
        /// </summary>
        /// <param name="fC"></param>
        /// <returns></returns>
        private bool FilterValidation(FilterClass fC)
        {
            if (fC.IsValidValue == false)
            {
                return (false);
            }

            foreach (FilterClass subfC in fC.SubConditions)
            {
                if (FilterValidation(subfC) == false)
                {
                    return (false);
                }
            }

            return (true);
        }
        
        private void btnFilterDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstFilter.SelectedValue != null && lstFilterOverview.SelectedValue != null)
            {
                for (var i = 0; i < FilterList.Count; i++)                
                {
                    if (FilterList[i] == lstFilter.SelectedValue)
                    {
                        FilterList.RemoveAt(i);
                        break;                        
                    }
                }
            }
        }

        private void ContentTab_Loaded(object sender, RoutedEventArgs e)
        {
            FilterSetList = StorageCore.Core.GetFilterSets();
            ChangeContextRibbon(new List<string> { "ribTabFilter" }, null);
        }

        private void lstFilterOverview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Save the current FilterList
            foreach(var fS in FilterSetList)
            {
                if (fS.Id == _SelectedFilterSetId)
                {
                    fS.Filter = FilterList.ToList();
                    break;
                }
            }

            if (lstFilterOverview.SelectedValue != null)
            {
                FilterList = new ObservableCollection<FilterClass>(((FilterSet)lstFilterOverview.SelectedValue).Filter);
                _SelectedFilterSetId = (lstFilterOverview.SelectedValue as FilterSet).Id;
            }
        }

        private void btnSetAddNew_Click(object sender, RoutedEventArgs e)
        {
            Save();
            StorageCore.Core.AddFilter("New Filterset", new List<FilterClass>());
            FilterSetList = StorageCore.Core.GetFilterSets();
        }

        private void btnSetDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstFilterOverview.SelectedValue != null)
            {   
                StorageCore.Core.DeleteFilterSet((lstFilterOverview.SelectedValue as FilterSet).Id);
                List<FilterSet> tempFs = FilterSetList;

                tempFs.Remove((FilterSet)lstFilterOverview.SelectedValue);
                
                lstFilterOverview.SelectedValue = null;

                RaisePropertyChanged("FilterSetList");
                Save();
                FilterSetList = StorageCore.Core.GetFilterSets();
            }
        }

        private void btnSetCopy_Click(object sender, RoutedEventArgs e)
        {
            Save();
            StorageCore.Core.AddFilter((lstFilterOverview.SelectedValue as FilterSet).Title + "_Copy", (lstFilterOverview.SelectedValue as FilterSet).Filter);
            FilterSetList = StorageCore.Core.GetFilterSets();
        }

        private void cmbValue_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (lstFilter.SelectedValue != null && 
                lstFilter.SelectedValue.GetType() == typeof(FilterClass) && 
                cmbValue.Visibility == Visibility.Visible &&
                cmbValue.Items.Count > 0)
            {
                if (((FilterClass) lstFilter.SelectedValue).Value == null) 
                    return;

                var idToSelect = (long) ((FilterClass) lstFilter.SelectedValue).Value;
                cmbValue.SelectedValue = idToSelect;
            }
        }

        private void TabBase_Close(object sender, RoutedEventArgs e)
        {
            ChangeContextRibbon(null, new List<string> { "ribTabFilter" });
        }

        private void lstFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            switch (((FilterClass) e.AddedItems[0]).ConditionType)
            {
                case FilterType.Protocol:
                    //Show Combobox with Protocols
                    SelectedFilterIsProtocol = true;
                    break;
                case FilterType.Collection:
                    //Show Combobox with other Collections
                    SelectedFilterIsFilterSet = true;
                    break;
                case FilterType.Connection:
                    //Show Combobox with all connections
                    break;
                case FilterType.Credential:
                    //Show Combobox with all credentials
                    break;
                case FilterType.Folder:
                case FilterType.Description:
                case FilterType.Host:
                case FilterType.Name:
                case FilterType.Port:
                case FilterType.OperatingSystem:
                    //show textbox
                    SelectedFilterIsString = true;
                    break;
                case FilterType.ProtocolSetting:
                    break;
                case FilterType.Public:
                    //Show Checkbox
                    SelectedFilterIsBoolean = true;
                    break;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _Filter.Clear();
            _FilterSetList.Clear();
            _PreviewResult.Clear();
            _ComboBoxContent.Clear();
        }
    }


    #region Converter

    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class VisibilityToBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var vis = (Visibility)value;
            switch (vis)
            {
                case Visibility.Collapsed:
                    return false;
                case Visibility.Hidden:
                    return false;
                case Visibility.Visible:
                    return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value) return Visibility.Visible;
            
            return Visibility.Collapsed;
        }

        #endregion
    }

    [ValueConversion(typeof(Int64), typeof(Int64))]
    public class FilterSetIdValidatorConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (value is long || value is int || value is short || value is byte))
            {
                return (value);
            }
            
            return (null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value);
        }

        #endregion
    }

    [ValueConversion(typeof(FilterType), typeof(Visibility))]
    public class FilterTypeToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Hidden;

            var fType = (FilterType)value;
            var cType = parameter.ToString();

            switch (fType)
            {
                //Strings
                case FilterType.Name:
                case FilterType.Description:
                case FilterType.Host:
                case FilterType.Folder:
                case FilterType.Port:
                case FilterType.Credential:
                case FilterType.OperatingSystem:
                case FilterType.Protocol:
                case FilterType.ProtocolSetting:
                    switch (cType)
                    { 
                        case "textbox":
                            return Visibility.Visible;
                        case "filtersets":
                        case "checkbox":
                            return Visibility.Collapsed;                            
                    }
                    break;
                
                //Booleans
                case FilterType.Public:
                    switch (cType)
                    {
                        case "checkbox":
                            return Visibility.Visible;
                        case "textbox":
                        case "filtersets":
                            return Visibility.Collapsed;
                    }
                    break;
                
                //Connectionlist
                case FilterType.Connection:
                    switch (cType)
                    {
                        case "textbox":
                        case "filtersets":
                        case "checkbox":
                            return Visibility.Collapsed;
                    }
                    break;

                //Filtersets
                case FilterType.Collection:
                    switch (cType)
                    {
                        case "textbox":
                        case "checkbox":
                            return Visibility.Collapsed;
                        case "filtersets":
                            return Visibility.Visible;
                    }
                    break;


            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("Convert Back is not possible for FilterTypeToVisibilityConverter");
        }

        #endregion
    }

     [ValueConversion(typeof(FilterType), typeof(bool))]
    public class FilterTypeToBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            var fType = (FilterType)value;
            var cType = parameter.ToString();

            switch (fType)
            {
                case FilterType.Name:
                case FilterType.Description:
                case FilterType.Host:
                case FilterType.Folder:
                case FilterType.Port:
                    switch (cType)
                    { 
                        case "textbox":
                        case "allExCol":
                            return true;
                        case "collection":
                        case "checkbox":
                            return false;                            
                    }
                    break;

                case FilterType.Credential:
                case FilterType.OperatingSystem:
                case FilterType.Protocol:
                case FilterType.ProtocolSetting:
                    switch (cType)
                    {
                        case "collection":
                        case "allExCol":
                            return true;
                        case "textbox":                        
                        case "checkbox":
                            return false;                            
                    }
                    break;

                case FilterType.Public:
                    switch (cType)
                    {
                        case "checkbox":
                        case "allExCol":
                            return true;
                        case "textbox":
                        case "collection":
                            return false;
                    }
                    break;
                
                case FilterType.Connection:
                    switch (cType)
                    {
                        case "allExCol":
                            return true;
                        case "textbox":
                        case "collection":
                        case "checkbox":
                            return false;
                    }
                    break;

                case FilterType.Collection:
                    switch (cType)
                    {
                        case "textbox":
                        case "collection":
                        case "allExCol":
                        case "checkbox":
                            return false;
                    }
                    break;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("Convert Back is not possible for FilterTypeToBoolConverter");
        }

        #endregion
    }

     //[ValueConversion(typeof(FilterSet), typeof(long))]
     //public class FilterSetIdToFilterSetConverter : IValueConverter
     //{
     //    #region IValueConverter Members

     //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
     //    {
     //       if (value != null)
     //            return ((FilterSet)value).Id;
     //        else
     //            return (null);
     //    }

     //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
     //    {
             
     //        if (value == null)
     //            return (null);

     //        List<FilterSet> lstFs = StorageCore.Core.GetFilterSets();
     //        foreach (FilterSet fS in lstFs)
     //        {
     //            if (fS.Id == Int32.Parse(value.ToString()))
     //            {
     //                return (fS);
     //            }
     //        }
     //        return (null);
     //    }

     //    #endregion
     //}
    #endregion
}
