using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using MySql.Data.MySqlClient;

namespace beRemote.Services.FTClasses
{
    public class User
    {
        public bool CanDeleteFiles, CanDeleteFolders, CanRenameFiles,
            CanRenameFolders, CanStoreFiles, CanStoreFolder, CanViewHiddenFiles,
            CanViewHiddenFolders, CanCopyFiles;

        public string UserName = "";
        public string StartUpDirectory = "";
        public string CurrentWorkingDirectory = "\\";
        public bool IsAuthenticated = false;
        public string Password;
        public String ClientIP;
        public String BuildInfo;

        public void LoadProfile(MySqlConnection conn, string p_UserName, String clientIP)
        {
            try
            {
                if (p_UserName == this.UserName) return;
                if ((this.UserName = p_UserName).Length == 0) return;

                String username = p_UserName.Split(new String[] { ";" }, StringSplitOptions.None)[0];
                String clientip = clientIP;
                String buildinfo = p_UserName.Split(new String[] { ";" }, StringSplitOptions.None)[1];

                IsAuthenticated = false;

                DataRow dr = FTClasses.DBTools.GetUserInformation(conn, username);
                                
                ClientIP = dr["clientip"].ToString();
                BuildInfo = dr["clientbuild"].ToString();

                if (ClientIP != clientip)
                    return;
                if (BuildInfo.ToUpper() != buildinfo.ToUpper())
                    return;

                Password = dr["token"].ToString();

                this.UserName = username;

                //Password = User.Attributes[1].Value;
                //StartUpDirectory = User.Attributes[2].Value;

                //char[] Permissions = User.Attributes[3].Value.ToCharArray();

                //CanStoreFiles = Permissions[0] == '1';
                //CanStoreFolder = Permissions[1] == '1';
                //CanRenameFiles = Permissions[2] == '1';
                //CanRenameFolders = Permissions[3] == '1';
                //CanDeleteFiles = Permissions[4] == '1';
                //CanDeleteFolders = Permissions[5] == '1';
                //CanCopyFiles = Permissions[6] == '1';                    
                //CanViewHiddenFiles = Permissions[7] == '1';
                //CanViewHiddenFolders = Permissions[8] == '1';

                CanStoreFiles = true;
                CanStoreFolder = false;
                CanRenameFiles = false;
                CanRenameFolders = false;
                CanDeleteFiles = false;
                CanDeleteFolders = false;
                CanCopyFiles = false;
                CanViewHiddenFiles = false;
                CanViewHiddenFolders = false;

            }
            catch (Exception Ex)
            {
               
            }
        }

        public static User NewUserObject(String username, String password, String clientIP, String build)
        {
            User usr = new User();
            usr.UserName = username.ToUpper();
            usr.Password = password;
            usr.StartUpDirectory = username + "_" + password;
            usr.ClientIP = clientIP;
            usr.BuildInfo = build;

            return usr;
        }

        public bool Authenticate(string Password)
        {
            if (Password.Substring(0, 8) == this.Password) IsAuthenticated = true;
            else IsAuthenticated = false;
            return IsAuthenticated;
        }

        public bool ChangeDirectory(string Dir)
        {
            //CurrentWorkingDirectory = Dir;
            //return true;
            return false;
        }
    }
}
