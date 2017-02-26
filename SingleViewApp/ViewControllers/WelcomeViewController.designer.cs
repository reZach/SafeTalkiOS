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
    [Register ("WelcomeViewController")]
    partial class WelcomeViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton WelcomeEnterButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel WelcomeMessageLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (WelcomeEnterButton != null) {
                WelcomeEnterButton.Dispose ();
                WelcomeEnterButton = null;
            }

            if (WelcomeMessageLabel != null) {
                WelcomeMessageLabel.Dispose ();
                WelcomeMessageLabel = null;
            }
        }
    }
}