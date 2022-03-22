using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColemanServerP2P
{
    public static class UserList
    {
        //public static List<string> _list_of_users = new List<string>();

        public static Dictionary<string, string> _list_of_users = new Dictionary<string, string>();

        public static void AddUser((string, string) user)
        {
            //item1: username
            //item2: endpoint
            _list_of_users.Add(user.Item1, user.Item2);
        }

        public static string GetEndpoint(string username)
        {
            return _list_of_users[username];
        }

        public static int GetNumberOfUsers()
        {
            return _list_of_users.Count;
        }

        public static Dictionary<string,string> GetCurrentUsers()
        {
            return _list_of_users;
        }

        public static bool UniqueUserCheck(string username)
        {
            if (_list_of_users.Count > 0)
                return _list_of_users.ContainsKey(username);
            else
                return true;
        }
    }

    public static class TopicList
    {
        public static Dictionary<string, List<string>> _list_of_topics = new Dictionary<string, List<string>>();

        public static void AddUserToTopic(string topicName, string username)
        {
            _list_of_topics[topicName].Add(username);
        }

        public static void RemoveUserFromTopic(string topicName, string username)
        {
            _list_of_topics[topicName].Remove(username);
        }

        public static int GetNumberOfTopics()
        {
            return _list_of_topics.Count;
        }

        public static List<string> GetCurrentTopics()
        {
            return _list_of_topics.Keys.ToList();
        }

        public static bool UniqueTopicCheck(string TopicName)
        {
            return _list_of_topics.ContainsKey(TopicName);
        }
    }
}
