using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using System.Text.RegularExpressions;
using beRemote.Core.Common.LogSystem;

namespace beRemote.GUI.Controls
{
    /// <summary>
    /// Interaction logic for ProtocolOptionGrid.xaml
    /// </summary>
    public partial class ProtocolOptionGrid : UserControl
    {
        private SortedList<string, ProtocolSetting> _protocolOptions = new SortedList<string, ProtocolSetting>();
        private Dictionary<string, string> _textBoxRegEx = new Dictionary<string, string>(); //key = name of the Textbox, value = the Regex-String
        private List<string> _invalidValues = new List<string>(); //Contains the Invalid fields checked by Regex
        private string _NoDataMessage = "no data to display";
        private List<UserCredential> _credentials = new List<UserCredential>(); //Credentials for a user. Required, if the Credentials-Control is used.

        public ProtocolOptionGrid()
        {
            InitializeComponent();
            lblNoData.Content = _NoDataMessage;            
        }

        public ProtocolOptionGrid(SortedList<string, ProtocolSetting> protocolOptions)
        {
            InitializeComponent();
            _protocolOptions = protocolOptions;
            lblNoData.Content = _NoDataMessage;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetProtocolOptions(_protocolOptions);            
        }

        /// <summary>
        /// Sets the Credentials for Credential-Comboboxes
        /// </summary>
        public List<UserCredential> Credentials
        {
            set { _credentials = value; }
        }

