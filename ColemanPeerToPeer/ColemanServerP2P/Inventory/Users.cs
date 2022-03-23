using ServiceOutliner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColemanServerP2P
{
    public static class UserList
    {

        public static Dictionary<string, UserModel> _list_of_users = new Dictionary<string, UserModel>();

        public static void AddUser(UserModel user)
        {
            _list_of_users.Add(user.Username, user);
        }

        public static string GetEndpoint(string username)
        {
            return _list_of_users[username].Endpoint;
        }

        public static int GetNumberOfUsers()
        {
            return _list_of_users.Count;
        }

        public static ObservableCollection<UserModel> GetCurrentUsers()
        {
            ObservableCollection<UserModel> users = new ObservableCollection<UserModel>();
            for (int i = 0; i < _list_of_users.Count; i++)
                users.Add(_list_of_users.ElementAt(i).Value);
            return users;
        }

        public static bool UniqueUserCheck(string username)
        {
            Console.WriteLine("before add: "+ _list_of_users.Count.ToString());
            if (_list_of_users.Count > 0)
                return !(_list_of_users.ContainsKey(username));
            else
                return true;
        }
    }
}
