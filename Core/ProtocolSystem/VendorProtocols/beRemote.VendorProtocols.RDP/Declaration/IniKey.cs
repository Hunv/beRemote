using System;

namespace beRemote.VendorProtocols.RDP.Declaration
{
    class IniKey : Core.ProtocolSystem.ProtocolBase.Declaration.IniKey
    {
        public static String SECURITY_AUTH_NLA = "rdp.security.authentication.nla";
        public static String FITTOTAB = "rdp.fittotab";
        public static String RESOLUTIONX = "rdp.resolutionx";
        public static String RESOLUTIONY = "rdp.resolutiony";
        public static String SMARTSIZE = "rdp.smartsize";
        public static String COLORQTY = "rdp.color.qty";
        public static String LOCAL_RES_SOUND_OUT = "rdp.local.resources.sound.out";
        public static String LOCAL_RES_SOUND_IN = "rdp.local.resources.sound.in";        
        public static String LOCAL_RES_DEV_SMARTCARD = "rdp.local.resources.devices.smartcard";
        public static String LOCAL_RES_DEV_PORTS = "rdp.local.resources.devices.ports";
        public static String LOCAL_RES_DEV_DRIVES = "rdp.local.resources.devices.drives";
        public static String GATEWAY_USE = "rdp.gateway.enabled";
        public static String GATEWAY_CREDENTIALS = "rdp.gateway.credentials";
        public static String GATEWAY_SERVER = "rdp.gateway.server";
    }
}
