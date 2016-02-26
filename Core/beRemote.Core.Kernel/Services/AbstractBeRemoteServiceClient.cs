using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using beRemote.Core.Common.Helper;

namespace beRemote.Core.Services
{
    public abstract class AbstractBeRemoteServiceClient : IDisposable
    {
        public delegate void ServiceClientRequestStarted(AbstractBeRemoteServiceClient sender);
        public static event ServiceClientRequestStarted OnRequestStarted;

        public delegate void ServiceClientRequestFinished(AbstractBeRemoteServiceClient sender);
        public static event ServiceClientRequestFinished OnRequestFinished;


        private Guid _clientSecret = Guid.NewGuid();
        private readonly CookieAwareWebClient _webClient;
        /// <summary>
        /// Contains a list of availale ServicePlugins presented by the response headers
        /// </summary>
        public List<String> AvailableServices = new List<string>();

        public abstract String ValidClientHash { get; }

        public WebProxy Proxy { get; set; }

        private Version _clientVersion;
        public Version ClientVersion
        {
            get
            {
                if(_clientVersion == null)
                    _clientVersion = Assembly.GetExecutingAssembly().GetName().Version;
                return _clientVersion;
            }
        }

        public int Major{ get { return ClientVersion.Major; } }
        public int Minor { get { return ClientVersion.Minor; } }
        public int Build { get { return ClientVersion.Build; } }
        public int Revision { get { return ClientVersion.Revision; } }

        private Dictionary<String, String> parameters = new Dictionary<string, string>();

        public String ClientChannel { get; private set; }

        public Boolean ProxyActive { get; set; }
        /// <summary>
        /// Initialiues a new client to the specified server
        /// </summary>
        /// <param name="baseUrl">The base URL e.g.: http://localhost:63092/Default.aspx</param>
        public AbstractBeRemoteServiceClient(String baseUrl)
        {
            String urlToUse = baseUrl;

            if (urlToUse.EndsWith("/"))
                urlToUse = urlToUse.Remove(urlToUse.Length - 1, 1);

            _webClient = new CookieAwareWebClient { BaseAddress = urlToUse };

            var p_cfg = Kernel.GetUserProxySettings();

            if (p_cfg != null)
            {
                ProxyActive = true;

                Proxy = p_cfg.ConfiguredProxy;

                _webClient.Proxy = Proxy;
            }
            else
            {
                ProxyActive = false;
            }

            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            _webClient.Headers.Add("beRemote.Client", ValidClientHash);

            // Get the current assembly version to communicate with the server
            parameters.Add("VersionMajor" , Major.ToString());
            parameters.Add("VersionMinor", Minor.ToString());
            parameters.Add("VersionBuild", Build.ToString());
            parameters.Add("VersionRevision", Revision.ToString());

            // Get the currently used plugin channel. (should be snapshot or release
            ClientChannel = Helper.GetApplicationConfiguration().GetValue("service.client", "svcclient.channel");

        }

        public Boolean Login()
        {
            if (OnRequestStarted != null)
                OnRequestStarted(this);

            try
            {
                // TODO verify if channel is valid on server
                var result = ExecuteJsonRequest(GetUri(ServiceUrl.Login + "?ClientChannel=" + ClientChannel));
                var cookies = _webClient.CookieContainer.GetCookies(new Uri(_webClient.BaseAddress));
                if (cookies.Count > 0)
                {
                    foreach (Cookie cookie in cookies)
                    {
                        if (cookie.Name.Equals("ASP.NET_SessionId") && false == cookie.Value.Equals(""))
                        {
                            foreach (var headerKey in _webClient.ResponseHeaders.AllKeys)
                            {
                                if (headerKey.ToLower().Contains("beremote"))
                                {
                                    AvailableServices.Add(_webClient.ResponseHeaders[headerKey]);
                                }
                            }

                            return true;
                        }
                    }
                }



                return false;
            }
            finally
            {
                if (OnRequestFinished != null)
                    OnRequestFinished(this);
            }


        }

        public void Logout()
        {
            ExecuteJsonRequest(GetUri(ServiceUrl.Logout));
        }

        public String ExecuteJsonRequest(Uri uri)
        {
            if (OnRequestStarted != null)
                OnRequestStarted(this);
            try
            {
                return _webClient.DownloadString(uri);
            }
            finally
            {
                if (OnRequestFinished != null)
                    OnRequestFinished(this);
            }
        }

        public FileInfo ExecuteDownloadRequest(Uri uri, String filename)
        {
            if (OnRequestStarted != null)
                OnRequestStarted(this);
            try
            {
                // store files in appdata
                String dlTmpDir = Path.Combine(Environment.GetEnvironmentVariable("appdata"), "beRemote", "tmp");
                String absoluteFilePath = Path.Combine(dlTmpDir, Guid.NewGuid().ToString().Substring(0,5) + ".bpl");

                if (false == Directory.Exists(dlTmpDir))
                    Directory.CreateDirectory(dlTmpDir);

                if (true == File.Exists(absoluteFilePath))
                    File.Delete(absoluteFilePath);

                _webClient.DownloadFile(uri, absoluteFilePath);

                if (true == File.Exists(absoluteFilePath))
                    return new FileInfo(absoluteFilePath);
                else
                    throw new FileNotFoundException("Plugin file was not downloaded or is not present at expected location", absoluteFilePath);
            }
            finally
            {
                if (OnRequestFinished != null)
                    OnRequestFinished(this);
            }
        }

        public Uri GetUri(string urlPath)
        {
            String url = _webClient.BaseAddress + urlPath;
           
                foreach (var kvp in parameters)
                {
                    if (url.Contains("?"))
                    {
                        url += String.Format("&{0}={1}", kvp.Key, kvp.Value);
                    }
                    else
                    {
                        url += String.Format("?{0}={1}", kvp.Key, kvp.Value);
                    }
                }

            
            return new Uri(url);
        }

        public Uri GetUri(string p, Dictionary<string, string> paras)
        {
            var url = GetUri(p).AbsoluteUri;

            foreach (var kvp in paras)
            {
                if (url.Contains("?"))
                {
                    url += String.Format("&{0}={1}", kvp.Key, kvp.Value);
                }
                else
                {
                    url += String.Format("?{0}={1}", kvp.Key, kvp.Value);
                }
            }


            return new Uri(url);
        }

        public void Dispose()
        {
            Logout();
            _webClient.Dispose();
        }
    }
}
