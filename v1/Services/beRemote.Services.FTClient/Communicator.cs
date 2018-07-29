using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace beRemote.Services.FTClient
{
    public class Communicator
    {
        private ServiceReference.ServiceClient client;
        private String token;
        private String user;

        private string build;

        public Communicator(String dbid, String build)
        {
            StartCommunication(dbid, build);
        }

        private void StartCommunication(String dbid, String build)
        {
            client = new ServiceReference.ServiceClient();
            token = client.InitiateAuthentication(dbid, build);
            user = dbid;
            this.build = build;
        }

        public String GetToken()
        {
            return token;
        }

        public bool Upload(String file)
        {
            // TODO: fix this shit!
            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", this.build );
            byte[] resp = wc.UploadFile(String.Format("http://svc.beremote.net/beRemote/services/FTUL/Uploader.aspx?db={0}&token={1}", user, token), file);
            string s = wc.Encoding.GetString(resp);
            return true;
        }
    }
}