        public void SetProtocolOptions(SortedList<string, ProtocolSetting> protocolOptions)
        {
            try
            {
                grdMain.Children.Clear();
                lblNoData.Visibility = Visibility.Hidden;

                _protocolOptions = protocolOptions;
                int propertycounter = 0;

                //Calculate the size of the Areas
                int controlWidth = 150;
                int lblWidth = (int)this.Width - controlWidth < 150 ? 150 : (int)this.Width - controlWidth-3;
                
                //Calculate the size of the Control
                this.Height = 24 * _protocolOptions.Values.Count +2;

                foreach (ProtocolSetting aSet in _protocolOptions.Values)
                {
                    try
                    {
                        #region Title
                        //Title-Label (1st Column)
                        Label lbl = new Label();
                        lbl.Content = aSet.GetTitle();
                        lbl.Margin = new Thickness(0, propertycounter * 24, 0, 0);
                        lbl.Width = lblWidth;
                        lbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        lbl.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        lbl.ToolTip = aSet.GetDescription();
                        grdMain.Children.Add(lbl);

                        //Rectangle for Title (1st Column)
                        Rectangle rec = new Rectangle();
                        rec.Margin = new Thickness(0, propertycounter * 24 - 1, 0, 0);
                        rec.Width = lblWidth + 1;
                        rec.Height = 25;
                        rec.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        rec.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        rec.Stroke = Brushes.Black;
                        grdMain.Children.Add(rec);

                        //Rectangle for Value (2nd Column)
                        Rectangle rec2 = new Rectangle();
                        rec2.Margin = new Thickness(lblWidth, propertycounter * 24 - 1, 0, 0);
                        rec2.Width = controlWidth + 2;
                        rec2.Height = 25;
                        rec2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        rec2.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        rec2.Stroke = Brushes.Black;
                        grdMain.Children.Add(rec2);
                        #endregion

                        if (aSet.GetDefinedValues() != null) //If there are predefined Values to select
                        {
                            #region ComboBox
                            ComboBox cmb = new ComboBox();
                            cmb.Tag = aSet.GetKey();
                            cmb.Text = aSet.GetProtocolSettingValue() != null ? aSet.GetProtocolSettingValue().GetValue().ToString() : "";
                            cmb.Margin = new Thickness(lblWidth + 1, propertycounter * 24, 0, 0);
                            cmb.Width = controlWidth;
                            cmb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            cmb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            cmb.SelectionChanged += cmb_SelectionChanged;

                            //Add the ComboboxItems
                            foreach (DefinedProtocolSettingValue dpsv in aSet.GetDefinedValues())
                            {
                                ComboBoxItem cmbi = new ComboBoxItem();
                                cmbi.Content = dpsv.GetTitle();
                                cmbi.Tag = dpsv.GetValue();

                                cmb.Items.Add(cmbi);
                            }

                            //Set the selected Item
                            if (aSet.GetProtocolSettingValue() != null)
                            {
                                foreach (ComboBoxItem cmbi in cmb.Items)
                                {
                                    if (cmbi.Tag.ToString() == aSet.GetProtocolSettingValue().GetValue().ToString())
                                    {
                                        cmb.SelectedItem = cmbi;
                                        break;
                                    }
                                }
                            }

                            grdMain.Children.Add(cmb);
                            
                            #endregion
                        }
                        else //No predefined values (= No Combobox)
                        {
                            if (aSet.GetDataType() == typeof(String)) //For Strings add a Textbox
                            {                                
                                #region TextBox
                                TextBox txt = new TextBox();
                                txt.Tag = aSet.GetKey();
                                txt.Text = aSet.GetProtocolSettingValue() != null ? aSet.GetProtocolSettingValue().GetValue().ToString() : "";
                                txt.Margin = new Thickness(lblWidth + 1, propertycounter * 24, 0, 0);
                                txt.Width = controlWidth;
                                txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                txt.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                                txt.LostFocus += new RoutedEventHandler(txt_LostFocus); //For RegEx Check

                                if (aSet.GetRegEx() != null && aSet.GetRegEx() != "")
                                    _textBoxRegEx.Add(aSet.GetKey(), aSet.GetRegEx());

                                grdMain.Children.Add(txt);
                                #endregion                                
                            }
                            else if (aSet.GetDataType() == typeof(UserCredential)) //For Credentials
                            {
                                #region Credentials
                                int credId = 0;
                                if (aSet.GetProtocolSettingValue() != null && aSet.GetProtocolSettingValue().GetValue().ToString() != "") //if there is a ID
                                {
                                    if (aSet.GetProtocolSettingValue().GetValue().GetType() == typeof(UserCredential))
                                        credId = ((UserCredential)aSet.GetProtocolSettingValue().GetValue()).Id; //Only the ID is required, the rest is fake.
                                }

                                ComboBox cmb = new ComboBox();
                                cmb.Tag = aSet.GetKey();
                                cmb.Name = "cred";
                                cmb.Margin = new Thickness(lblWidth + 1, propertycounter * 24, 0, 0);
                                cmb.Width = controlWidth;
                                cmb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                cmb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                                cmb.SelectionChanged += cmb_SelectionChanged;

                                //Set the CMB-Content
                                cmb.DisplayMemberPath = "Description";
                                cmb.SelectedValuePath = "Id";
                                cmb.ItemsSource = _credentials;

                                //Set the selected value
                                if (credId != 0) cmb.SelectedValue = credId;

                                grdMain.Children.Add(cmb);
                                #endregion                                
                            }
                            else if (aSet.GetDataType() == typeof(Boolean)) //For Bool add a Checkbox
                            {
                                #region CheckBox
                                CheckBox chk = new CheckBox();
                                chk.Tag = aSet.GetKey();
                                chk.IsChecked = aSet.GetProtocolSettingValue() != null ? Convert.ToBoolean(aSet.GetProtocolSettingValue().GetValue()) : false;
                                chk.Margin = new Thickness(lblWidth + 5, propertycounter * 24 + 4, 0, 0);
                                chk.Width = controlWidth;
                                chk.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                chk.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                                grdMain.Children.Add(chk);
                                #endregion
                            }
                            else if (aSet.GetDataType() == typeof(DateTime)) //For Datetime add a DateTimePicker
                            {
                                #region DateTimePicker
                                DatePicker dpi = new DatePicker();
                                dpi.Tag = aSet.GetKey();
                                dpi.SelectedDate = aSet.GetProtocolSettingValue() != null ? Convert.ToDateTime(aSet.GetProtocolSettingValue().GetValue()) : DateTime.Now;
                                dpi.Margin = new Thickness(lblWidth + 1, propertycounter * 24, 0, 0);
                                dpi.Width = controlWidth;
                                dpi.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                dpi.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                                grdMain.Children.Add(dpi);
                                #endregion
                            }
                            else if (aSet.GetDataType() == typeof(Int16) || //For a Number add a NumericUpDown                            
                                aSet.GetDataType() == typeof(System.Int32) ||
                                aSet.GetDataType() == typeof(Int64) ||
                                aSet.GetDataType() == typeof(UInt16) ||
                                aSet.GetDataType() == typeof(UInt32) ||
                                aSet.GetDataType() == typeof(UInt64) ||
                                aSet.GetDataType() == typeof(Byte))
                            {
                                #region NumericUpDown
                                NumericUpDown nud = new NumericUpDown();
                                nud.Tag = aSet.GetKey();
                                nud.Value = aSet.GetProtocolSettingValue() != null ? Convert.ToInt32(aSet.GetProtocolSettingValue().GetValue()) : 0;
                                nud.Margin = new Thickness(lblWidth + 1, propertycounter * 24, 0, 0);
                                nud.Width = controlWidth;
                                nud.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                nud.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                                nud.LostFocus += new RoutedEventHandler(txt_LostFocus);

                                if (aSet.GetDataType() == typeof(Int32))
                                    nud.Maximum = Int32.MaxValue;
                                else if (aSet.GetDataType() == typeof(Int64))
                                    nud.Maximum = Int32.MaxValue;
                                else if (aSet.GetDataType() == typeof(UInt32))
                                    nud.Maximum = Int32.MaxValue;
                                else if (aSet.GetDataType() == typeof(UInt64))
                                    nud.Maximum = Int32.MaxValue;
                                else if (aSet.GetDataType() == typeof(UInt16))
                                    nud.Maximum = UInt16.MaxValue;
                                else if (aSet.GetDataType() == typeof(Byte))
                                    nud.Maximum = Byte.MaxValue;


                                if (aSet.GetRegEx() != null && aSet.GetRegEx() != "")
                                    _textBoxRegEx.Add(aSet.GetKey(), aSet.GetRegEx());

                                grdMain.Children.Add(nud);
                                #endregion
                            }
                            else if (aSet.GetDataType() == typeof(Color)) //For a Color add a Colorpicker
                            {
                                //For future Releases
                            }
                        }

                        propertycounter++;
                    }
                    catch (Exception ex)
                    {
                        String msg = String.Format("Problem generating UI Item for protocol option. (Protocol option: {0}; description: {1})", aSet.GetTitle(), aSet.GetDescription());
                        Logger.Log(LogEntryType.Exception, msg,ex, "GUI");
                        MessageBox.Show(String.Format("{0}\r\n\r\n{1}", msg, "WARNING: Adding the connection anyways may result in a broken connection in database. Consider to contact the support!"));
                    }

                }
            }
            catch (Exception ea)
            {
                MessageBox.Show(ea.ToString());
            }
        }


