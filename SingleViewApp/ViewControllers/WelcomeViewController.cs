using Foundation;
using System;
using UIKit;

namespace SingleViewApp
{
    public partial class WelcomeViewController : UIViewController
    {
        public WelcomeViewController (IntPtr handle) : base (handle)
        {
        }

        [Action("UnwindToWelcomeViewController:")]
        public void UnwindToWelcomeViewController(UIStoryboardSegue segue)
        {
        }
    }
}