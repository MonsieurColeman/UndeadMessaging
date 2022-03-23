using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColemanServerP2P
{
    public static class TopicList //toDo, handle usermodel compatibility
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
