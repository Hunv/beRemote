using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace beRemote.GUI.ViewModel.EventArg
{
    public class WindowPropertiesChangedEventArgs : RoutedEventArgs
    {
        public WindowPropertiesChangedEventArgs(int windowWidth, int windowHeight, int windowPositionX, int windowPositionY)
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            WindowPositionX = windowPositionX;
            WindowPositionY = windowPositionY;
        }

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public int WindowPositionX { get; set; }
        public int WindowPositionY { get; set; }
    }
}
