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
            get { return (Messages != null) ? Messages.Last().Message : ""; }
            set
            {
                _LastMessage = value;
                OnPropertyChanged();
            }
        }
        
    }
}
