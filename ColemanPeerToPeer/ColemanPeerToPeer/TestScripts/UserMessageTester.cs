using ColemanPeerToPeer.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColemanPeerToPeer.TestScripts
{
    public static class UserMessageTester
    {
        /* 
         * Data to create test user and test user messages from
         */

        private static Random random = new Random();
        private static string userNameColor = "#000000"; //black
        private static string randomProfilePicture = "https://picsum.photos/200/300";
        private static List<string> _userNames = new List<string>() { "Phillip","Billie","Matty","Sarah","Bertha" };
        private static List<string> messages = new List<string>()
        {
                "Hey, want to go to the bar",
                "I saw this really nice hat today",
                "bread",
                "Can I borrow a dollar",
                "Tape is sticky",
                "Water is wet",
                "Water is not wet",
                "The dress is blue",
                "The dress is not blue",
                "Are you going to the game this weekend?",
                "So you're not going to give me a ride?",
                "So you're not going to lemme borrow a dolla?",
                "Do you think adderal will increase my productivity?",
                "Don't do drugs"
        };


        /* Public method for other classes to interface with */
        public static ObservableCollection<UserModel> GetUsers()
        {
            return CreateUsers();
        }

        /* 
         * Returns a list a list of generated messages in the type of
         * Observable Collection which conceptually represents a chat
         */
        private static ObservableCollection<MessageModel> GetMessages(string user, int num)
        {
            ObservableCollection<MessageModel> messages = new ObservableCollection<MessageModel>();
            for (int i = 0; i < num; i++)
            {
                messages.Add(CreateMessage(user, GetSampleText()));
            }
            return messages;
        }

        /*
         * Creates a test chat message object to be added to a chat collection object
         * based on a user name and text
         */
        public static MessageModel CreateMessage(string user, string s)
        {
            return new MessageModel
            {
                Username = user,
                UsernameColor = userNameColor,
                ImageSource = randomProfilePicture,
                Message = s,
                Time = DateTime.Now,
                IsFromMe = false,
                FirstMessage = true
            };
        }

        /*
         * Returns a random string based on the list of sample strings
         * defined privately within the class
         */
        private static string GetSampleText()
        {
            int randomInt = random.Next(0,messages.Count);
            return messages[randomInt];
        }

        /*
         * Returns a list of users where...
         * Usernames are randomly Selected
         * Chats are randomly selected
         */
        private static ObservableCollection<UserModel> CreateUsers()
        {
            ObservableCollection<UserModel> users = new ObservableCollection<UserModel>();
            for (int i = 0; i < _userNames.Count; i++)
            {
                users.Add(new UserModel
                {
                    Username = _userNames[i],
                    ImageSource = randomProfilePicture,
                    Messages = GetMessages(_userNames[i], random.Next(1, messages.Count))
                });
            }
            return users;
        }

        /* Display an example Topic
         */
        public static UserModel GetExampleTopic()
        {
            //Setup details for topic
            string topicName = "ExampleTopic";
            ObservableCollection<MessageModel> messages = new ObservableCollection<MessageModel>();

            //Create unique messages for topic chat
            for (int i = 0; i < _userNames.Count; i++)
            {
                messages.Add(new MessageModel
                {
                    Username = _userNames[i],
                    UsernameColor = userNameColor,
                    ImageSource = randomProfilePicture,
                    Message = GetSampleText(),
                    Time = DateTime.Now,
                    IsFromMe = false,
                    FirstMessage = true
                });
            }

            //Return topic to be seen as a test
            return new UserModel
            {
                Username = topicName,
                ImageSource = randomProfilePicture,
                Messages = messages
            };
        }
    }
}
