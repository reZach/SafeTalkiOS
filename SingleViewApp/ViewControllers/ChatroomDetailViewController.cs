using Foundation;
using Newtonsoft.Json;
using SafeTalkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UIKit;

namespace SingleViewApp
{
    public partial class ChatroomDetailViewController : UIViewController
    {
        public ChatroomDetailViewController(IntPtr handle) : base(handle)
        {            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Load chatroom name, from previous viewcontroller
            chatroomNameLabel.Text = ChatroomName;
            Title = ChatroomName;

            // Load table of users
            UsersInChatroomTableView.Source = new UsersInChatroomTableSource(
                AllUsersInChatroom(GetCurrentChatroom(), GetAllUsers()));
        }

        public string ChatroomName { get; set; }
        private string GetAllUsersURI { get { return "https://safetalkapi.azurewebsites.net/api/user/list"; } }
        private string GetCurrentChatroomURI { get { return "https://safetalkapi.azurewebsites.net/api/chatroom/get?name={name}"; } }

        private List<User> AllUsersInChatroom(Chatroom chatroom, List<User> allUsers)
        {
            List<User> usersInChatroom = new List<User>();

            foreach (string userGuid in chatroom.UserGuids)
            {
                usersInChatroom.Add(allUsers.Find(x => x.Guid == userGuid));
            }

            return usersInChatroom;
        }

        private Chatroom GetCurrentChatroom()
        {
            string URI = GetCurrentChatroomURI.Replace("{name}", WebUtility.UrlEncode(ChatroomName));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);
            Chatroom chatroom = new Chatroom();

            try
            {
                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream dataStream = webResponse.GetResponseStream();

                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        string responseJson = reader.ReadToEnd();
                        chatroom = JsonConvert.DeserializeObject<Chatroom>(responseJson);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return chatroom;
        }

        private List<User> GetAllUsers()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetAllUsersURI);
            List<User> users = new List<User>();

            try
            {
                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream dataStream = webResponse.GetResponseStream();

                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        string responseJson = reader.ReadToEnd();
                        users = JsonConvert.DeserializeObject<List<User>>(responseJson);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return users;
        }
    }

    public class UsersInChatroomTableSource : UITableViewSource
    {
        List<User> Users;
        string CellIdentifier = "UserCell";

        public UsersInChatroomTableSource(List<User> users)
        {
            Users = users;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Users.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            string username = Users[indexPath.Row].PublicName;

            // If there are no cells to reuse, create a new one
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
            }

            cell.TextLabel.Text = username;

            return cell;
        }
    }
}