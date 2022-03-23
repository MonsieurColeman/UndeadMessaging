using ServiceOutliner.Private;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceOutliner
{
    public class UserModel : ObservableObject
    {
        public string Username { get; set; }
        public string ChatName { get; set; }
        public string ImageSource { get; set; }
        public string UsernameColor { get; set; }
        public string Endpoint { get; set; }
        public ObservableCollection<MessageModel> Messages { get; set; }
        private string _LastMessage;

        public string LastMessage
        {
            get { return (Messages != null) ? Messages.Last().Message : ""; }
            set { _LastMessage = value;
                OnPropertyChanged();
            }
        }
    }
}
