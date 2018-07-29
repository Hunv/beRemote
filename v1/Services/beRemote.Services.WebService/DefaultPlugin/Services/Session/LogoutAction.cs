using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using beRemote.Services.ServiceLib.Classes.ServicePlugin;

namespace beRemote.Services.WebService.DefaultPlugin.Services.Session
{
    [ListenerActionAttribute(
        ActionName = "Logout",
        Description = "Performs session logout")]
    public class LogoutAction : AbstractListenerAction
    {
        public LogoutAction(AbstractListener parentListener) : base(parentListener)
        {
        }

        public override void ExecuteAction(ExecutionContext context)
        {
            context.Session.Abandon();
            context.AddCookieToResponse("ASP.NET_SessionId", "");
        }


    }
}