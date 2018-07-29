using System.Drawing;
using beRemote.GUI.Notification.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace beRemote.GUI.Notification
{
    public sealed class TrayIcon
    {
        private Dictionary<Guid, NotificationObj> notifications = new Dictionary<Guid, NotificationObj>();
        private ContextMenu trayContext = null;
        private MenuItem mnuNotifications = null;
        private MenuItem mnuAdvanced = null;

        private NotificationObj _activeNotificationObj;

        private Thread _activityCheckThread;
        private bool _stopActivityCheckThread = false;
        private bool _activityIconAnimated = false;

        private static TrayIcon tray_icon;
        public static TrayIcon TrayIconInstance
        {
            get 
            {
                if (tray_icon == null)
                    tray_icon = new TrayIcon();

                return tray_icon;
            }
        }

        public TrayIconState State = TrayIconState.LoginScreen;

        private NotifyIcon notify_icon;
        public NotifyIcon Icon
        {
            get
            {
                return notify_icon;
            }
        }
        public TrayIcon()
        {
            notify_icon = new NotifyIcon();
            notify_icon.Text = "beRemote";

            if(trayContext == null)
                trayContext = new ContextMenu();

            if (mnuNotifications == null)
                mnuNotifications = new MenuItem("Notifications");
            
            mnuNotifications.MenuItems.Add("< no notifications >");

            if (mnuAdvanced == null)
                mnuAdvanced = new MenuItem("Advanced");

            MenuItem kernelSettings = new MenuItem("Config editor");
            kernelSettings.Click += kernelSettings_Click;

            mnuAdvanced.MenuItems.Add(kernelSettings);
            
            trayContext.MenuItems.Add(mnuNotifications);

            notify_icon.ContextMenu = trayContext;

        }

        private void StartActivityChecker()
        {
            _stopActivityCheckThread = false;
            if (_activityCheckThread == null)
            {
                _activityCheckThread = new Thread(ActivityChecker);
                _activityCheckThread.Start();    
            }
            
        }

        /// <summary>
        /// Calling this method will start a new thread that animates the tray icon
        /// </summary>
        private void ActivityChecker()
        {
            while (!_stopActivityCheckThread)
            {
                switch (State)
                {
                    case TrayIconState.LoginScreen:
                        _stopActivityCheckThread = true;
                        SetIconResource(Resources.icon_bw);
                        break;
                    case TrayIconState.LoggedIn:
                        _stopActivityCheckThread = true;
                        SetIconResource(Resources.icon);
                        break;
                    case TrayIconState.AnimateIcon:
                        if (_activityIconAnimated)
                        {
                            SetIconResource(Resources.icon);
                            _activityIconAnimated = false;
                        }
                        else 
                        {
                            SetIconResource(Resources.icon_debug);
                            _activityIconAnimated = true;
                        }
                        break;
                }

                Thread.Sleep(1000);
            }
        }

        private void SetIconResource(Icon icon)
        {
            Application.Current.Dispatcher.BeginInvoke((Action) (() =>
            {
                notify_icon.Icon = icon;
            }));
            
        }

        void kernelSettings_Click(object sender, EventArgs e)
        {
            //var t = new Client();
            //t.ExecuteCommandFlag(CommandId.NotificationDummy);

            //var t = new beRemote.Core.AppInterComm.Client().ProcessInput("TEST");
            //var t = new beRemote.Core.AppInterComm.Client();
            //t.Connect();

            //t.SendMessage(beRemote.Core.AppInterComm.Commands.NOTIFICATION_DUMMY);

            //t.Stop();
        }

        public void Show()
        {
            notify_icon.Visible = true;

            notify_icon.BalloonTipClicked += notify_icon_BalloonTipClicked;

            _stopActivityCheckThread = false;
        }

        public void Hide()
        {
            notify_icon.Visible = false;

            notify_icon.BalloonTipClicked -= notify_icon_BalloonTipClicked;

            _stopActivityCheckThread = true;
            
        }

        public void SetIconState(TrayIconState state)
        {
            State = state;

            switch (State)
            {
                case TrayIconState.LoginScreen:
                    trayContext.MenuItems.Add(mnuAdvanced);
                    SetIconResource(Resources.icon_bw);
                    _stopActivityCheckThread = true;
                    break;
                case TrayIconState.LoggedIn:
                    SetIconResource(Resources.icon);
                    trayContext.MenuItems.Remove(mnuAdvanced);
                    _stopActivityCheckThread = true;
                    break;
                case TrayIconState.AnimateIcon:
                    StartActivityChecker();
                    break;
            }
        }

        public void ShowNotification(String message)
        {
            ShowNotification(message, 3000);
        }

        public void ShowNotification(String message, int duration)
        {
            ShowNotification(message, duration, null);
        }

        public void ShowNotification(String message, int duration, Window wnd)
        {
            ShowNotification(Guid.NewGuid(), message, duration, null);
        }

        public void ShowNotification(String message, Window wnd)
        {
            ShowNotification(message, 3000, wnd);
        }

        public void ShowNotification(Guid id, String message, int duration, Window wnd)
        {
            _activeNotificationObj = AddNotificationObj(id, message, wnd);

            //notify_icon.ShowBalloonTip(duration, "beRemote notification", message, ToolTipIcon.None);
            ShowBalloonTip(duration, "beRemote notification", message, ToolTipIcon.None);
        }

        public void ShowNotification(string message, Exception exc)
        {
            _activeNotificationObj = AddNotificationObj(Guid.NewGuid(), String.Format("{0}\r\n{1}\r\n\r\n{2}", message, exc.Message, exc.ToString()), null);

            ShowBalloonTip(3000, "beRemote exception", message, ToolTipIcon.None);
        }

        private NotificationObj AddNotificationObj(Guid id, string message, Window wnd)
        {
            var obj = new NotificationObj(id, message, wnd);
            notifications.Add(id, obj);

            if (mnuNotifications.MenuItems.Count == 1)
            {
                if (mnuNotifications.MenuItems[0].Text.ToLower().Contains("no noti"))
                {
                    mnuNotifications.MenuItems.Clear();
                }
            }

            mnuNotifications.MenuItems.Add(notifications[id].MenuItem);

            return obj;
        }

        private void ShowBalloonTip(int duration, string title, string message, ToolTipIcon toolTipIcon)
        {
            notify_icon.ShowBalloonTip(duration, title, message, toolTipIcon);
        }

        void notify_icon_BalloonTipClicked(object sender, EventArgs e)
        {
            if (null != _activeNotificationObj)
            {
             _activeNotificationObj.ShowDialog();   
            }
        }
    }
}
