using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Core.Common.Vpn 
{
    public class VpnBase : INotifyPropertyChanged
    {
        #region Privates

        private string _Name;
        private int _Id;
        private VpnType _Type;

        private string _Parameter1;
        private string _Parameter2;
        private string _Parameter3;
        private string _Parameter4;
        private string _Parameter5;
        private string _Parameter6;
        private string _Parameter7;
        private string _Parameter8;
        private string _Parameter9;
        private string _Parameter10;

        private bool _IsConnected;
        private int _ConnectionCounter;
        #endregion

        #region public Properties

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                triggerPropertyChanged("Name");
            }
        }

        public int Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                triggerPropertyChanged("Id");
            }
        }

        public VpnType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
                triggerPropertyChanged("Type");
                triggerPropertyChanged("TypeId");
                triggerPropertyChanged("IsValid");                
            }
        }

        public int TypeId
        {
            get { return (int) _Type; }
            set
            {
                _Type = (VpnType)value;

                triggerPropertyChanged("Type");
                triggerPropertyChanged("TypeId");
                triggerPropertyChanged("IsValid");
            }
        }

        public string Parameter1
        {
            get { return _Parameter1; }
            set
            {
                _Parameter1 = value;
                triggerPropertyChanged("Parameter1");
                triggerPropertyChanged("IsValid");
            }
        }

        public string Parameter2
        {
            get { return _Parameter2; }
            set
            {
                _Parameter2 = value;
                triggerPropertyChanged("Parameter2");
                triggerPropertyChanged("IsValid");
            }
        }

        public string Parameter3
        {
            get { return _Parameter3; }
            set
            {
                _Parameter3 = value;
                triggerPropertyChanged("Parameter3");
                triggerPropertyChanged("IsValid");
            }
        }

        public string Parameter4
        {
            get { return _Parameter4; }
            set
            {
                _Parameter4 = value;
                triggerPropertyChanged("Parameter4");
                triggerPropertyChanged("IsValid");
            }
        }

        public string Parameter5
        {
            get { return _Parameter5; }
            set
            {
                _Parameter5 = value;
                triggerPropertyChanged("Parameter5");
                triggerPropertyChanged("IsValid");
            }
        }

        public string Parameter6
        {
            get { return _Parameter6; }
            set
            {
                _Parameter6 = value;
                triggerPropertyChanged("Parameter6");
                triggerPropertyChanged("IsValid");
            }
        }

        public string Parameter7
        {
            get { return _Parameter7; }
            set
            {
                _Parameter7 = value;
                triggerPropertyChanged("Parameter7");
                triggerPropertyChanged("IsValid");
            }
        }

        public string Parameter8
        {
            get { return _Parameter8; }
            set
            {
                _Parameter8 = value;
                triggerPropertyChanged("Parameter8");
                triggerPropertyChanged("IsValid");
            }
        }

        public string Parameter9
        {
            get { return _Parameter9; }
            set
            {
                _Parameter9 = value;
                triggerPropertyChanged("Parameter9");
                triggerPropertyChanged("IsValid");
            }
        }

        public string Parameter10
        {
            get { return _Parameter10; }
            set
            {
                _Parameter10 = value;
                triggerPropertyChanged("Parameter10");
                triggerPropertyChanged("IsValid");
            }
        }

        public bool IsConnected
        {
            get { return _IsConnected; }
            set
            {
                _IsConnected = value;
                triggerPropertyChanged("IsConnected");
                triggerPropertyChanged("IsValid");
            }
        }

        /// <summary>
        /// The Number of connections, that currently uses this VPN-Tunnel
        /// </summary>
        public int ConnectionCounter
        {
            get { return _ConnectionCounter; }
            set
            {
                _ConnectionCounter = value;
                triggerPropertyChanged("ConnectionCounter");
                triggerPropertyChanged("IsValid");
            }
        }
        #endregion


        public virtual bool Connect()
        {
            return (false);
        }

        public virtual bool Disconnect()
        {
            return (false);
        }

        public virtual bool IsInstalled()
        {
            return (false);
        }

        public virtual bool Validate()
        {
            return (false);
        }

        public virtual bool Validate(bool showError)
        {
            return (false);
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; //To Update Content on the Form

        /// <summary>
        /// Helper for Triggering PropertyChanged
        /// </summary>
        /// <param name="triggerControl">The Name of the Property to update</param>
        private void triggerPropertyChanged(string triggerControl)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(triggerControl));
            }
        }
        #endregion
    }
}
