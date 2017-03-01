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

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Creates button that takes us to a chatroom detail view
            CreateDetailButton();
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

        private void CreateDetailButton()
        {
            var viewController = this;

            // https://developer.xamarin.com/recipes/ios/content_controls/navigation_controller/add_a_nav_bar_right_button/
            NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(UIBarButtonSystemItem.Action, (sender, args) =>
                {
                    // CreateChatroomSegue
                    viewController.PerformSegue("ChatroomChatToDetailSegue", viewController);
                }), true);
        }
    }
}