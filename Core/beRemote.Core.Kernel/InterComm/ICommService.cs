using System;
using System.ServiceModel;

namespace beRemote.Core.InterComm
{
    [ServiceContract]
    public interface ICommService
    {
        [OperationContract]
        void ShowNotification(String message);

        [OperationContract]
        void OpenNewConnection(long connectionsettingId);

        [OperationContract]
        void FocusMainWindow();
    }

}
