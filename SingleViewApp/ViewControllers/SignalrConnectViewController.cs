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
    public partial class SignalrConnectViewController : UIViewController
    {        
        public SignalrConnectViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Initializing";
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (!UserInformationExistOnDevice())
            {                
                // Can only call ui-updating functions within the ui
                // thread; https://developer.xamarin.com/guides/ios/user_interface/controls/part_2_-_working_with_the_ui_thread/
                InvokeOnMainThread(async () => await CreateNewUser());
            }
            else
            {
                InvokeOnMainThread(async () => await SetNewRandomName());
            }   
        }

        private string CreateNewUserURI { get { return "https://safetalkapi.azurewebsites.net/api/user/new"; } }        
        private string SaveUserURI { get { return "https://safetalkapi.azurewebsites.net/api/user/save?guid={guid}&publicname={publicname}"; } }
        private string GetRandomNameURI { get { return "https://safetalkapi.azurewebsites.net/api/user/newname?guid={guid}"; } }

        private async void DelayAndNavigateToTheMainApp()
        {
            UpdateProgress("Complete", 1f);
            await Task.Delay(2500);

            // Can't figure out how to transition to another view,
            // so instead going to click the button to go to the next view
            goToMainApp.SendActionForControlEvents(UIControlEvent.TouchUpInside);
        }

        /// <summary>
        /// Updates the update message and progress bar
        /// </summary>
        /// <param name="message"></param>
        /// <param name="percent"></param>
        private void UpdateProgress(string message = "", float percent = -1.0f)
        {
            if (!string.IsNullOrEmpty(message))
            {
                progressLabel.Text = message;
            }
            if (percent >= 0.0f) {
                initializationProgress.SetProgress(percent, true);
            }            
        }

        /// <summary>
        /// Checks if user information is saved on device
        /// </summary>
        /// <returns></returns>
        private bool UserInformationExistOnDevice()
        {
            // http://stackoverflow.com/a/32642766/1837080
            string guid = NSUserDefaults.StandardUserDefaults.StringForKey("guid");
            string publicName = NSUserDefaults.StandardUserDefaults.StringForKey("name");

            if (string.IsNullOrEmpty(guid) || string.IsNullOrEmpty(publicName))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Requests a new user object from the api
        /// </summary>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        private async Task<bool> CreateNewUserHelper(HttpClient httpClient)
        {
            string responseJson = null;
            bool success = false;

            // Query api
            Task<HttpResponseMessage> getResponse = httpClient.GetAsync(CreateNewUserURI);
            HttpResponseMessage response = await getResponse;

            if (!response.IsSuccessStatusCode)
            {
                return success;
            }
            else
            {
                success = true;
            }

            // Parse response
            responseJson = await response.Content.ReadAsStringAsync();
            User newUser = JsonConvert.DeserializeObject<User>(responseJson);

            // Save values on device
            NSUserDefaults.StandardUserDefaults.SetString(newUser.Guid, "guid");
            NSUserDefaults.StandardUserDefaults.SetString(newUser.PublicName, "name");

            return success;
        }

        /// <summary>
        /// Posts and saves a new random name for the user
        /// </summary>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        private async Task<bool> SetRandomNameHelper(HttpClient httpClient)
        {
            bool success = false;

            string guidFromDevice = NSUserDefaults.StandardUserDefaults.StringForKey("guid");
            string URI = GetRandomNameURI.Replace("{guid}", WebUtility.UrlEncode(guidFromDevice));

            // Query api
            Task<HttpResponseMessage> getResponse = httpClient.PostAsync(URI, null);
            HttpResponseMessage response = await getResponse;

            if (!response.IsSuccessStatusCode)
            {
                return success;
            }
            else
            {
                success = true;
            }

            // Parse response
            string newName = await response.Content.ReadAsStringAsync();

            // Save values on device
            NSUserDefaults.StandardUserDefaults.SetString(newName, "name");

            return success;
        }

        /// <summary>
        /// Sends a save request, to save the user, through the api
        /// </summary>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        private async Task<bool> SaveUserHelper(HttpClient httpClient)
        {
            bool success = false;

            string guidFromDevice = NSUserDefaults.StandardUserDefaults.StringForKey("guid");
            string nameFromDevice = NSUserDefaults.StandardUserDefaults.StringForKey("name");               
            string URI = SaveUserURI.Replace("{guid}", WebUtility.UrlEncode(guidFromDevice)).Replace("{publicname}", WebUtility.UrlEncode(nameFromDevice));

            // Query API
            Task<HttpResponseMessage> getResponse = httpClient.PostAsync(URI, null);
            HttpResponseMessage response = await getResponse;

            if (!response.IsSuccessStatusCode)
            {
                return success;
            }
            return true;
        }


        /// <summary>
        /// Communicates with the API to generate
        /// and save a new user
        /// </summary>
        /// <returns></returns>
        private async Task CreateNewUser()
        {
            bool success = false;
            using (var httpClient = new HttpClient())
            {
                try
                {
                    UpdateProgress("Creating random user...", 0.15f);

                    // Creates a new user
                    success = await CreateNewUserHelper(httpClient);

                    if (!success)
                    {
                        UpdateProgress("Could not create random user", 0f);
                        return;
                    }
                    else
                    {
                        // Save new user details to server
                        success = await SaveUserHelper(httpClient);

                        if (!success)
                        {
                            UpdateProgress("Error occurred, please try again", 0f);
                            return;
                        }
                        else
                        {
                            UpdateProgress("Random user saved", 0.8f);
                            DelayAndNavigateToTheMainApp();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Communicates with the API to set a new
        /// random name for the user; and save their details
        /// </summary>
        /// <returns></returns>
        private async Task SetNewRandomName()
        {
            bool success = false;
            using (var httpClient = new HttpClient())
            {
                try
                {
                    UpdateProgress("Generating random name...", 0.15f);

                    // Generate a new random name
                    success = await SetRandomNameHelper(httpClient);

                    // Guid was not found; will create a new user instead
                    if (!success)
                    {
                        success = await CreateNewUserHelper(httpClient);

                        if (!success)
                        {
                            UpdateProgress("Error occurred, please try again", 0f);
                            return;
                        }
                        else
                        {
                            // Save new user details to server
                            success = await SaveUserHelper(httpClient);

                            if (!success)
                            {
                                UpdateProgress("Error occurred, please try again", 0f);
                                return;
                            }
                            else
                            {
                                UpdateProgress("Generated random name", 0.8f);
                                DelayAndNavigateToTheMainApp();
                            }
                        }
                    }
                    else
                    {
                        // We set our random name; success
                        UpdateProgress("Generated random name", 0.8f);
                        DelayAndNavigateToTheMainApp();
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