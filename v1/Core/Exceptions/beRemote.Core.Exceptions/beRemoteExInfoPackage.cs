using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Core.Exceptions
{
    public class beRemoteExInfoPackage
    {        
        private Definitions.ExceptionUrgency exception_urgency;
        
        /// <summary>
        /// complete StackTrace for this info package
        /// </summary>
        private StackTrace stackTrace;
        
        /// <summary>
        /// Current frame for this info package
        /// </summary>
        private StackFrame stackframe;

        public String ModuleNameFull { get { return stackframe.GetMethod().DeclaringType.ToString() + "." + ModuleName; } }
        public String ModuleName { get { return stackframe.GetMethod().Name; } }
        public Definitions.ExceptionUrgency ExceptionUrgency { get { return exception_urgency; } }
        
        /// <summary>
        /// Current frame for this info package
        /// </summary>
        public StackFrame ModuleStackFrame { get { return stackframe; } }
        /// <summary>
        /// Current trace for this info package
        /// </summary>
        public StackTrace ModuleStackTrace { get { return stackTrace; } }


        public String ModuleCallStack
        {
            get
            {
                String result = "";
                foreach (StackFrame frame in stackTrace.GetFrames())
                {
                    if (frame != stackTrace.GetFrame(0))
                    {
                        try
                        {
                            result += "   " + frame.GetMethod().DeclaringType.ToString() + "." + frame.GetMethod().Name + "\r\n";
                        }
                        catch (Exception)
                        {
                            result += "  < error in stack >";
                        }
                        
                    }
                }
                return result;
            }
        }

        public beRemoteExInfoPackage(StackTrace stacktrace, Definitions.ExceptionUrgency urgency)
        {
            stackTrace = stacktrace;

            stackframe = stacktrace.GetFrame(1);

            exception_urgency = urgency;
        }

        public override string ToString()
        {
            String template = "\r\nObject: {0}\r\nCalling module name: {1}\r\nUrgency: [{2}] {3}\r\nCall stack:\r\n{4}\r\n";

            return String.Format(template,
                base.ToString(),
                ModuleNameFull,
                (int)exception_urgency,
                exception_urgency.ToString(),
                ModuleCallStack);
        }

        public static beRemoteExInfoPackage NewInformationExceptionPackage(Definitions.ExceptionUrgency urgency)
        {
            // .GetFrame(1).GetMethod().Name;
            return new beRemoteExInfoPackage(new StackTrace(), urgency);
        }

        /// <summary>
        /// Package with minor urgency. Most occurences are ignorable. this should only be logged
        /// </summary>
        public static beRemoteExInfoPackage MinorInformationPackage
        {
            get
            {
                return new beRemoteExInfoPackage(new StackTrace(), Definitions.ExceptionUrgency.MINOR);
            }
        }

        /// <summary>
        /// package with major urgency. This will be logged and the user will be notifed
        /// </summary>
        public static beRemoteExInfoPackage MajorInformationPackage
        {
            get
            {
                return new beRemoteExInfoPackage(new StackTrace(), Definitions.ExceptionUrgency.MAJOR);
            }
        }
        /// <summary>
        /// Package with significant urgency. This will be logged, the user will be notified, 
        /// somehow a meta type of major. just to show a higher urgency
        /// </summary>
        public static beRemoteExInfoPackage SignificantInformationPackage
        {
            get
            {
                return new beRemoteExInfoPackage(new StackTrace(), Definitions.ExceptionUrgency.SIGNIFICANT);
            }
        }
        /// <summary>
        /// Package with stop urgency. This will be logged, the user will be notified, the application will stop after handling
        /// </summary>
        public static beRemoteExInfoPackage StopInformationPackage
        {
            get
            {
                return new beRemoteExInfoPackage(new StackTrace(), Definitions.ExceptionUrgency.STOP);
            }
        }
    }
}
