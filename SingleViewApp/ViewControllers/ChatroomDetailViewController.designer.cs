// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SingleViewApp
{
    [Register ("ChatroomDetailViewController")]
    partial class ChatroomDetailViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView UsersInChatroomTableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (UsersInChatroomTableView != null) {
                UsersInChatroomTableView.Dispose ();
                UsersInChatroomTableView = null;
            }
        }
    }
}