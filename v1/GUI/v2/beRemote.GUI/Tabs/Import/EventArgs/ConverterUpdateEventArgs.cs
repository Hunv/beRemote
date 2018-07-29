using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.GUI.Tabs.Import.EventArgs
{
    public class ConverterUpdateEventArgs
    {
        private string _CurrentStatus = "";
        private string _Title = "";
        private int _CurrentStep = 0;
        private int _MaxSteps = 0;

        /// <summary>
        /// The text for the Status-Line
        /// </summary>
        public string CurrentStatus
        {
            get { return (_CurrentStatus); }
            set { _CurrentStatus = value; }
        }

        /// <summary>
        /// The Title of the Loading-Window
        /// </summary>
        public string Title
        {
            get { return (_Title); }
            set { _Title = value; }
        }

        /// <summary>
        /// The Step of the current import-job
        /// </summary>
        public int CurrentStep
        {
            get { return (_CurrentStep); }
            set
            {                
                _CurrentStep = value;
            }
        }

        /// <summary>
        /// The maximum number of steps of that job
        /// </summary>
        public int MaxSteps
        {
            get { return (_MaxSteps); }
            set
            {
                if (value >= CurrentStep)
                    _MaxSteps = value;
                else
                    throw new Exception("New value is smaller than current step-value");
            }
        }
    }
}
