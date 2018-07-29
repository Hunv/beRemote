using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace beRemote.GUI.Controls
{
    /// <summary>
    /// Interaction logic for LoadingCircle.xaml
    /// </summary>
    public partial class LoadingCircle : UserControl
    {
        private readonly DispatcherTimer animationTimer;

        public LoadingCircle()
        {
            InitializeComponent();

            animationTimer = new DispatcherTimer(DispatcherPriority.ContextIdle, Dispatcher);
            animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 75);
        }

        #region Private Methods
        private void Start()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            animationTimer.Tick += HandleAnimationTick;
            animationTimer.Start();
        }

        private void Stop()
        {
            animationTimer.Stop();
            Mouse.OverrideCursor = Cursors.Arrow;
            animationTimer.Tick -= HandleAnimationTick;
        }

        private void HandleAnimationTick(object sender, EventArgs e)
        {
            SpinnerRotate.Angle = (SpinnerRotate.Angle + 36) % 360;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            const double step = Math.PI * 2 / 10.0;
            const double offset = Math.PI;

            C0.SetValue(Canvas.LeftProperty, 50.0 +Math.Sin(offset + 0.0 * step) * 50.0);
            C0.SetValue(Canvas.TopProperty, 50 +Math.Cos(offset + 0.0 * step) * 50.0);

            C1.SetValue(Canvas.LeftProperty, 50.0 +Math.Sin(offset + 1.0 * step) * 50.0);
            C1.SetValue(Canvas.TopProperty, 50 +Math.Cos(offset + 1.0 * step) * 50.0);

            C2.SetValue(Canvas.LeftProperty, 50.0 +Math.Sin(offset + 2.0 * step) * 50.0);
            C2.SetValue(Canvas.TopProperty, 50 +Math.Cos(offset + 2.0 * step) * 50.0);

            C3.SetValue(Canvas.LeftProperty, 50.0 +Math.Sin(offset + 3.0 * step) * 50.0);
            C3.SetValue(Canvas.TopProperty, 50 +Math.Cos(offset + 3.0 * step) * 50.0);

            C4.SetValue(Canvas.LeftProperty, 50.0 +Math.Sin(offset + 4.0 * step) * 50.0);
            C4.SetValue(Canvas.TopProperty, 50 +Math.Cos(offset + 4.0 * step) * 50.0);

            C5.SetValue(Canvas.LeftProperty, 50.0 +Math.Sin(offset + 5.0 * step) * 50.0);
            C5.SetValue(Canvas.TopProperty, 50 +Math.Cos(offset + 5.0 * step) * 50.0);

            C6.SetValue(Canvas.LeftProperty, 50.0 +Math.Sin(offset + 6.0 * step) * 50.0);
            C6.SetValue(Canvas.TopProperty, 50 +Math.Cos(offset + 6.0 * step) * 50.0);

            C7.SetValue(Canvas.LeftProperty, 50.0 +Math.Sin(offset + 7.0 * step) * 50.0);
            C7.SetValue(Canvas.TopProperty, 50 +Math.Cos(offset + 7.0 * step) * 50.0);

            C8.SetValue(Canvas.LeftProperty, 50.0 +Math.Sin(offset + 8.0 * step) * 50.0);
            C8.SetValue(Canvas.TopProperty, 50 +Math.Cos(offset + 8.0 * step) * 50.0);
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;

            if (isVisible)
                Start();
            else
                Stop();
        }
        #endregion

        
    }
}
