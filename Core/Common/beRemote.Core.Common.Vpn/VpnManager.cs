using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.Core.Common.Vpn
{
    /// <summary>
    /// Manages the instances of VPN-Connections
    /// </summary>
    public class VpnManager
    {
        private static VpnManager _instance;
        public static VpnManager Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new VpnManager();
                return _instance;
            }
        }

        private Dictionary<int, VpnBase> _ListVpn = new Dictionary<int, VpnBase>();

        public bool ConnectVpn(int vpnId)
        {
            if (_ListVpn.ContainsKey(vpnId))
            {
                //Connection already established
                if (_ListVpn[vpnId].IsConnected)
                {
                    _ListVpn[vpnId].ConnectionCounter++; //One more connection uses this VPN
                    return (true);
                }

                if (_ListVpn[vpnId].Connect() == false)
                {
                    return (false);
                }
                
                _ListVpn[vpnId].ConnectionCounter++; //One more connection uses this VPN
                return (true);
            }

            VpnBase vpnConnection = GetUserVpnConnection(vpnId);

            //Add VPN-Connection to list
            if (vpnConnection != null)
            {
                _ListVpn.Add(vpnConnection.Id, vpnConnection);
                _ListVpn[vpnId].ConnectionCounter = 1;

                bool establishSuccess = vpnConnection.Connect();

                if (establishSuccess == false)
                {
                    _ListVpn.Remove(vpnConnection.Id);
                }
                
                return (establishSuccess);
            }

            return(false);
        }

        /// <summary>
        /// Disconnects the given ID, if it is not used anymore
        /// </summary>
        /// <param name="vpnId"></param>
        /// <returns>The Disconnect was handled successful. If there is a second connection that uses this VPN, the connection will stay open!</returns>
        public bool DisconnectVpn(int vpnId)
        {
            return (DisconnectVpn(vpnId, false));
        }

        /// <summary>
        /// Disconnects the given ID
        /// </summary>
        /// <param name="vpnId">ID of the VPN</param>
        /// <param name="force">Force disconnect, even if it is used by another connection?</param>
        /// <returns>The Disconnect was handled successful. If there is a second connection that uses this VPN, the connection will stay open except you use the force-Parameter!</returns>
        public bool DisconnectVpn(int vpnId, bool force)
        {
            if (_ListVpn.ContainsKey(vpnId))
            {
                //Connection already established
                if (_ListVpn[vpnId].IsConnected)
                {
                    //Check if it is used by another Connection
                    if (_ListVpn[vpnId].ConnectionCounter > 1)
                    {
                        //Reduce Usage-Counter by one
                        _ListVpn[vpnId].ConnectionCounter--;
                        return (true);
                    }
                    
                    //Disconnect VPN
                    _ListVpn[vpnId].ConnectionCounter--;
                    return (_ListVpn[vpnId].Disconnect());
                }
            }
            return (false);
        }

        /// <summary>
        /// Gets all Existing VPNs. This can be connected or disconnected
        /// </summary>
        /// <returns></returns>
        public List<VpnBase> GetVpnList()
        {
            var vpnList = GetUserVpnConnections();

            return (vpnList);
        }


        #region Database queries
        /// <summary>
        /// Gets all VPNs of a User
        /// </summary>
        /// <returns></returns>
        private List<VpnBase> GetUserVpnConnections()
        {
            var dT = StorageCore.Core.GetUserVpnConnections();
            var vpnConnections = new List<VpnBase>();

            for (var i = 0; i < dT.Rows.Count; i++)
            {
                switch (Convert.ToInt32(dT.Rows[i]["type"]))
                {
                    default: //Invalid
                        continue;
                    case 0: //Undefined
                        var bVpn = new VpnBase();
                        bVpn.Id = Convert.ToInt32(dT.Rows[i]["id"]);
                        bVpn.Name = StorageCore.Core.EnfuseString(dT.Rows[i]["name"].ToString());
                        bVpn.Type = VpnType.Undefined;

                        vpnConnections.Add(bVpn);
                        continue;
                    case 1: //Cisco VPN
                        var cVpn = new CiscoVpn();
                        cVpn.ClientPath = StorageCore.Core.EnfuseString(dT.Rows[i]["parameter2"].ToString());
                        cVpn.ConfigName = StorageCore.Core.EnfuseString(dT.Rows[i]["parameter1"].ToString());

                        if (dT.Rows[i]["parameter3"] != null && dT.Rows[i]["parameter3"].ToString().Length > 0)
                            cVpn.ShowAuthenticationWindow = Convert.ToBoolean(dT.Rows[i]["parameter3"]);
                        if (dT.Rows[i]["parameter4"] != null && dT.Rows[i]["parameter4"].ToString() != "")
                            cVpn.CredentialId = Convert.ToInt32(dT.Rows[i]["parameter4"]);
                        cVpn.Name = StorageCore.Core.EnfuseString(dT.Rows[i]["name"].ToString());
                        cVpn.Id = Convert.ToInt32(dT.Rows[i]["id"]);
                        cVpn.Type = VpnType.CiscoVpn;

                        vpnConnections.Add(cVpn);
                        break;
                    case 2: //OpenVPN
                        var oVpn = new OpenVpn();
                        oVpn.ConfigPath = StorageCore.Core.EnfuseString(dT.Rows[i]["parameter1"].ToString());
                        oVpn.ConfigName = StorageCore.Core.EnfuseString(dT.Rows[i]["parameter2"].ToString());
                        oVpn.Name = StorageCore.Core.EnfuseString(dT.Rows[i]["name"].ToString());
                        oVpn.Id = Convert.ToInt32(dT.Rows[i]["id"]);
                        oVpn.Type = VpnType.OpenVpn;

                        vpnConnections.Add(oVpn);
                        break;
                    case 3: //Windows
                        var wVpn = new WindowsVPN();
                        wVpn.ConfigName = StorageCore.Core.EnfuseString(dT.Rows[i]["parameter1"].ToString());
                        if (dT.Rows[i]["parameter3"] != null && dT.Rows[i]["parameter3"].ToString().Length > 0)
                            wVpn.Parameter3 = Convert.ToBoolean(dT.Rows[i]["parameter3"]).ToString();
                        if (dT.Rows[i]["parameter4"] != null && dT.Rows[i]["parameter4"].ToString() != "")
                            wVpn.CredentialId = Convert.ToInt32(dT.Rows[i]["parameter4"]);
                        wVpn.Name = StorageCore.Core.EnfuseString(dT.Rows[i]["name"].ToString());
                        wVpn.Id = Convert.ToInt32(dT.Rows[i]["id"]);
                        wVpn.Type = VpnType.WindowsVpn;

                        vpnConnections.Add(wVpn);
                        break;
                    case 4: //Shrewsoft VPN
                        var sVpn = new ShrewSoftVPN();
                        sVpn.ConfigPath = StorageCore.Core.EnfuseString(dT.Rows[i]["parameter1"].ToString());
                        sVpn.ConfigName = StorageCore.Core.EnfuseString(dT.Rows[i]["parameter2"].ToString());                                                
                        if (dT.Rows[i]["parameter3"] != null && dT.Rows[i]["parameter3"].ToString().Length > 0)
                            sVpn.ShowCredentialDialog = Convert.ToBoolean(dT.Rows[i]["parameter3"]);
                        if (dT.Rows[i]["parameter4"] != null && dT.Rows[i]["parameter4"].ToString() != "")
                            sVpn.CredentialId = Convert.ToInt32(dT.Rows[i]["parameter4"]);
                        sVpn.Name = StorageCore.Core.EnfuseString(dT.Rows[i]["name"].ToString());
                        sVpn.Id = Convert.ToInt32(dT.Rows[i]["id"]);
                        sVpn.Type = VpnType.ShrewSoftVpn;

                        vpnConnections.Add(sVpn);
                        break;
                }
            }

            return (vpnConnections);
        }

        /// <summary>
        /// Gets the Definde VPN
        /// </summary>
        /// <param name="id">ID of the VPN</param>
        /// <returns></returns>
        private VpnBase GetUserVpnConnection(int id)
        {
            return GetUserVpnConnections().FirstOrDefault(vpn => vpn.Id.Equals(id));

            //var dT = StorageCore.Core.GetUserVpnConnection(id);

            //VpnBase vpnConnection = null;

            //if (dT.Rows.Count < 1)
            //{
            //    return (null);
            //}

            //switch (Convert.ToInt32(dT.Rows[0]["type"]))
            //{
            //    default: //Invalid / Undefined
            //        return (null);
            //    case 1: //Cisco VPN
            //        var cVpn = new CiscoVpn();
            //        cVpn.ClientPath = StorageCore.Core.EnfuseString(dT.Rows[0]["parameter1"].ToString());
            //        cVpn.ConfigName = StorageCore.Core.EnfuseString(dT.Rows[0]["parameter2"].ToString());
            //        cVpn.ShowAuthenticationWindow = Convert.ToBoolean(dT.Rows[0]["parameter3"].ToString());
            //        cVpn.CredentialId = Convert.ToInt32(dT.Rows[0]["parameter4"].ToString());
            //        cVpn.Name = StorageCore.Core.EnfuseString(dT.Rows[0]["name"].ToString());
            //        cVpn.Id = Convert.ToInt32(dT.Rows[0]["id"]);

            //        vpnConnection = cVpn;
            //        break;
            //    case 2: //OpenVPN
            //        var oVpn = new OpenVpn();
            //        oVpn.ConfigPath = StorageCore.Core.EnfuseString(dT.Rows[0]["parameter1"].ToString());
            //        oVpn.ConfigName = StorageCore.Core.EnfuseString(dT.Rows[0]["parameter2"].ToString());
            //        oVpn.Name = StorageCore.Core.EnfuseString(dT.Rows[0]["name"].ToString());
            //        oVpn.Id = Convert.ToInt32(dT.Rows[0]["id"]);

            //        vpnConnection = oVpn;
            //        break;
            //    case 3: //Windows
            //        break;
            //    case 4: //Shrewsoft
            //        //Not Supported
            //        break;
            //}


            //return (vpnConnection);
        }

        #endregion
    }
}
