using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using beRemote.Core.StorageSystem.StorageBase;
using System.ComponentModel;

namespace beRemote.GUI.Tabs.SuperAdminTools
{
    /// <summary>
    /// Interaction logic for ContentTabAbout.xaml
    /// </summary>
    public partial class TabSuperAdminTools : INotifyPropertyChanged
    {
        public TabSuperAdminTools()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Load Userlist
            Dictionary<int, string> userList = StorageCore.Core.GetUserList();

            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(System.Int32));
            dt.Columns.Add("name", typeof(System.String));

            foreach(KeyValuePair<int, string> kvp in userList)
            {
                DataRow dR = dt.NewRow();
                dR[0] = kvp.Key;
                dR[1] = kvp.Value;
                dt.Rows.Add(dR);
            }
            
            nudHeartbeat.Value = Convert.ToInt32(StorageCore.Core.GetSetting("heartbeat"));
            chkMaintmode.IsChecked = (StorageCore.Core.GetSetting("maintmode") == "1" ? true : false);

            if (StorageCore.Core.GetSetting("ribbonimageopacity") != "") //older Version compability (available since 0.0.3)
                RibbonImageOpacity = Convert.ToByte(StorageCore.Core.GetSetting("ribbonimageopacity"));            
        }
        
        private void btnSaveDatabase_Click(object sender, RoutedEventArgs e)
        {
            StorageCore.Core.SetSetting("heartbeat", nudHeartbeat.Value.ToString());
            StorageCore.Core.SetSetting("maintmode", (chkMaintmode.IsChecked == true?"1": "0"));
            StorageCore.Core.SetSetting("useraccountcreation", (chkDisableAccountCreation.IsChecked == true ? "0" : "1"));
            StorageCore.Core.SetSetting("ribbonimageopacity", RibbonImageOpacity.ToString());
        }

        private void chkMaintmode_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Attention!!! After saving, the maintenance mode will be activated IMMEDIATE! " + System.Environment.NewLine +
                "Even YOU and ALL superadmins were forced to logoff!" + System.Environment.NewLine +
                "If you don't want this, disable the maintenance mode before saving!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void btnRibbonImagePath_Click(object sender, RoutedEventArgs e)
        {
            //Select pngfile
            Microsoft.Win32.OpenFileDialog oFD = new Microsoft.Win32.OpenFileDialog();
            oFD.Filter = "PNG-Image|*.png";            
            oFD.ShowDialog();

            //If a File is selected
            if (oFD.FileName != "")
            {
                //Set the txtfield
                txtRibbonImagePath.Text = oFD.FileName;
                if (System.IO.File.Exists(oFD.FileName))
                {
                    //Load the Image for Preview-Box
                    BitmapImage bI = new BitmapImage();
                    bI.BeginInit();
                    bI.UriSource = new Uri(oFD.FileName, UriKind.RelativeOrAbsolute);
                    bI.EndInit();

                    //Check if height is to large
                    if (Convert.ToInt32(bI.Height) <= 85)
                    {
                        imgRibbonImagePath.Stretch = Stretch.None;
                        imgRibbonImagePath.Source = bI;
                        
                        //Write the Image to database and get Id
                        int ribImgId = Convert.ToInt32(StorageCore.Core.GetSetting("ribbonimg"));

                        //Convert the image to byte
                        byte[] data = BitmapImageToByte(bI);                        

                        //If no image was applied previously
                        if (ribImgId == 0)
                        {
                            //Add Data-Entry
                            long newId = StorageCore.Core.AddData(data);
                            StorageCore.Core.SetSetting("ribbonimg", newId.ToString());
                        }
                        else
                        {
                            //Modify Data-Entry
                            StorageCore.Core.SetData(data, ribImgId);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Your image is too large. Please reduce the height. The height is " + Convert.ToInt32(bI.Height).ToString(), "Cannot use image", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
        }


        #region Properties
        private byte _RibbonImageOpacity = 70;

        public byte RibbonImageOpacity
        {
            get { return _RibbonImageOpacity; }
            set 
            { 
                _RibbonImageOpacity = value;
                RaisePropertyChanged("RibbonImageOpacity");
            }
        }
        #endregion

        #region BitmapImage Conversion
        /// <summary>
        /// Converts a BitmapImage with an UriSource to Byte[]
        /// </summary>
        /// <param name="bI"></param>
        /// <returns></returns>
        public byte[] BitmapImageToByte(BitmapImage bI)
        {
            try
            {
                FileStream fS = new FileStream(bI.UriSource.AbsolutePath, FileMode.Open, FileAccess.Read);
                BinaryReader bR = new BinaryReader(fS);

                byte[] ret = bR.ReadBytes((Int32)new FileInfo(bI.UriSource.AbsolutePath).Length);

                bR.Close();
                bR.Dispose();
                fS.Close();
                fS.Dispose();

                return (ret);
            }
            catch (Exception)
            {                 
                return(new byte[0]);
            }
        }
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; //To Update Content on the Form

        /// <summary>
        /// Helper for Triggering PropertyChanged
        /// </summary>
        /// <param name="triggerControl">The Name of the Property to update</param>
        private void RaisePropertyChanged(string triggerControl)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(triggerControl));
            }
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
