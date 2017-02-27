using Foundation;
using Newtonsoft.Json;
using SafeTalkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UIKit;

namespace SingleViewApp
{
    public partial class ChatroomsListTableViewController : UITableViewController
    {
        public ChatroomsListTableViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            Title = "Chatrooms";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Load chatroom data
            List<Chatroom> chatrooms = GetChatrooms();
            TableView.Source = new ChatroomsListTableSource(chatrooms, this);

            // Add create chatroom button
            CreateAddChatroomButton();
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            var chatroomDetailViewController = segue.DestinationViewController as ChatroomDetailViewController;

            if (chatroomDetailViewController != null)
            {
                chatroomDetailViewController.ChatroomName = SelectedChatroomName;
            }
        }

        [Action("UnwindToChatroomsListTableViewController:")]
        public void UnwindToChatroomsListTableViewController(UIStoryboardSegue segue)
        {
        }

        public string SelectedChatroomName { get; set; }
        private string GetChatroomsURI { get { return "https://safetalkapi.azurewebsites.net/api/chatroom/list"; } }

        private List<Chatroom> GetChatrooms()
        {
            // https://msdn.microsoft.com/en-us/library/456dfw4f(v=vs.110).aspx#Anchor_0
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetChatroomsURI);
            List<Chatroom> chatrooms = new List<Chatroom>();

            try
            {
                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream dataStream = webResponse.GetResponseStream();

                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        string responseJson = reader.ReadToEnd();
                        chatrooms = JsonConvert.DeserializeObject<List<Chatroom>>(responseJson);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return chatrooms;
        }

        private void CreateAddChatroomButton()
        {
            var viewController = this;

            // https://developer.xamarin.com/recipes/ios/content_controls/navigation_controller/add_a_nav_bar_right_button/
            NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender, args) =>
                {
                    // CreateChatroomSegue
                    viewController.PerformSegue("CreateChatroomSegue", viewController);
                }), true);
        }
    }

    // https://developer.xamarin.com/guides/ios/user_interface/tables/part_2_-_populating_a_table_with_data/
    public class ChatroomsListTableSource : UITableViewSource
    {
        List<Chatroom> Chatrooms;
        string CellIdentifier = "ChatroomCell";
        ChatroomsListTableViewController ParentViewController;

        public ChatroomsListTableSource(List<Chatroom> chatrooms, ChatroomsListTableViewController parent)
        {
            Chatrooms = chatrooms;
            ParentViewController = parent;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Chatrooms.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            string item = Chatrooms[indexPath.Row].PublicName;

            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
            }
            
            cell.TextLabel.Text = item;

            return cell;
        }

        // Move to the detail view controller if we click on a table cell
        // https://developer.xamarin.com/guides/ios/getting_started/hello,_iOS_multiscreen/hello,_iOS_multiscreen_deepdive/
        // https://forums.xamarin.com/discussion/19855/send-segue-from-uitable-selectedrow
        // http://stackoverflow.com/a/18166148/1837080
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var chatroomName = Chatrooms[indexPath.Row].PublicName;
            ParentViewController.SelectedChatroomName = chatroomName;

            ParentViewController.PerformSegue("ChatroomListToDetailSegue", ParentViewController);
        }
    }
}