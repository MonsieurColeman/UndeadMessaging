using ServiceOutliner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColemanServerP2P
{
    public static class TopicList //toDo, handle usermodel compatibility
    {
        public static Dictionary<TopicModel, List<UserModel>> _list_of_topics = new Dictionary<TopicModel, List<UserModel>>();

        public static void AddUserToAllTopic(UserModel user)
        {
            for (int i = 0; i < _list_of_topics.Count; i++)
            {
                KeyValuePair<TopicModel, List<UserModel>> TopicItem = _list_of_topics.ElementAt(i);
                if (!TopicItem.Value.Contains(user))
                    TopicItem.Value.Remove(user);
            }
        }

        public static void RemoveUserFromTopic(string topicName, UserModel user)
        {
            for (int i = 0; i < _list_of_topics.Count; i++)
            {
                KeyValuePair<TopicModel, List<UserModel>> TopicItem = _list_of_topics.ElementAt(i);
                if (TopicItem.Key.TopicName == topicName)
                {
                    if (TopicItem.Value.Contains(user))
                        TopicItem.Value.Remove(user);
                    if (TopicItem.Value.Count == 0)
                        RemoveTopic(TopicItem.Key);
                }
            }
        }

        public static void RemoveUserFromTopic(TopicModel topic, UserModel user)
        {
            for (int i = 0; i < _list_of_topics.Count; i++)
            {
                KeyValuePair<TopicModel, List<UserModel>> TopicItem = _list_of_topics.ElementAt(i);
                if (TopicItem.Key.TopicName == topic.TopicName)
                {
                    if (TopicItem.Value.Contains(user))
                        TopicItem.Value.Remove(user);
                    if (TopicItem.Value.Count == 0)
                        RemoveTopic(TopicItem.Key);
                }

            }
        }

        public static void RemoveUserFromAllTopics(UserModel user)
        {
            for (int i = 0; i < _list_of_topics.Count; i++)
            {
                KeyValuePair<TopicModel, List<UserModel>> TopicItem = _list_of_topics.ElementAt(i);
                if (TopicItem.Value.Contains(user))
                    TopicItem.Value.Remove(user);
                if(TopicItem.Value.Count == 0)
                    RemoveTopic(TopicItem.Key);
            }
        }

        public static int GetNumberOfTopics()
        {
            return _list_of_topics.Count;
        }

        public static ObservableCollection<TopicModel> GetCurrentTopics()
        {
            ObservableCollection<TopicModel> topics = new ObservableCollection<TopicModel>();
            for (int i = 0; i < _list_of_topics.Count; i++)
                topics.Add(_list_of_topics.ElementAt(i).Key);
            return topics;
        }

        public static bool UniqueTopicCheck(string TopicName)
        {
            for (int i = 0; i < _list_of_topics.Count; i++)
                if (_list_of_topics.ElementAt(i).Key.TopicName == TopicName)
                    return false;
            return true;
        }

        public static TopicModel GetTopicFromTopicName(string topicName)
        {
            for(int i = 0; i < _list_of_topics.Count; i++)
                if (_list_of_topics.ElementAt(i).Key.TopicName == topicName)
                    return _list_of_topics.ElementAt(i).Key;
            return null;
        }

        private static bool UserListContains (TopicModel topic, UserModel user)
        {
            List<UserModel> userList = _list_of_topics[topic];
            foreach (UserModel _user in userList)
                if (_user.Endpoint == user.Endpoint)
                    return true;
            return false;
        }

        public static void AddTopic (TopicModel topic)
        {
            _list_of_topics.Add(topic, UserList.GetCurrentUsers().ToList());
        }

        public static void RemoveTopic(TopicModel topic)
        {
            _list_of_topics.Remove(topic);
        }

        public static List<UserModel> TopicReceivedMsg(TopicModel topic, MessageModel msg)
        {
            if(topic.Messages == null)
                topic.Messages = new ObservableCollection<MessageModel>() { msg };
            else
                topic.Messages.Add(msg);

            if (topic.Messages.Count > 100)
                topic.Messages.Remove(topic.Messages.First());

            return GetUserListOfTopic(topic);
        }

        private static TopicModel GetTopicFromTopic(TopicModel topic)
        {
            foreach (KeyValuePair<TopicModel,List<UserModel>> topicItem in _list_of_topics)
                if (topicItem.Key.ChatName == topic.ChatName)
                    return topic;
            return null;
        }

        private static List<UserModel> GetUserListOfTopic(TopicModel topic)
        {
            foreach (KeyValuePair<TopicModel, List<UserModel>> topicItem in _list_of_topics)
                if (topicItem.Key.ChatName == topic.ChatName)
                    return topicItem.Value;
            return null;
        }
    }
}
