using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace beRemote.Services.FTClasses
{
    public class DBTools
    {
        public static DataRow GetUserInformation(MySqlConnection conn, String username)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM users WHERE name = @username";
            cmd.Parameters.Add("@username", username.ToUpper());

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataSet dataset = new DataSet();
            adapter.Fill(dataset);

            if (dataset.Tables[0].Rows.Count == 1)
            {
                DateTime expires = DateTime.Parse(dataset.Tables[0].Rows[0]["expires"].ToString());
                if (DateTime.Compare(DateTime.Now, expires) > 0)
                {
                    DeleteUserInformation(conn, username);
                    return null;
                }
                else
                    return dataset.Tables[0].Rows[0];
            }

            return null;
            
        }

        public static void DeleteUserInformation(MySqlConnection conn, String username)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM users WHERE name =  @name";
            cmd.Parameters.Add("@name", username);

            cmd.ExecuteNonQuery();
        }

        public static void AddUserInformation(MySqlConnection conn, FTClasses.User user)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            DeleteUserInformation(conn, user.UserName.ToUpper());

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO users (name, token, expires, clientip, clientbuild) VALUES (@name, @token, @expires, @clientip, @clientbuild)";
            cmd.Parameters.Add("@name", user.UserName.ToUpper());
            cmd.Parameters.Add("@token", user.Password.Substring(0,8));
            cmd.Parameters.Add("@expires", DateTime.Now.AddMinutes(5));
            cmd.Parameters.Add("@clientip", user.ClientIP);
            cmd.Parameters.Add("@clientbuild", user.BuildInfo);

            cmd.ExecuteNonQuery();


        }
    }
}
