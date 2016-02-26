using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdDeleteItemImpl : ICommand
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public void Execute(object sender)
        {
            if (sender != null)
                OnItemRemove(new ItemRemoveEventArgs((ConnectionItem)sender));
        }

        public event EventHandler CanExecuteChanged;

        #region Events

        public delegate void ItemRemoveEventHandler(object sender, ItemRemoveEventArgs e);

        public event ItemRemoveEventHandler ItemRemove;

        protected virtual void OnItemRemove(ItemRemoveEventArgs e)
        {
            var Handler = ItemRemove;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }
}
