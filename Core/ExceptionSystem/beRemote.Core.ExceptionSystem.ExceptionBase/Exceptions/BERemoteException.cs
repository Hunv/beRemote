using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions
{
    [Serializable]
    public class BERemoteException : ApplicationException
    {
        private String _message;

        public String GetMessage()
        {
            return _message;
        }

        public BERemoteException(String message)
            : base(message)
        {
            this._message = message;
            ExceptionHandler.GetInstance().Handle(message, this);
            
        }

        public BERemoteException(String message, Exception innerEx)
            : base(message, innerEx)
        {
            this._message = message;
            ExceptionHandler.GetInstance().Handle(_message, this);
           
        }
    }
}

