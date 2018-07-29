using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.IO;

namespace beRemote.Services.FTPUserService
{
    public static class Tools
    {
        private static MySqlConnection _conn;

        public static void AddUser(FTClasses.User user)
        {
            if (_conn == null)
                InitConn();

            FTClasses.DBTools.AddUserInformation(_conn, user);
        }

        private static void InitConn()
        {
            TextReader tr = (TextReader)new StreamReader(Properties.Settings.Default.ConStrPath);
            String val = tr.ReadToEnd();
            tr.Close();
            tr.Dispose();
            _conn = new MySqlConnection(val);
            try
            {
                _conn.Open();
                _conn.Close();
            }
            catch(Exception ex)
            {
                
            }
        }
    }
}