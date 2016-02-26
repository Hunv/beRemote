using System;
using System.Windows.Threading;

namespace beRemote.Core.InterComm
{
    public class CommService : ICommService
    {
      //public string ProcessInput(string value)
        //{
        //    return value.ToLower() + Guid.NewGuid().ToString();
        //}
        //public void ExecuteCommand(Command command)
        //{
        //    command.Execute();
        //}
      

        public void ShowNotification(string message)
        {
            GUI.Notification.TrayIcon.TrayIconInstance.ShowNotification(message);
        }

        public void OpenNewConnection(long connectionsettingId)
        {
            Kernel.TriggerNewConnection(connectionsettingId);
        }

        public void FocusMainWindow()
        {
            //System.Windows.Application.Current.MainWindow.Dispatcher.BeginInvoke(new Action(delegate
            //{
            //    System.Windows.Application.Current.MainWindow.BringIntoView();
            //    System.Windows.Application.Current.MainWindow.Focus();
            //}));
            CommServer.Instance.Events.SendSetFocusToMainWindow();
        }
    }
}
