using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.Core.Exceptions.Plugin;
using beRemote.Core.ProtocolSystem.ProtocolBase;

namespace beRemote.GUI.ViewModel.Worker
{
    public class ConnectionSession
    {
        /// <summary>
        /// Opens a new Connection
        /// </summary>
        /// <param name="session"></param>
        public static async void OpenConnectionThreadedAsync(Session session)
        {
            var myTask = Task.Run(
                () =>
                {
                    try
                    {
                        session.OpenConnection();
                    }
                    catch (PluginException pEx)
                    {
                        //todo
                        // caught exception that will not interrupt the execution
                        //App.ShowHandledExceptionDialog(pEx);

                        //App.Current.Dispatcher.BeginInvoke(new Action(() =>
                        //{
                        //    var sess = (Session)session;

                        //    sess.TriggerCloseConnectionEvent();
                        //}), null);
                    }
                    catch (Exception ex)
                    {
                        // TODO: Gui exception
                        //this.Dispatcher.Invoke(new Action(() => new BERemoteGUIException("Problem while opening the connection.", ex).ShowMessageWindow(this)));
                        //new BERemoteGUIException("Problem while opening the connection.", ex).ShowMessageWindow(this);
                        //if (ex.GetType() != typeof(beRemote.Core.Exceptions.beRemoteException))
                        //{

                        //}
                        //App.ShowUnhandledExceptionDialog(ex);
                    }
                });

            await myTask;
        }
    }
}
