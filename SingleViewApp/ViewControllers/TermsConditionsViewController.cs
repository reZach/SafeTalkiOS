using Foundation;
using System;
using UIKit;

namespace SingleViewApp
{
    public partial class TermsConditionsViewController : UIViewController
    {
        public TermsConditionsViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Terms";
        }
    }
}