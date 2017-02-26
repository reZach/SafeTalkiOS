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
    [Register ("SignalrConnectViewController")]
    partial class SignalrConnectViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton goToMainApp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView initializationProgress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel progressLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (goToMainApp != null) {
                goToMainApp.Dispose ();
                goToMainApp = null;
            }

            if (initializationProgress != null) {
                initializationProgress.Dispose ();
                initializationProgress = null;
            }

            if (progressLabel != null) {
                progressLabel.Dispose ();
                progressLabel = null;
            }
        }
    }
}