using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace beRemote.GUI.ViewModel.Command
{
    public sealed class EventCommand : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventCommand), null);


        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(EventCommand), null);


        public static readonly DependencyProperty InvokeParameterProperty = DependencyProperty.Register(
            "InvokeParameter", typeof(object), typeof(EventCommand), null);

        private string _CommandName;

        public object InvokeParameter
        {
            get{return GetValue(InvokeParameterProperty);}
            set{SetValue(InvokeParameterProperty, value);}
        }


        public ICommand Command
        {
            get{return (ICommand)GetValue(CommandProperty);}
            set{SetValue(CommandProperty, value);}
        }

        public string CommandName
        {
            get{return _CommandName;}
            set
            {
                if (CommandName != value)
                {
                    _CommandName = value;
                }
            }
        }

        public object CommandParameter
        {
            get{return GetValue(CommandParameterProperty);}
            set{SetValue(CommandParameterProperty, value);}
        }

        public object Sender { get; set; }

        protected override void Invoke(object parameter)
        {
            InvokeParameter = parameter;

            if (AssociatedObject != null)
            {
                ICommand command = Command;
                if ((command != null) && command.CanExecute(CommandParameter))
                {
                    command.Execute(CommandParameter);
                }
            }
        }
    }
}
