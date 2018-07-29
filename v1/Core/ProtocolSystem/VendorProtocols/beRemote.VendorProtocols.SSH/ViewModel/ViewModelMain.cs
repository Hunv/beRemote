using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beRemote.VendorProtocols.SSH.EventArgs;
using beRemote.VendorProtocols.SSH.ViewModel.Command;

namespace beRemote.VendorProtocols.SSH.ViewModel
{
    public class ViewModelMain : INotifyPropertyChanged, IDisposable
    {
        public CmdSendCommandImpl CmdSendCommand { get; set; }

        #region Constructor

        public ViewModelMain()
        {
            CmdSendCommand = new CmdSendCommandImpl();
            CmdSendCommand.SendCommand += CmdSendCommand_SendCommand;
        }
        #endregion

        #region Command Executions
        public void CmdSendCommand_SendCommand(object sender, System.Windows.RoutedEventArgs e)
        {
            //No Command required, of no text is given
            if (CommandText.Length <= 0) 
                return;

            var e2 = new SendInputEventArgs();
            e2.SendCommand = CommandText;
            OnSendInput(e2);

            CommandText = "";
        }
        #endregion

        #region Properties
        private string _DisplayText = "";
        public string DisplayText
        {
            get { return _DisplayText; }
            set
            {
                _DisplayText = value;
                RaisePropertyChanged("DisplayText");
            }
        }

        private string _CommandText = "";
        public string CommandText
        {
            get { return _CommandText; }
            set
            {
                _CommandText = value;
                RaisePropertyChanged("CommandText");
            }
        }

        private bool _TextWrap;
        public bool TextWrap 
        {
            get { return _TextWrap; }
            set
            {
                _TextWrap = value;
                RaisePropertyChanged("TextWrap");
            }
        } 
        #endregion

        #region SendInput

        public delegate void SendInputEventHandler(object sender, SendInputEventArgs e);

        public event SendInputEventHandler SendInput;

        protected virtual void OnSendInput(SendInputEventArgs e)
        {
            var Handler = SendInput;
            if (Handler != null)
                Handler(this, e);
        }

        public bool IsSendInputRegistered
        {
            get { return(SendInput != null); }
        }

        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; //To Update Content on the Form

        /// <summary>
        /// Helper for Triggering PropertyChanged
        /// </summary>
        /// <param name="triggerControl">The Name of the Property to update</param>
        private void RaisePropertyChanged(string triggerControl)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(triggerControl));
            }
        }
        #endregion

        #region

        public void Dispose()
        {
            CmdSendCommand.SendCommand -= CmdSendCommand_SendCommand;
            CmdSendCommand = null;

            DisplayText = "";
            CommandText = "";
        }

        #endregion
    }
}
