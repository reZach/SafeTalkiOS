using Foundation;
using SafeTalkCore;
using System;
using System.Collections.Generic;
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

            // Load table of users
            UsersInChatroomTableView.Source = new UsersInChatroomTableSource(new List<User>());
        }

        public string ChatroomName { get; set; }

        private List<>
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