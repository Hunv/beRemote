using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using beRemote.GUI.Controls;
using beRemote.GUI.Controls.Items;

namespace beRemote.GUI.Controls.ViewModel
{
    /// <summary>
    /// Provides a simplified view model for the sample
    /// application.
    /// </summary>
    public class ConnectionModel : INotifyPropertyChanged
    {
        #region Original ViewModel
        private ObservableCollection<ConnectionItem> connections;

        public ObservableCollection<ConnectionItem> Connections
        {
            get { return connections; }
            set
            {
                connections = value;
                RaisePropertyChangedEvent("Connections");
            }
        }


        /// <summary>
        /// Refreshes the data.
        /// </summary>
        public void RefreshData()
        {
            Connections = ConnectionUtil.CreateConnections();
        }


        ///<summary>
        ///Occurs when a property value changes.
        ///</summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Fires the <see cref="PropertyChanged"/> event for a
        /// given property.
        /// </summary>
        /// <param name="propertyName">The changed property.</param>
        private void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public ConnectionModel()
        {
            RefreshData();
        }

        public ConnectionItem TryFindConnectionByName(string connectionName)
        {
            return TryFindConnectionByName(null, connectionName);
        }


        public ConnectionItem TryFindConnectionByName(ConnectionItem parent, string connectionName)
        {
            ObservableCollection<ConnectionItem> conns;
            conns = parent == null ? connections : parent.SubConnections;
            foreach (ConnectionItem connection in conns)
            {
                if (connection.ConnectionName == connectionName)
                {
                    return connection;
                }
                else
                {
                    ConnectionItem cI = TryFindConnectionByName(connection, connectionName);
                    if (cI != null) return cI;
                }
            }

            return null;
        }
        #endregion

        #region Events
        
        #endregion
    }
}
