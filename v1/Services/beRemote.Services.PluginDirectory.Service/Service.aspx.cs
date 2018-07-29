using System;
using beRemote.Services.PluginDirectory.Library.Objects;
using beRemote.Services.PluginDirectory.Service.Database;
using Newtonsoft.Json;

namespace beRemote.Services.PluginDirectory.Service
{
    public partial class Service : System.Web.UI.Page
    {
        private String _action = "get";
        private String _type = "plugins";

        private bool _joinObjects;

        private bool _abort = false;
        private ServiceException _svcExc;
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessQueryString();

            if (_abort)
            {
                Response.Write(JsonConvert.SerializeObject(_svcExc));
            }
            else
            {
                switch (_action.ToLower())
                {
                    case "get":

                        using (var client = new Client())
                        {
                            client.Open();
                            switch (_type.ToLower())
                            {
                                case "authors":
                                    Response.Write(
                                        JsonConvert.SerializeObject(client.GetAllAuthors()));
                                    break;
                                case "plugins":
                                    if (_joinObjects)
                                    {
                                        try
                                        {
                                            var pluginList = client.GetAllPlugins();
                                            foreach (var plugin in pluginList)
                                            {
                                                plugin.PluginType = client.GetPluginType(plugin.TypeId);
                                                plugin.Author = client.GetAuthor(plugin.AuthorId);
                                                plugin.PluginVersion = client.GetVersion(plugin.VersionId);
                                                plugin.RequiredBeRemoteVersion =
                                                    client.GetVersion(plugin.RequiredBeRemoteVersionId);

                                                plugin.Groups = client.GetPluginGroups(plugin.Id);
                                                plugin.SearchTerms = client.GetPluginSearchTerms(plugin.Id);

                                                
                                            }

                                            Response.Write(JsonConvert.SerializeObject(pluginList));
                                        }
                                        catch (Exception ex)
                                        {
                                            Response.Write(new ServiceException { Message = ex.ToString() }.Serialize());
                                        }
                                    }
                                    else
                                    {
                                        Response.Write(
                                            JsonConvert.SerializeObject(client.GetAllPlugins()));
                                    }
                                    break;
                            }

                            client.Close();
                        }

                        break;
                    case "last_change_date":
                        using (Client client = new Client())
                        {
                            client.Open();

                            Response.Write(client.GetDbConfigValue("last_change_date"));

                            client.Close();
                        }
                        break;
                    case "last_change_table":
                        using (Client client = new Client())
                        {
                            client.Open();

                            Response.Write(client.GetDbConfigValue("last_change_table"));

                            client.Close();
                        }
                        break;
                    default:
                        Response.Write(new ServiceException { Message = "Unknown action parameter '" + _action + "'" }.Serialize());
                        break;
                }
            }
            //using (var client = new Client())
            //{
            //    var t = client.GetAllAuthors();
            //}
        }


        private void ProcessQueryString()
        {
            try
            {
                if(Request.QueryString["action"] != null)
                    _action = Request.QueryString["action"];
            }
            catch 
            {
                //Response.Write(.Serialize());
                _abort = true;
                _svcExc = new ServiceException { Message = "Missing request parameter", Params = new[] { "action" } };
                return;
            }

            try
            {
                if (Request.QueryString["type"] != null)
                    _type = Request.QueryString["type"];
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
                _abort = true;
                _svcExc = new ServiceException { Message = "Missing request parameter", Params = new[] { "type" } };
                return;
            }

            try
            {
                if (Request.QueryString["join"].ToLower().Equals("true"))
                {
                    _joinObjects = true;
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {

            }
        }
    }
}