using System;

namespace beRemote.Core.Definitions.Classes
{
    public class UserVisuals
    {
        private int _MainWindowPosX;
        private int _MainWindowPosY;
        private bool _MainWindowMax;
        private int _MainWindowWidth;
        private int _MainWindowHeight;
        private string _RibbonState = "";
        private string _ExpandedNodes = "";
        private int _StatusbarSetting;
        private string _Favorites = "";
        private string _GridLayout = "";
        private string _RibbonQat = "";

        public UserVisuals() { }
        public UserVisuals(int mainWindowPosX, int mainWindowPosY, bool mainWindowMaximized, int mainWindowWidth, int mainWindowHeight, string ribbonState, string expandedNodes, int statusbarSetting, string favorites, string gridLayout, string ribbonQat)
        {
            _MainWindowPosX = mainWindowPosX;
            _MainWindowPosY = mainWindowPosY;
            _MainWindowMax = mainWindowMaximized;
            _MainWindowHeight = mainWindowHeight;
            _MainWindowWidth = mainWindowWidth;
            _RibbonState = ribbonState;
            _ExpandedNodes = expandedNodes;
            _StatusbarSetting = statusbarSetting;
            _Favorites = favorites;
            _GridLayout = gridLayout;
            _RibbonQat = ribbonQat;
        }

        [Obsolete("Use Property instead")]
        public int getMainWindowPosX() { return (_MainWindowPosX); }
        [Obsolete("Use Property instead")]
        public int getMainWindowPosY() { return (_MainWindowPosY); }
        [Obsolete("Use Property instead")]
        public bool getMainWindowIsMaximized() { return (_MainWindowMax); }
        [Obsolete("Use Property instead")]
        public int getMainWindowWidth() { return (_MainWindowWidth); }
        [Obsolete("Use Property instead")]
        public int getMainWindowHeight() { return (_MainWindowHeight); }
        [Obsolete("Use Property instead")]
        public string getRibbonState() { return (_RibbonState); }
        [Obsolete("Use Property instead")]
        public string getNodesExpanded() { return (_ExpandedNodes); }
        [Obsolete("Use Property instead")]
        public int getStatusbarSetting() { return (_StatusbarSetting); }

        /// <summary>
        /// The saved value for the X-Position of the Mainwindow
        /// </summary>
        public int MainWindowPosX { get { return (_MainWindowPosX); } set { _MainWindowPosX = value; } }

        /// <summary>
        /// The saved value for the Y-Position of the Mainwindow
        /// </summary>
        public int MainWindowPosY { get { return (_MainWindowPosY); } set { _MainWindowPosY = value; } }

        /// <summary>
        /// The saved value for the maximized-State of the Mainwindow
        /// </summary>
        public bool MainWindowMax { get { return (_MainWindowMax); } set { _MainWindowMax = value; } }

        /// <summary>
        /// The saved value for the Mainwindow Width
        /// </summary>
        public int MainWindowWidth { get { return (_MainWindowWidth); } set { _MainWindowWidth = value; } }

        /// <summary>
        /// The saved value for the Mainwindow Height
        /// </summary>
        public int MainWindowHeight { get { return (_MainWindowHeight); } set { _MainWindowHeight = value; } }

        /// <summary>
        /// The saved value for the Ribbonstate
        /// </summary>
        public string RibbonState { get { return (_RibbonState); } set { _RibbonState = value; } }

        /// <summary>
        /// The saved value for the expanded nodes of the ConnectionTreeView
        /// </summary>
        public string ExpandedNodes { get { return (_ExpandedNodes); } set { _ExpandedNodes = value; } }

        /// <summary>
        /// The saved value for the visibibility of the Statusbar-Items
        /// </summary>
        public int StatusbarSetting { get { return (_StatusbarSetting); } set { _StatusbarSetting = value; } }
        
        /// <summary>
        /// The saved value for the Favorite-Items
        /// </summary>
        public string Favorites { get { return (_Favorites); } set { _Favorites = value; } }

        /// <summary>
        /// The saved value for the Grid-Layout
        /// </summary>
        public string GridLayout { get { return (_GridLayout); } set { _GridLayout = value; } }

        /// <summary>
        /// The saved value for the Quick Access Toolbar of the Ribbon
        /// </summary>
        public string RibbonQat { get { return (_RibbonQat); } set { _RibbonQat = value; } }
    }
}
