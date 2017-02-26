using Foundation;
using Newtonsoft.Json;
using SafeTalkCore;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UIKit;

namespace SingleViewApp
{
    public partial class UserNameViewController : UIViewController
    {
        public UserNameViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            InvokeOnMainThread(async () => await GetUserName());
        }

        private string GetExistingUserURI { get { return "https://safetalkapi.azurewebsites.net/api/user/get?guid={guid}"; } }

        private async Task<string> GetUserNameHelper(HttpClient httpClient)
        {
            string guidFromDevice = NSUserDefaults.StandardUserDefaults.StringForKey("guid");
            string URI = GetExistingUserURI.Replace("{guid}", WebUtility.UrlEncode(guidFromDevice));

            // Query API
            Task<HttpResponseMessage> getResponse = httpClient.GetAsync(URI);
            HttpResponseMessage response = await getResponse;

            if (!response.IsSuccessStatusCode)
            {
                return "";
            }

            // Parse
            string responseJson = await response.Content.ReadAsStringAsync();
            User user = JsonConvert.DeserializeObject<User>(responseJson);

            return user.PublicName;
        }

        private async Task GetUserName()
        {
            bool success = false;
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string userName = await GetUserNameHelper(httpClient);

                    if (string.IsNullOrEmpty(userName))
                    {
                        // Error, could not get name
                    }
                    else
                    {
                        userPublicName.Text = userName;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}