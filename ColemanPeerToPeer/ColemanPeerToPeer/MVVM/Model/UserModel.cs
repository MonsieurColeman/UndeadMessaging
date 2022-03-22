using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColemanPeerToPeer.MVVM.Model
{
    public class UserModel
    {
        public string Username { get; set; }
        public string ChatName { get; set; }
        public string ImageSource { get; set; }
        public ObservableCollection<MessageModel> Messages { get; set; }
        public string LastMessage => Messages.Last().Message;
    }


    /*
         public class UserModel : ObservableObject
    {
        public string Username { get; set; }
        public string ChatName { get; set; }
        public string ImageSource { get; set; }
        private ObservableCollection<MessageModel> _messages;

        public ObservableCollection<MessageModel> Messages
        {
            get { return _messages; }
            set { _messages = value;
                OnPropertyChanged();
                
            }
        }


        public string LastMessage => _messages.Last().Message;
    }
     
     */
}
