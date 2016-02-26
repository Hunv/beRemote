using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using beRemote.Services.ServiceLib.Classes.Clients;

namespace beRemote.Services.ServiceLib.Classes.ServicePlugin
{
    public class ExecutionContext
    {
        private HttpContext _httpContext;
        private ClientType _clientType;
        public Version ClientVersion = new Version(0,0,0,0);
        public HttpSessionState Session {get { return _httpContext.Session; }}

        public ExecutionContext(HttpContext httpContext)
        {
            _httpContext = httpContext;

            if(httpContext.Session[Key.ClientHeader] == null)
                throw new Exception("Client header information is missing. Aborting.");

            _clientType = (ClientType) (_httpContext.Session[Key.ClientHeader]);

            int major = 0;
            int minor = 0;
            int build = 0;
            int revision = 0;

            if (RequestParameters.ContainsKey("VersionMajor"))
                major = Int32.Parse(RequestParameters["VersionMajor"]);
            if (RequestParameters.ContainsKey("VersionMinor"))
                minor = Int32.Parse(RequestParameters["VersionMinor"]);
            if (RequestParameters.ContainsKey("VersionBuild"))
                build = Int32.Parse(RequestParameters["VersionBuild"]);
            if (RequestParameters.ContainsKey("VersionRevision"))
                revision = Int32.Parse(RequestParameters["VersionRevision"]);

            ClientVersion = new Version(major, minor, build, revision);
        }

        private Dictionary<String, String> _requestParameters;
        public Dictionary<String, String> RequestParameters
        {
            get
            {
                if (_requestParameters == null)
                {
                    _requestParameters = new Dictionary<String, String>();
                    foreach (var key in _httpContext.Request.QueryString.AllKeys)
                    {
                        _requestParameters.Add(key, _httpContext.Request.QueryString[key]);
                    }
                }
                return _requestParameters;
            }
        }

        public void WriteJsonResponse(String json)
        {
            // ensure encryption
            if (_clientType.Encrypted)
            {
                _httpContext.Response.Write(json);
            }
            else
            {
                _httpContext.Response.Write(json);
            }
        }

        public void WriteFileResponse(FileInfo targetFile)
        {
            // ensure encryption
            if (_clientType.Encrypted)
            {
                _httpContext.Response.WriteFile(targetFile.FullName);
            }
            else
            {
                _httpContext.Response.WriteFile(targetFile.FullName);
            }
        }

        public void AddCookieToResponse(string key, string value)
        {
            _httpContext.Response.Cookies.Add(new HttpCookie(key, value));
        }
    }
}
