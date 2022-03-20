using ColemanPeerToPeer.Core;
using ColemanPeerToPeer.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ColemanPeerToPeer.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public  ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }

        /* Commands */
        public RelayCommand SendCommand { get; set; }

        private UserModel _selectedChat;

        public UserModel SelectedChat
        {
            get { return _selectedChat; }
            set {
                _selectedChat = value; 
                OnPropertyChanged();
            }
        }


        private string _message;

        public string Message
        {
            get { return _message; }
            set { 
                _message = value; 
                OnPropertyChanged();
            }
        }

        public string userNameColor;

        public MainViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            Users = new ObservableCollection<UserModel>();
            userNameColor = "#000000";
            int users = new Random().Next(0, 100);

            SendCommand = new RelayCommand(o =>
            {
                Messages.Add(new MessageModel
                {
                    Message = Message,
                    FirstMessage = false
                });
                MessageBox.Show("asd");
                Message = "";
            });

            Messages.Add(new MessageModel
            {
                Username = "Jeff",
                UsernameColor = userNameColor,
                ImageSource = "https://picsum.photos/200/300",
                Message = "Want to Handout",
                Time = DateTime.Now,
                IsFromMe = false,
                FirstMessage = true
            });

            for (int i = 0; i < users; i++)
            {
                Messages.Add(new MessageModel
                {
                    Username = "Jeff",
                    UsernameColor = userNameColor,
                    ImageSource = "https://picsum.photos/200/300",
                    Message = "Hey!",
                    Time = DateTime.Now,
                    IsFromMe = true,
                    FirstMessage = false
                });
            }
            for (int i = 0; i < 4; i++)
            {
                Messages.Add(new MessageModel
                {
                    Username = "NotJeff",
                    UsernameColor = userNameColor,
                    ImageSource = "https://picsum.photos/200/300",
                    Message = "Hey!",
                    Time = DateTime.Now,
                    IsFromMe = true,
                });
            }

            Messages.Add(new MessageModel
            {
                Username = "NotJeff",
                UsernameColor = userNameColor,
                ImageSource = "https://picsum.photos/200/300",
                Message = "Laiouhiuhst",
                Time = DateTime.Now,
                IsFromMe = true,
            });

            for (int i = 0; i < 5; i++)
            {
                Users.Add(new UserModel
                {
                    Username = $"Nigel {i}",
                    ImageSource = "https://picsum.photos/200/300",
                    Messages = Messages
                });
            }



        }
    }
}
