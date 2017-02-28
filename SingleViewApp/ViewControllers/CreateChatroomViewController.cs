using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace SingleViewApp
{
    public partial class CreateChatroomViewController : UIViewController
    {
        public CreateChatroomViewController (IntPtr handle) : base (handle)
        {
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            var chatroomChatController = segue.DestinationViewController as ChatroomChatViewController;

            if (chatroomChatController != null)
            {
                ViewControllerStack.Add(this);
                chatroomChatController.ViewControllerStack = ViewControllerStack;
            }
        }

        public List<UIViewController> ViewControllerStack { get; set; }
    }
}