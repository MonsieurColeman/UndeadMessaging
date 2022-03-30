/*
 This file:
-Handles the startup logic of the application
-Handles some button behaviors for the client view
 */

using System;
using System.Windows;
using ColemanPeerToPeer.MVVM.ViewModel;
using System.Windows.Input;
using System.Collections.Specialized;
using ServiceOutliner;

namespace ColemanPeerToPeer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainViewModel _viewModel;
        public bool loginSuccessful = false; //used to determine whether to shutdown app

        #region Startup Functions
        public MainWindow()
        //Handles app setup
        {
            InitializeComponent();
            ViewManager.SetMainWindowInstance(this); //used to keep aid UI windows
            ((INotifyCollectionChanged)MessageListView.Items).CollectionChanged += ListView_CollectionChanged;
            LoginView loginView = new LoginView(this);
            loginView.ShowDialog();
            if(!loginSuccessful)
                Application.Current.Shutdown();
        }

        private void ListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        // if message is added to list view, scroll to the new item into view 
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                MessageListView.ScrollIntoView(e.NewItems[0]);  
        }
        #endregion

        #region HelperFunctions
        private void ConnectToMainViewModel()
        {
            _viewModel = ViewManager.GetMainViewModelInstance();
        }

        private void ViewModelCheck()
        {
            if (_viewModel == null)
                ConnectToMainViewModel();
        }
        #endregion

        #region Window Ease of Use Functions
        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            ViewModelCheck();
            _viewModel.ShutdownChat();
            Application.Current.Shutdown();
        }

        private void Btn_Minimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Btn_Maxmize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                App.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        //Aids app accessibility
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        #endregion

        #region Button Behaviors
        public void Btn_AddMessage(object sender, RoutedEventArgs e)
        {
            ViewModelCheck();
        }
        private void CreateTopic(object sender, RoutedEventArgs e)
        {
            if (_viewModel == null)
                _viewModel = ViewManager.GetMainViewModelInstance();
            _viewModel.ShowCreateTopicDialog();
        }

        private void LeaveTopic(object sender, RoutedEventArgs e)
        {
            if (_viewModel == null)
                _viewModel = ViewManager.GetMainViewModelInstance();
            _viewModel.LeaveTopic();
        }
        #endregion
    }
}

/*
 Maintenance History

0.1 Off-loaded everything to mainViewModel
0.4 Added window ease of use functions
0.6 Added scroll view behavior
0.8 Added Login View Dialog to start the application
1.0 Added comments and history
 */
