using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;

namespace ServiceOutliner
{
    public static class Converter
    {
        public static string MessageProtocolToJSON(MessageProtocol mP)
        {
            //use extension
            var serializer = new JavaScriptSerializer();

            //serialize
            var serializedResult = serializer.Serialize(mP);

            //return
            return serializedResult;
        }

        public static MessageProtocol MessageProtocolToJSON(string JSON)
        {
            var serializer = new JavaScriptSerializer();
            var deserializedResult = serializer.Deserialize<MessageProtocol>(JSON);
            return deserializedResult;
        }

        public static UserModel TopicModelToUserModel(TopicModel tm)
        {
            return new UserModel
            {
                Username = tm.ChatName,
                ChatName = tm.ChatName,
                Endpoint = tm.Endpoint,
                ImageSource = tm.ImageSource,
                LastMessage = tm.LastMessage,
                Messages = tm.Messages,
                UsernameColor = tm.UsernameColor,
            };
        }

        public static TopicModel UserModelToTopicModel(UserModel um)
        {
            return new TopicModel
            {
                Username = um.ChatName,
                ChatName = um.ChatName,
                Endpoint = um.Endpoint,
                ImageSource = um.ImageSource,
                LastMessage = um.LastMessage,
                Messages = um.Messages,
                UsernameColor = um.UsernameColor,
            };
        }

        public static ObservableCollection<UserModel> TopicListToUserList(ObservableCollection<TopicModel> topics)
        {
            ObservableCollection<UserModel> models = new ObservableCollection<UserModel>();
            foreach (var topic in topics)
                models.Add(TopicModelToUserModel(topic));
            return models;
        }

        public static ObservableCollection<TopicModel> UserListToTopicList(ObservableCollection<UserModel> users)
        {
            ObservableCollection<TopicModel> models = new ObservableCollection<TopicModel>();
            foreach (var user in users)
                models.Add(UserModelToTopicModel(user));
            return models;
        }

    }
}
