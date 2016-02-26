using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using beRemote.Core.StorageSystem.StorageBase;

namespace beRemote.Core.Common.PluginBase
{
    /// <summary>
    /// Abstract class for the smart storage. In most cases there is no need to modify the derived class (so you can use the DefaultSmartStorage implementation), unless you want to use the generic load method on user defined classes.<br />
    /// If that's the case implement a new class that uses this class as base. The simpliest way would be to copy the DefaultSmartStorage.cs class to your solution.<br />
    /// This is needed in order to cast all user defined types of your protocol implementation. So that the SmartSTorage implementation has all needed type-definitions on its own Namespace.
    /// </summary>
    public abstract class ISmartStorage
    {
        public Plugin AssociatedPlugin { get; private set; }
        public ISmartStorage(Plugin plugin)
        {
            // Get the class name to avoid unauthorized access
            this.AssociatedPlugin = plugin;

            // try init
            StorageCore.Core.SmartStorageCreateTable(AssociatedPlugin.GetPluginIdentifier());
        }

        protected byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            
            using (MemoryStream ms = new MemoryStream())            
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        protected Object ByteArrayToObject(byte[] arrBytes)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                Object obj = (Object)binForm.Deserialize(memStream);
                return obj;
            }
        }

        /// <summary>
        /// Save any object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void Save(String key, Object data)
        {
            StorageCore.Core.SmartStorageWriteValue(key, ObjectToByteArray(data), AssociatedPlugin.GetPluginIdentifier());
        }

        public Byte[] GetByteData(String key)
        {
            return StorageCore.Core.SmartStorageReadValue(key, AssociatedPlugin.GetPluginIdentifier());
        }


        public abstract T Load<T>(String key);

        /// <summary>
        /// Method to load data from storage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue">This will be used as default if no information are present or something went wrong</param>
        /// <returns></returns>
        public abstract T Load<T>(String key, Object defaultValue);

        //public T Load<T>(String key)
        //{
        //    Object data = ByteArrayToObject(StorageCore.Core.SmartStorageReadValue(key, AssociatedProtocol.GetProtocolIdentifer()));

        //    return (T)data;
        //}
    }
}
