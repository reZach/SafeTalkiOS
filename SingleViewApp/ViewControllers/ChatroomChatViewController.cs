using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using ObjCRuntime;

namespace SingleViewApp
{
    public partial class ChatroomChatViewController : UIViewController
    {
        public ChatroomChatViewController (IntPtr handle) : base (handle)
        {
        }

        // http://stackoverflow.com/questions/42498911/xamarin-ios-pop-multiple-view-controllers-on-back-button-click
        public override void DidMoveToParentViewController(UIViewController parent)
        {
            base.DidMoveToParentViewController(parent);
            
            // parent will be null when clicking the back button
            if (parent == null)
            {
                if (ViewControllerStack != null && ViewControllerStack.Count == 2)
                {
                    // Pops back 2 controllers
                    ViewControllerStack[ViewControllerStack.Count - 1].NavigationController.PopToViewController(ViewControllerStack[0], true);

                    // Refreshes view
                    ReloadInputViews();
                }
            }           
        }

        public List<UIViewController> ViewControllerStack { get; set; }
    }
}