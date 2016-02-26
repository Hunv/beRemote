using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Channels;

namespace beRemote.Services.FTPUserService
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Klassennamen "Service1" sowohl im Code als auch in der SVC- und der Konfigurationsdatei ändern.
    public class Service : IService
    {

        public string InitiateAuthentication(string dbid, string build)
        {
            String user = dbid;
            String clientIP = GetClientIP();
            String token = Guid.NewGuid().ToString();

            Tools.AddUser(FTClasses.User.NewUserObject(user, token, clientIP, build));

            return token;
        }

        private string GetClientIP()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties messageProperties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpointProperty =
                messageProperties[RemoteEndpointMessageProperty.Name]
                as RemoteEndpointMessageProperty;

            return endpointProperty.Address;
        }
    }
}
