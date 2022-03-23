using ColemanPeerToPeer.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * Used to keep allow the passing of arguments between mainWindow and mainViewModel
 * by being an intermediary to get each other's instances as
 * both classes get instantiated in InitializeComponent() of main window
 */

namespace ColemanPeerToPeer
{
    public static class ViewManager
    {
        private static MainWindow _mainWindow;
        private static MainViewModel _mainViewModel;
        private static string _username;

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

        public static void SetUsername(string s)
        {
            _username = s;
        }

        public static string GetUsername()
        {
            return _username;
        }
    }
}
