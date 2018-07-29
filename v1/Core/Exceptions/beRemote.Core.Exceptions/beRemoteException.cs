using beRemote.Core.Common.LogSystem;
using beRemote.GUI.Notification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Core.Exceptions
{
    public class beRemoteException : ApplicationException
    {
        private beRemoteExInfoPackage info_package;
        public beRemoteExInfoPackage ExceptionInformationPackage { get { return info_package; } }

        public virtual int EventId { get { return 0; } }

        /// <summary>
        /// Returns an Action object that executes the handle logic for exceptions.
        /// If this is null it will be ignored and default actions are executed
        /// </summary>
        /// <returns></returns>
        public virtual Action GetHandlerAction()
        {
            return null;
        }


        public beRemoteException(beRemoteExInfoPackage infoPackage)
        {
            SetBaseInfo(infoPackage);
        }

        public beRemoteException(beRemoteExInfoPackage infoPackage, String message) : base(message)
        {
            SetBaseInfo(infoPackage);
        }

        public beRemoteException(beRemoteExInfoPackage infoPackage, String message, Exception innerEx)
            : base(message, innerEx)
        {
            SetBaseInfo(infoPackage);
        }

        private void SetBaseInfo(beRemoteExInfoPackage exInfo)
        {
            info_package = exInfo;

            switch (info_package.ExceptionUrgency)
            {
                case Definitions.ExceptionUrgency.MINOR:
                    Logger.Verbose(String.Format("{0}", this) , EventId);
                    break;
                case Definitions.ExceptionUrgency.MAJOR:
                    Logger.Warning(String.Format("{0}", this), EventId);
                    break;
                default:
                case Definitions.ExceptionUrgency.SIGNIFICANT:
                case Definitions.ExceptionUrgency.STOP:
                    TrayIcon.TrayIconInstance.Icon.BalloonTipClicked += Icon_BalloonTipClicked;
                    TrayIcon.TrayIconInstance.Icon.BalloonTipClosed += Icon_BalloonTipClosed;
                    notificationVisible = true;
                    TrayIcon.TrayIconInstance.ShowNotification(String.Format("Exception thrown at: \r\n{0}\r\nException urgency: [{1}] {2}", this.info_package.ModuleNameFull, (int)this.info_package.ExceptionUrgency, this.info_package.ExceptionUrgency), this);
                    Logger.Error(String.Format("{0}", this), EventId);

                    break;
                
                    //TrayIcon.TrayIconInstance.ShowNotification(String.Format("Exception thrown at: \r\n{0}\r\nException urgency: [{1}] {2}", this.info_package.ModuleNameFull, (int)this.info_package.ExceptionUrgency, this.info_package.ExceptionUrgency));
                    //Logger.Error(String.Format("{0}", this), EventId);
                    
                    //break;
            }
        }

        void Icon_BalloonTipClosed(object sender, EventArgs e)
        {
            notificationVisible = false;
        }

        bool notificationVisible = false;
        void Icon_BalloonTipClicked(object sender, EventArgs e)
        {
            
            if(notificationVisible)
                new UIExceptionWindow(this, false).ShowDialog();
            notificationVisible = false;
        }

        public override string ToString()
        {
            String result = "[beRemote exception information ### BEGIN]\r\n";
            result += ExceptionInformationPackage;
            result += "[beRemote exception information ### END]\r\n";
            result += "\r\n" + base.ToString();

            return result;
        }

        public String HumanReadable
        {
            get { return ToString(); }
        }
    }
}
