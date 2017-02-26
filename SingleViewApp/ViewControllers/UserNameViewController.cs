using Foundation;
using SafeTalkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UIKit;

namespace SingleViewApp
{
    public partial class ChatRoomsViewController : UIViewController
    {
        public ChatRoomsViewController (IntPtr handle) : base (handle)
        {            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            UpdateUserPublicName();
        }



        private void UpdateUserPublicName()
        {
            string publicName = NSUserDefaults.StandardUserDefaults.StringForKey("name");

            userPublicName.Text = publicName;
        }
    }
}