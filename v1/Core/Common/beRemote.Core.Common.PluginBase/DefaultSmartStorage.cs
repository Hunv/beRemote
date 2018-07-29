using System;
using System.Runtime.Serialization;
using beRemote.Core.Exceptions;

namespace beRemote.Core.Common.PluginBase
{
    public class DefaultSmartStorage : ISmartStorage
    {
        public DefaultSmartStorage(Plugin plugin) : base(plugin) { }

        public override T Load<T>(String key)
        {
            try
            {
                Object data = base.ByteArrayToObject(base.GetByteData(key));

                return (T)data;
            }
            catch (SerializationException sex)
            {
                new Exceptions.Plugin.Protocol.ProtocolException(beRemoteExInfoPackage.MinorInformationPackage, "Missing key or wrong cast. If this occurs everytime something is not working as expected.", sex);
                return default(T);
            }
            catch (Exception ex)
            {
                throw new Exceptions.Plugin.Protocol.ProtocolException(beRemoteExInfoPackage.SignificantInformationPackage, "The current protocol has no implementation for SmartStorage, due to this fact some casts may result in exceptions. Please take care of the proper implementation of the SmartStorage interface.", ex);
            }
        }

        public override T Load<T>(string key, object defaultValue)
        {
            T obj = Load<T>(key);

            if (obj == null)
                return (T)defaultValue;
            else
                return obj;
        }
    }
}
