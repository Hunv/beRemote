using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace beRemote.Services.PluginDirectory.Library.Objects
{
    public class BaseObject : INotifyPropertyChanged
    {
        public String Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public T Deserialize<T>(String input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}
