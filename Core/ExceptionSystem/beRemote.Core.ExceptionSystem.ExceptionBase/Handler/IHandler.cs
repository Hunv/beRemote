using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core.ExceptionSystem.ExceptionBase.Handler
{
    public interface IHandler
    {
        void Handle(String message);
        void Handle(String message, Exception exception);
        String GetHandlerName();
    }
}
