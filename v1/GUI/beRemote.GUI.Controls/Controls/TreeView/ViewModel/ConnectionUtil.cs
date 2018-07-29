using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media.Imaging;
using beRemote.GUI.Controls.Items;
using beRemote.Core.StorageSystem.StorageBase;
using System.Windows.Media;

namespace beRemote.GUI.Controls
{
    public static class ConnectionUtil
    {
        public static ObservableCollection<ConnectionItem> CreateConnections()
        {
            ObservableCollection<ConnectionItem> list = new ObservableCollection<ConnectionItem>(); //The full List of the Icons in the TreeView           
            

            //BitmapImage overlay = new BitmapImage();
            //overlay.BeginInit();
            //overlay.UriSource = new Uri("pack://application:,,,/Images/ovl_redrec.png", UriKind.Absolute);
            //overlay.EndInit();
            

            
            //ConnectionItem books = new ConnectionItem("Server Company", ConnectionTypeItems.folder, null, null, new List<ImageSource>(1){overlay},0 );
            //ConnectionItem movies = new ConnectionItem("Server Customer", ConnectionTypeItems.folder);
            //ConnectionItem music = new ConnectionItem("Server Home", ConnectionTypeItems.folder);
            //ConnectionItem programs = new ConnectionItem("Server Demo", ConnectionTypeItems.folder);

            ////root categories
            //list.Add(books);
            //list.Add(movies);
            //list.Add(music);
            //list.Add(programs);

            ////2nd level items on all categories
            //books.SubConnections.Add(new ConnectionItem("Server1", ConnectionTypeItems.connection, books));
            //books.SubConnections.Add(new ConnectionItem("Server2", ConnectionTypeItems.connection, books));
            //books.SubConnections.Add(new ConnectionItem("Server3", ConnectionTypeItems.connection, books));

            //movies.SubConnections.Add(new ConnectionItem("Server4", ConnectionTypeItems.connection, movies));
            //movies.SubConnections.Add(new ConnectionItem("Server5", ConnectionTypeItems.connection, movies));
            //movies.SubConnections.Add(new ConnectionItem("Server6", ConnectionTypeItems.connection, movies));
            //movies.SubConnections.Add(new ConnectionItem("Server7", ConnectionTypeItems.connection, movies));

            //music.SubConnections.Add(new ConnectionItem("Server8", ConnectionTypeItems.connection, music));
            //music.SubConnections.Add(new ConnectionItem("Server9", ConnectionTypeItems.connection, music));
            //music.SubConnections.Add(new ConnectionItem("Server10", ConnectionTypeItems.connection, music));
            //music.SubConnections.Add(new ConnectionItem("Server11", ConnectionTypeItems.connection, music));
            //ConnectionItem rock = new ConnectionItem("Server12", ConnectionTypeItems.connection, music);
            //music.SubConnections.Add(rock);

            ////get 3rd level on rock
            //rock.SubConnections.Add(new ConnectionItem("RDP", ConnectionTypeItems.protocol, rock));
            //rock.SubConnections.Add(new ConnectionItem("VNC", ConnectionTypeItems.protocol, rock));
            //rock.SubConnections.Add(new ConnectionItem("Chromium", ConnectionTypeItems.protocol, rock));



            return list;
        }
    }
}
