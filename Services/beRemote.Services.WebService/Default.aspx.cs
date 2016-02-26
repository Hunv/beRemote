using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using beRemote.Services.ServiceLib.Classes;
using beRemote.Services.ServiceLib.Classes.ServicePlugin;
using beRemote.Services.WebService.Authentication;

namespace beRemote.Services.WebService
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.Session.IsNewSession)
            {
                var sessionStorage = SessionStorage.DoAuthentication(Context);
                if (null == sessionStorage)
                {
                    SessionStorage.InvalidateSession(Context);
                }
                else
                {
                    // TODO: Todo
                    // Authenticated. start doing after auth, e.g. service init
                    var mefLoader = new MefLoader();
                    mefLoader.LoadServicePlugins(new DirectoryInfo(HttpRuntime.AppDomainAppPath + "\\ServicePlugins"), Context);

                    sessionStorage.MefLoader = mefLoader;
                }
            }
            else
            {
                if (Context.Session[Key.SessionAuthenticator] != null)
                {
                    var sessionStorage = (SessionStorage)Context.Session[Key.SessionAuthenticator];
                    if (sessionStorage.IsCurrentClientTypeValid(Context))
                    {
                        String listenerName = Context.Request.QueryString[Key.QueryStringListener];
                        String actionName = Context.Request.QueryString[Key.QueryStringAction];

                        if (listenerName != null)
                        {
                            if (sessionStorage.MefLoader.HandlesListener(listenerName))
                            {
                                if (actionName != null)
                                {
                                    var service = sessionStorage.MefLoader.GetPluginThatHandlesListener(listenerName);

                                    AbstractListener listener = service.GetListener(listenerName);

                                    if (listener.HandlesAction(actionName))
                                    {
                                        var ctx = new ExecutionContext(Context);
                                        try
                                        {
                                            listener.Execute(actionName, ctx);
                                        }
                                        catch (Exception ex)
                                        {
                                            var svc_exc = new ServiceException
                                            {
                                                Message = "Error while executing request",
                                                Listener = listenerName,
                                                Action = actionName,
                                                RequestParameter = ctx.RequestParameters,
                                                Exception = ex
                                            };
                                            ctx.WriteJsonResponse(svc_exc.Serialize());
                                        }
                                        
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        SessionStorage.InvalidateSession(Context);
                    }
                }
                else
                {
                    SessionStorage.InvalidateSession(Context);
                }

            }
        }
    }
}