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
        private string username = "Me";
        private string profileColor = "#000000";
        string profilePicture = "https://picsum.photos/200/300";
        public bool loginSuccessful = false;

        #region Startup Functions
        public MainWindow()
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
        {
            // if message is added to list view, scroll to the new item into view 
            if (e.Action == NotifyCollectionChangedAction.Add)
                MessageListView.ScrollIntoView(e.NewItems[0]);  
        }

        private void ConnectToMainViewModel()
        {
            _viewModel = ViewManager.GetMainViewModelInstance();
        }

        private void ViewModelCheck()
        {
            if (_viewModel == null)
                ConnectToMainViewModel();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        #endregion

        public void Btn_AddMessage(object sender, RoutedEventArgs e)
        {
            ViewModelCheck();
            //_viewModel.AddMessageToChat("cheese");
        }

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
        #endregion

        private void Btn_SendMessage(object sender, RoutedEventArgs e)
        {
            /*
            MainViewModel ad = new MainViewModel();
            MessageModel aa = new MessageModel();
            aa.Message = "sdsdsd";
            ad.Messages.Add(aa);
            */
            MessageBox.Show("no");
        }

        private void CreateTopic(object sender, RoutedEventArgs e)
        {
            if(_viewModel == null)
                _viewModel = ViewManager.GetMainViewModelInstance();
            _viewModel.ShowCreateTopicDialog();
        }

        private void LeaveTopic(object sender, RoutedEventArgs e)
        {
            if (_viewModel == null)
                _viewModel = ViewManager.GetMainViewModelInstance();
            _viewModel.LeaveTopic();
        }
    }
}
