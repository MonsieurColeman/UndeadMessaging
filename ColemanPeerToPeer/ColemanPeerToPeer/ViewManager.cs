using ColemanPeerToPeer.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColemanPeerToPeer
{
    public static class ViewManager
    {
        private static MainWindow _mainWindow;
        private static MainViewModel _mainViewModel;

        public static MainWindow GetMainWindowInstance()
        {
            return _mainWindow;
        }

        public static void SetMainWindowInstance(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public static MainViewModel GetMainViewModelInstance()
        {
            return _mainViewModel;
        }

        public static void SetMainViewModelInstance(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}
