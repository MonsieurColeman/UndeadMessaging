using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ServiceOutliner.Private;

namespace ServiceOutliner
{
    public class TopicModel : UserModel
    {
        public string TopicName { get; set; }
        public new string Username { get; set; }
        public override string ChatName { get; set; }
        public new string ImageSource { get; set; }
        public new string UsernameColor { get; set; }
        public new string Endpoint { get; set; }
        public string ServerEndpoint { get; set; }
        private string _LastMessage;

        
        public override string LastMessage
        {
            get { return (Messages != null) ? PreviewTruncated(Messages.Last()) : ""; }
            set
            {
                _LastMessage = value;
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
