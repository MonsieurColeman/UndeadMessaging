using System;

using System.Windows;
using ColemanPeerToPeer.MVVM.ViewModel;
using ColemanPeerToPeer.MVVM.Model;
using System.Windows.Input;
using System.Collections.Specialized;

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

        #region Startup Functions
        public MainWindow()
        {
            InitializeComponent();
            ViewManager.SetMainWindowInstance(this); //used to keep aid UI windows
            ((INotifyCollectionChanged)MessageListView.Items).CollectionChanged += ListView_CollectionChanged;
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
            MainViewModel ad = new MainViewModel();
            MessageModel aa = new MessageModel();
            aa.Message = "sdsdsd";
            ad.Messages.Add(aa);
        }

        private void Message_KeyDown(object sender, KeyEventArgs e)
        {
            //ignore if not return key
            if (e.Key != Key.Return)
                return;

            //make sure obj is instantiated
            ViewModelCheck();

            //Add message to the view model and clear textbox
            _viewModel.AddMessageToChat(new MessageModel
            {
                Username = username,
                UsernameColor = profileColor,
                ImageSource = profilePicture,
                Message = _viewModel.Message,
                Time = DateTime.Now,
                IsFromMe = false,
                FirstMessage = true
            });
            _viewModel.Message = "";
        }

    }
}
