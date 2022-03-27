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
        public virtual string ChatName { get; set; }
        public string ImageSource { get; set; }
        public string UsernameColor { get; set; }
        public string Endpoint { get; set; }
        private ObservableCollection<MessageModel> _messages;
        public ObservableCollection<MessageModel> Messages {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }
        private string _LastMessage;

        public virtual string LastMessage
        {
            get { return (Messages != null) ? PreviewTruncated(Messages.Last()) : ""; }
            set { _LastMessage = value;
                OnPropertyChanged();
            }
        }

        private string PreviewTruncated(MessageModel model)
        {
            string recentMsg = model.Message;
            MessageLimiter();
            if (recentMsg.Length > 15)
            {
                recentMsg = recentMsg.Substring(0, 15);
                return model.Username + ": " + recentMsg + "...";
            }
            return model.Username + ": " + recentMsg;
        }

        private void MessageLimiter()
        {
            if (Messages.Count > 100)
            {
                Messages.Remove(Messages.First());
            }
        }
    }
}
