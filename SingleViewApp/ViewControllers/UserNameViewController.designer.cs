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
    [Register ("ChatRoomsViewController")]
    partial class ChatRoomsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel userPublicName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (userPublicName != null) {
                userPublicName.Dispose ();
                userPublicName = null;
            }
        }
    }
}