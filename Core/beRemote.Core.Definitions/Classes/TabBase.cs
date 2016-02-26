using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using beRemote.Core.Definitions.EventArgs;

namespace beRemote.Core.Definitions.Classes
{
    public class TabBase : ContentControl, INotifyPropertyChanged
    {
        static TabBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabBase), new FrameworkPropertyMetadata(typeof(TabBase)));
        }

        public TabBase()
        {
            _BottomTextTimer.Tick += BottomTextTimerTick;
            _TopTextTimer.Tick += TopTextTimerTick;

            _BottomTextTimer.Interval = new TimeSpan(0,0,5);
            _TopTextTimer.Interval = new TimeSpan(0, 0, 5);
        }

        void BottomTextTimerTick(object sender, System.EventArgs e)
        {
            BottomTextVisibility = Visibility.Collapsed;
            if (_CloseOnFadeOut)
                OnClose(new RoutedEventArgs());
        }

        void TopTextTimerTick(object sender, System.EventArgs e)
        {
            TopTextVisibility = Visibility.Collapsed;
            if (_CloseOnFadeOut)
                OnClose(new RoutedEventArgs());
        }

        #region Properties
        #region Header
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof(String),
            typeof(TabBase),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        #endregion

        #region TabToolTip
        public static readonly DependencyProperty TabToolTipProperty = DependencyProperty.Register(
            "TabToolTip",
            typeof(String),
            typeof(TabBase),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public string TabToolTip
        {
            get { return (string)GetValue(TabToolTipProperty); }
            set { SetValue(TabToolTipProperty, value); }
        }
        #endregion

        #region IconSource
        public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register(
            "IconSource",
            typeof(ImageSource),
            typeof(TabBase),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public ImageSource IconSource
        {
            get { return (ImageSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }
        #endregion

        #region IsMultiTab

        private bool _IsMultiTab = true;

        /// <summary>
        /// Is a Tab allowed to be opend Multiple times? Default is true.
        /// </summary>
        public bool IsMultiTab
        {
            get { return _IsMultiTab; }
            set
            {
                _IsMultiTab = value;
                RaisePropertyChanged("IsMultiTab");
            }
        }

        #endregion

        #region ShowStarttime

        private bool _ShowStarttime;

        /// <summary>
        /// Show the label in the lower center of the tab, that displays the starttime of the connection?
        /// </summary>
        public bool ShowStarttime
        {
            get { return _ShowStarttime; }
            set
            {
                _ShowStarttime = value;
                RaisePropertyChanged("ShowStarttime");
            }
        }

        #endregion

        #region Starttime

        private DateTime _Starttime = DateTime.Now;

        /// <summary>
        /// The point of time, when the Connection was started
        /// </summary>
        public DateTime Starttime
        {
            get { return _Starttime; }
            set
            {
                _Starttime = value;
                RaisePropertyChanged("Starttime");
            }
        }

        #endregion

        #region BottomText
        public static readonly DependencyProperty BottomTextProperty = DependencyProperty.Register(
            "BottomText",
            typeof(String),
            typeof(TabBase),
            new FrameworkPropertyMetadata("",
                FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public string BottomText
        {
            get { return (string)GetValue(BottomTextProperty); }
            set
            {
                SetValue(BottomTextProperty, value);
                BottomTextVisibility = Visibility.Visible;
                _BottomTextTimer.Start();
            }
        }
        #endregion

        #region BottomTextBackground
        public static readonly DependencyProperty BottomTextBackgroundProperty = DependencyProperty.Register(
            "BottomTextBackground",
            typeof(Brush),
            typeof(TabBase),
            new FrameworkPropertyMetadata(Brushes.Black,
                FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public Brush BottomTextBackground
        {
            get { return (Brush)GetValue(BottomTextBackgroundProperty); }
            set { SetValue(BottomTextBackgroundProperty, value); }
        }
        #endregion

        #region BottomTextVisibility
        public static readonly DependencyProperty BottomTextVisibilityProperty = DependencyProperty.Register(
            "BottomTextVisibility",
            typeof(Visibility),
            typeof(TabBase),
            new FrameworkPropertyMetadata(Visibility.Collapsed,
                FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public Visibility BottomTextVisibility
        {
            get { return (Visibility)GetValue(BottomTextVisibilityProperty); }
            set { SetValue(BottomTextVisibilityProperty, value); }
        }
        #endregion

        #region BottomTextTimeout
        public static readonly DependencyProperty BottomTextTimeoutProperty = DependencyProperty.Register(
            "BottomTextTimeout",
            typeof(TimeSpan),
            typeof(TabBase),
            new FrameworkPropertyMetadata(new TimeSpan(0, 0, 5),
                FrameworkPropertyMetadataOptions.AffectsRender)
            );

        /// <summary>
        /// The Time in ms the Text will be shown
        /// </summary>
        public TimeSpan BottomTextTimeout
        {
            get { return (TimeSpan)GetValue(BottomTextTimeoutProperty); }
            set
            {
                SetValue(BottomTextTimeoutProperty, value);
                _BottomTextTimer.Interval = value;
            }
        }
        #endregion

        #region TopText
        public static readonly DependencyProperty TopTextProperty = DependencyProperty.Register(
            "TopText",
            typeof(String),
            typeof(TabBase),
            new FrameworkPropertyMetadata("",
                FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public string TopText
        {
            get { return (string)GetValue(TopTextProperty); }
            set
            {
                SetValue(TopTextProperty, value);
                TopTextVisibility = Visibility.Visible;
                _TopTextTimer.Start();
            }
        }
        #endregion

        #region TopTextBackground
        public static readonly DependencyProperty TopTextBackgroundProperty = DependencyProperty.Register(
            "TopTextBackground",
            typeof(Brush),
            typeof(TabBase),
            new FrameworkPropertyMetadata(Brushes.Black,
                FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public Brush TopTextBackground
        {
            get { return (Brush)GetValue(TopTextBackgroundProperty); }
            set { SetValue(TopTextBackgroundProperty, value); }
        }
        #endregion

        #region TopTextVisibility
        public static readonly DependencyProperty TopTextVisibilityProperty = DependencyProperty.Register(
            "TopTextVisibility",
            typeof(Visibility),
            typeof(TabBase),
            new FrameworkPropertyMetadata(Visibility.Collapsed,
                FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public Visibility TopTextVisibility
        {
            get { return (Visibility)GetValue(TopTextVisibilityProperty); }
            set { SetValue(TopTextVisibilityProperty, value); }
        }
        #endregion

        #region TopTextTimeout
        public static readonly DependencyProperty TopTextTimeoutProperty = DependencyProperty.Register(
            "TopTextTimeout",
            typeof(TimeSpan),
            typeof(TabBase),
            new FrameworkPropertyMetadata(new TimeSpan(0,0,5),
                FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public TimeSpan TopTextTimeout
        {
            get { return (TimeSpan)GetValue(TopTextTimeoutProperty); }
            set
            {
                SetValue(TopTextTimeoutProperty, value);
                _TopTextTimer.Interval = value;
            }
        }
        #endregion

        #region ControlVisibility
        public static readonly DependencyProperty ControlVisibilityProperty = DependencyProperty.Register(
            "ControlVisibility",
            typeof(Visibility),
            typeof(TabBase),
            new FrameworkPropertyMetadata(Visibility.Visible,
                FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public Visibility ControlVisibility
        {
            get { return (Visibility)GetValue(ControlVisibilityProperty); }
            set { SetValue(ControlVisibilityProperty, value); }
        }
        #endregion
        #endregion

        #region Methods
        #region GetIcon
        /// <summary>
        /// Gets the given Icon as a BitmapFrame for an ImageSource
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public void SetIcon(string url)
        {
            //Load the public-overlay-Icon (small guy in the bottom right corner)
            var iconUri = new Uri(url, UriKind.RelativeOrAbsolute);
            var iconBitmap = BitmapFrame.Create(iconUri);
            iconBitmap.Freeze();
            IconSource = iconBitmap;
        }
        #endregion
        #endregion

        #region internal Management

        private DispatcherTimer _BottomTextTimer = new DispatcherTimer();
        private DispatcherTimer _TopTextTimer = new DispatcherTimer();

        //If a Text is shown, when the tab should be closed, the Tab will be closed when the text disappears
        private bool _CloseOnFadeOut;
        #endregion

        #region Events

        #region PropertyChanged
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Close-Event

        /// <summary>
        /// Closes the Tab
        /// </summary>
        public void CloseTab()
        {
            if (TopTextVisibility == Visibility.Visible ||
                BottomTextVisibility == Visibility.Visible)
            {
                _CloseOnFadeOut = true;
                ControlVisibility = Visibility.Collapsed;
                return;
            }
            OnClose(new RoutedEventArgs());
        }

        public delegate void CloseEventHandler(object sender, RoutedEventArgs e);

        public event CloseEventHandler Close;

        protected virtual void OnClose(RoutedEventArgs e)
        {
            var Handler = Close;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region ConnectionListChanged-Event

        public void RefreshConnectionList()
        {
            OnConnectionListChanged(new RoutedEventArgs());
        }

        public delegate void ConnectionListChangedEventHandler(object sender, RoutedEventArgs e);

        public event ConnectionListChangedEventHandler ConnectionListChanged;

        protected virtual void OnConnectionListChanged(RoutedEventArgs e)
        {
            var Handler = ConnectionListChanged;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region FavoritesChanged-Event

        public void RefreshFavorites(string favorites)
        {
            var evArgs = new FavoriteChangedEventArgs();
            evArgs.Favorites = favorites;

            OnFavoritesChanged(evArgs);
        }

        public delegate void FavoritesChangedEventHandler(object sender, FavoriteChangedEventArgs e);

        public event FavoritesChangedEventHandler FavoritesChanged;

        protected virtual void OnFavoritesChanged(FavoriteChangedEventArgs e)
        {
            var Handler = FavoritesChanged;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region OnContextRibbonVisibileChange

        public void ChangeContextRibbon(List<string> show, List<string> hide)
        {
            var evArgs = new ContextRibbonVisibileChangeEventArgs();
            evArgs.ShowContextRibbon = show;
            evArgs.HideContextRibbon = hide;

            OnContextRibbonVisibileChange(evArgs);
        }

        public delegate void ContextRibbonVisibileChangeEventHandler(object sender, ContextRibbonVisibileChangeEventArgs e);

        public event ContextRibbonVisibileChangeEventHandler ContextRibbonVisibileChange;

        protected virtual void OnContextRibbonVisibileChange(ContextRibbonVisibileChangeEventArgs e)
        {
            var Handler = ContextRibbonVisibileChange;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region ConnectionFilterChanged-Event

        public void RefreshConnectionFilter()
        {
            OnConnectionFilterChanged(new RoutedEventArgs());
        }

        public delegate void ConnectionFilterChangedEventHandler(object sender, RoutedEventArgs e);

        public event ConnectionFilterChangedEventHandler ConnectionFilterChanged;

        protected virtual void OnConnectionFilterChanged(RoutedEventArgs e)
        {
            var Handler = ConnectionFilterChanged;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion


        #region ConnectionClosed-Event
        public delegate void ConnectionClosedEventHandler(object sender, RoutedEventArgs e);

        public event ConnectionClosedEventHandler ConnectionClosed;

        protected virtual void OnConnectionClosed(RoutedEventArgs e)
        {
            var Handler = ConnectionClosed;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region ConnectionClosing-Event
        public delegate void ConnectionClosingEventHandler(object sender, RoutedEventArgs e);

        public event ConnectionClosingEventHandler ConnectionClosing;

        protected virtual void OnConnectionClosing(RoutedEventArgs e)
        {
            var Handler = ConnectionClosing;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region ConnectionOpened-Event
        public delegate void ConnectionOpenedEventHandler(object sender, RoutedEventArgs e);

        public event ConnectionOpenedEventHandler ConnectionOpened;

        protected virtual void OnConnectionOpened(RoutedEventArgs e)
        {
            var Handler = ConnectionOpened;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region ConnectionOpening-Event
        public delegate void ConnectionOpeningEventHandler(object sender, RoutedEventArgs e);

        public event ConnectionOpeningEventHandler ConnectionOpening;

        protected virtual void OnConnectionOpening(RoutedEventArgs e)
        {
            var Handler = ConnectionOpening;
            if (Handler != null)
                Handler(this, e);
        }

        public void TriggerConnectionOpeningEvent()
        {
            ConnectionOpening(this, null);
        }
        #endregion
        #endregion

        #region Dispose

        public virtual void Dispose()
        {
            if (ConnectionClosing != null)
                ConnectionClosing(this, null);

            _BottomTextTimer.Stop();
            _TopTextTimer.Stop();
            _BottomTextTimer = null;
            _TopTextTimer = null;
        }

        #endregion
    }
}
