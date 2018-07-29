using beRemote.Core.Definitions.Classes;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Controls.Items;
using beRemote.GUI.Tabs.Import.EventArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.GUI.Tabs.Import.Command
{
    public class CmdTabLoadedImpl : BaseCommand
    {
        public override void Execute(object sender)
        {
            var evArgs = new TabLoadedEventArgs();
            //evArgs.FolderList = GetFolderList();
            OnTabLoaded(evArgs);
        }
        #region TabLoaded

        public delegate void TabLoadedEventHandler(object sender, TabLoadedEventArgs e);

        public event TabLoadedEventHandler TabLoaded;

        protected virtual void OnTabLoaded(TabLoadedEventArgs e)
        {
            var Handler = TabLoaded;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion
    }
}
