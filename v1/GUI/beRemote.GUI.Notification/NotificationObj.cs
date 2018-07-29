using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace beRemote.GUI.Notification
{
    public class NotificationObj
    {
        private bool IsShown = false;

        public Guid Id { get; private set; }
        public String Message { get; private set; }
        public Window Window { get; private set; }

        public NotificationObj(Guid id, String message, Window wnd)
        {
            this.Id = id;
            this.Message = message;
            this.Window = wnd;
            if(message.Length > 15)
                MenuItem = new System.Windows.Forms.MenuItem(message.Substring(0, 15) + "...");
            else
                MenuItem = new System.Windows.Forms.MenuItem(message);

            MenuItem.Click += MenuItem_Click;

            //if (Window == null)
            //{
            //    Window = new NotificationWindow(Message);
            //}

        }

        void MenuItem_Click(object sender, EventArgs e)
        {
            //if (Window != null)
            //{
               ShowDialog();
            //}
            //else
            //{
                
            //}
        }

        public System.Windows.Forms.MenuItem MenuItem { get; private set; }

        public void ShowDialog()
        {
            if (IsShown == false)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    IsShown = true;
                    if (Window == null)
                    {
                        var wnd = new NotificationWindow(Message);
                        wnd.Owner = Application.Current.MainWindow;
                        wnd.Show();
                    }
                    else
                        Window.ShowDialog();

                    IsShown = false;
                }));
            }
            
        }
    }
}