        void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }


        /// <summary>
        /// Occurs when a RegEx-Checked Control lost its focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = "";
            string key = "";            

            if (sender.GetType() == typeof(TextBox))
            {
                text = ((TextBox)sender).Text;
                key = ((TextBox)sender).Tag.ToString();
                
            }
            else if (sender.GetType() == typeof(NumericUpDown))
            {
                text = ((NumericUpDown)sender).Value.ToString();
                key = ((NumericUpDown)sender).Tag.ToString();
                
            }

            if (_textBoxRegEx.ContainsKey(key))
            {
                Regex r = new Regex(_textBoxRegEx[key]);
                Match m = r.Match(text);

                if (m.Success == false)
                {
                    if (!_invalidValues.Contains(key))
                    {
                        _invalidValues.Add(key);

                        if (sender.GetType() == typeof(TextBox))
                            ((TextBox)sender).Background = new SolidColorBrush(Colors.LightCoral);
                        else if (sender.GetType() == typeof(NumericUpDown))
                            ((NumericUpDown)sender).Background = new SolidColorBrush(Colors.LightCoral);
                    }
                }
                else //RegEx Matches
                {
                    if (_invalidValues.Contains(key))
                    {
                        _invalidValues.Remove(key);

                        if (sender.GetType() == typeof(TextBox))
                            ((TextBox)sender).Background = new SolidColorBrush(Colors.White);
                        else if (sender.GetType() == typeof(NumericUpDown))
                            ((NumericUpDown)sender).Background = new SolidColorBrush(Colors.White);
                    }
                }
            }
        }


        /// <summary>
        /// Get all Options and its Settings
        /// </summary>
        /// <returns>Null if invalid values are existing</returns>
        public Dictionary<string, object> GetProtocolOptions()
        {
            this.Focus(); //to unfocus selected field and validate it

            if (_invalidValues.Count > 0)
            {
                MessageBox.Show("You have entered some invalid values. Please check them.", "Invalid values", MessageBoxButton.OK, MessageBoxImage.Error);
                return (null);
            }
            else
            {
                Dictionary<string, object> ret = new Dictionary<string, object>();

                foreach (ProtocolSetting ps in _protocolOptions.Values)
                {
                    foreach (UIElement elem in grdMain.Children)
                    {
                        if (elem.GetType() != typeof(Rectangle))
                        {
                            Control contr = (Control)elem;

                            if (contr.Tag == null)
                                continue;

                            if (ps.GetKey() == contr.Tag.ToString())
                            {
                                if (contr.GetType() == typeof(TextBox)) //Strings
                                    ret.Add(contr.Tag.ToString(), ((TextBox)contr).Text);
                                else if (contr.GetType() == typeof(NumericUpDown)) //INteger
                                    ret.Add(contr.Tag.ToString(), ((NumericUpDown)contr).Value);
                                else if (contr.GetType() == typeof(CheckBox)) //Boolean
                                    ret.Add(contr.Tag.ToString(), ((CheckBox)contr).IsChecked.Value);
                                else if (contr.GetType() == typeof(DatePicker)) //DateTime
                                    ret.Add(contr.Tag.ToString(), ((DatePicker)contr).SelectedDate);
                                else if (contr.GetType() == typeof(ComboBox)) //Object
                                {
                                    if (contr.Name == "cred") //For Credentials
                                    {
                                        //If no credential is selected, return 0
                                        if (((ComboBox)contr).SelectedValue == null)
                                            ret.Add(contr.Tag.ToString(), 0);
                                        else
                                            ret.Add(contr.Tag.ToString(), ((ComboBox)contr).SelectedValue);
                                    }
                                    else
                                    {
                                        ret.Add(contr.Tag.ToString(), ((ComboBoxItem)((ComboBox)contr).SelectedItem).Tag);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }

                return (ret);
            }
        }

        public void SetSingleValue(string key, object value)
        {
            try
            {
                foreach (UIElement elem in grdMain.Children)
                {
                    if (elem.GetType() != typeof(Rectangle))
                    {
                        Control contr = (Control)elem;

                        if (contr.Tag == null)
                            continue;

                        if (key == contr.Tag.ToString())
                        {
                            if (contr.GetType() == typeof(TextBox)) //Strings
                                ((TextBox)contr).Text = value.ToString();
                            else if (contr.GetType() == typeof(NumericUpDown)) //Integer
                                ((NumericUpDown)contr).Value = Convert.ToInt32(value);
                            else if (contr.GetType() == typeof(CheckBox)) //Boolean
                                ((CheckBox)contr).IsChecked = Convert.ToBoolean(value);
                            else if (contr.GetType() == typeof(DatePicker)) //DateTime
                                ((DatePicker)contr).SelectedDate = Convert.ToDateTime(value);
                            else if (contr.GetType() == typeof(ComboBox)) //Object
                            {
                                //((ComboBox)contr).SelectedValue = value;

                                for (int i = 0; i < ((ComboBox)contr).Items.Count; i++)
                                {
                                    if (((ComboBoxItem)((ComboBox)contr).Items[i]).Tag.ToString() == value.ToString())
                                    {
                                        ((ComboBox)contr).SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ea)
            {
                Logger.Log(LogEntryType.Warning, "Value \"" + key + "\" could not be set to \"" + value.ToString() + "\".", ea);
            }
        }

        public string NoDataMessage
        {
            get { return _NoDataMessage; }
            set { _NoDataMessage = value; }
        }
    }
}

