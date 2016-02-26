using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using beRemote.Services.ServiceLib.Classes.ServicePlugin;
using beRemote.Services.WebService.DefaultPlugin.Services.Session;

namespace beRemote.Services.WebService.DefaultPlugin.Services
{
    [ListenerMetadata(
        Id = "B11B28FD-33DC-4EF4-98C9-4250C8306477",
        Listener = "Session",
        Name="Session manager for the beremote service")]
    public class SessionService : AbstractListener
    {
       
        private LogoutAction _logoutAction;

        public SessionService(AbstractServicePlugin plugin) : base(plugin) 
        {
            _logoutAction = new LogoutAction(this);

            ListenerActions.Add(_logoutAction);

            
        }


    }
}

