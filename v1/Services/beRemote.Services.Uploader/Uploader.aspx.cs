using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.IO;
using System.Data;

namespace beRemote.Services.Uploader
{
    public partial class Uploader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Context.Request.QueryString.AllKeys.Contains("db"))
                {
                    divContent.InnerHtml = "Error [-1]";
                    return;
                }

                //Logger.InitiateLogger(new IniFile(Properties.Settings.Default.LoggerConfig));

                //Logger.Log(LogEntryType.Info, "Initiated new uploader session");
                //Logger.Log(LogEntryType.Info, String.Format("[DB_ID]: {0}", Context.Request.QueryString["id"]));
                //Logger.Log(LogEntryType.Info, String.Format("[TOKEN]: {0}", Context.Request.QueryString["token"]));

                //Logger.Log(LogEntryType.Verbose, "Reading connection string from configuration");
                TextReader tr = (TextReader)new StreamReader(Properties.Settings.Default.ConStrPath);
                String val = tr.ReadToEnd();
                tr.Close();
                tr.Dispose();
                //Logger.Log(LogEntryType.Verbose, "Creating database connection");
                MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(val);
                //Logger.Log(LogEntryType.Verbose, "Retrieving user information");
                DataRow dr = beRemote.Services.FTClasses.DBTools.GetUserInformation(con, Context.Request.QueryString["db"]);

                if (dr != null)
                {

                    //  Logger.Log(LogEntryType.Verbose, "Verifying client information");

                    bool stop = false;

                    if (Context.Request.UserHostAddress.ToUpper() != dr["clientip"].ToString().ToUpper())
                    {
                        divContent.InnerHtml = "Error [0]";
                        //Logger.Log(LogEntryType.Exception, "Error [0]: Client ip mismatch");
                        stop = true;
                    }

                    if (Context.Request.UserAgent.ToUpper() != dr["clientbuild"].ToString().ToUpper())
                    {
                        divContent.InnerHtml = "Error [1]";
                        //      Logger.Log(LogEntryType.Exception, "Error [1]: Client build mismatch");
                        stop = true;
                    }

                    if (Context.Request.QueryString["token"].ToUpper().Substring(0, 8) != dr["token"].ToString().ToUpper())
                    {
                        divContent.InnerHtml = "Error [2]";
                        //   Logger.Log(LogEntryType.Exception, "Error [2]: Datbase token mismatch");
                        stop = true;
                    }

                    if (!stop)
                    {
                        //    Logger.Log(LogEntryType.Info, "Working on uploads (" + Request.Files.Count.ToString() + ")");

                        HttpFileCollection uploadFiles = Request.Files;
                        try
                        {
                            // Build HTML listing the files received.
                            string summary = "<p>Files Uploaded:</p><ol>";

                            // Loop over the uploaded files and save to disk.
                            int i;
                            for (i = 0; i < uploadFiles.Count; i++)
                            {
                                HttpPostedFile postedFile = uploadFiles[i];

                                // Access the uploaded file's content in-memory:
                                System.IO.Stream inStream = postedFile.InputStream;
                                byte[] fileData = new byte[postedFile.ContentLength];
                                inStream.Read(fileData, 0, postedFile.ContentLength);

                                String uplpath = Properties.Settings.Default.UplPath + "\\" + dr["name"].ToString() + "\\" + dr["token"].ToString();

                                // Save the posted file in our "data" virtual directory.
                                //postedFile.SaveAs( Server.MapPath("data") + "\\" + Guid.NewGuid().ToString().Substring(0,8) +  "_"  + postedFile.FileName);

                                if (!Directory.Exists(uplpath))
                                    Directory.CreateDirectory(uplpath);

                                postedFile.SaveAs(uplpath + "\\" + postedFile.FileName);

                                // Also, get the file size and filename (as specified in
                                // the HTML form) for each file:
                                summary += "<li>" + postedFile.FileName + ": "
                                    + postedFile.ContentLength.ToString() + " bytes</li>";
                            }
                            summary += "</ol>";

                            // If there are any form variables, get them here:
                            summary += "<p>Form Variables:</p><ol>";

                            //Load Form variables into NameValueCollection variable.
                            NameValueCollection coll = Request.Form;

                            // Get names of all forms into a string array.
                            String[] arr1 = coll.AllKeys;
                            for (i = 0; i < arr1.Length; i++)
                            {
                                summary += "<li>" + arr1[i] + "</li>";
                            }
                            summary += "</ol>";
                            divContent.InnerHtml = summary;

                        }
                        catch (Exception ex)
                        {
                            divContent.InnerHtml = "Error [3]";
                            //  Logger.Log(LogEntryType.Exception, "Error [3]: Error while uploading");
                            //         Logger.Log(LogEntryType.Exception, "Error [3]: " + ex.ToString());
                            beRemote.Services.FTClasses.DBTools.DeleteUserInformation(con, Context.Request.QueryString["db"]);
                        }
                    }
                }
                else
                {
                    // no user info
                    //     Logger.Log(LogEntryType.Warning, "No db found");
                    divContent.InnerHtml = "No db found";
                }

                beRemote.Services.FTClasses.DBTools.DeleteUserInformation(con, Context.Request.QueryString["db"]);
            }
            catch (Exception ex)
            {
                divContent.InnerHtml = ex.ToString();
                //       Logger.Log(LogEntryType.Exception, ex.ToString());
            }
            finally
            {
                //         Logger.StopLogger(true);
            }
            
            
            
        }
    }
}