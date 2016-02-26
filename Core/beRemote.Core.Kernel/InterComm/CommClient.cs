using System;
using System.ServiceModel;

namespace beRemote.Core.InterComm
{
    public class CommClient : ICommService
    {
        private ChannelFactory<ICommService> pipeFactory;
        private ICommService pipeProxy;
        public CommClient() 
        {
            pipeFactory = new ChannelFactory<ICommService>(new NetNamedPipeBinding(),
                  new EndpointAddress("net.pipe://localhost/beRemoteInterComm"));
            pipeProxy = pipeFactory.CreateChannel();

        }

        public void ShowNotification(String message)
        {
            try
            {
                if (null == pipeFactory || null == pipeProxy)
                    throw new ApplicationException("Intercomm initialization error");
                pipeProxy.ShowNotification(message);
            }
            catch (Exception)
            {
                
            }
        }

        public void OpenNewConnection(long connectionSettingId)
        {
            try
            {
                if (null == pipeFactory || null == pipeProxy)
                    throw new ApplicationException("Intercomm initialization error");
                pipeProxy.OpenNewConnection(connectionSettingId);
            }
            catch (Exception)
            {
                
            }
        }

        public void FocusMainWindow()
        {
            try
            {
                if (null == pipeFactory || null == pipeProxy)
                    throw new ApplicationException("Intercomm initialization error");
                pipeProxy.FocusMainWindow();
            }
            catch (Exception)
            {

            }
        }
    }
}
