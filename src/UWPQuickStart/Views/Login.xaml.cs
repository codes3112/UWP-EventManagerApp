using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPQuickStart.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using winsdkfb;
using winsdkfb.Graph;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPQuickStart.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public string SId = "Your_Machine_Sid";
        public string AppId = "";
        private readonly string[] requested_permissions =
                                    {
                                    "public_profile",
                                    "email"
                                    };
        MyDatabase db;

        private async void fbLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            
            FBSession session = FBSession.ActiveSession;
            session.WinAppId = SId;
            session.FBAppId = AppId;
            //List<string> permissionList = new List<string>();
            //permissionList.Add("public_profile");
            //permissionList.Add("email");
            FBPermissions permissions = new FBPermissions(requested_permissions);
            var result = await session.LoginAsync(permissions);
            if (result.Succeeded)
            {
                string name = session.User.Name;
                Debug.WriteLine(session.User.Id);
                Debug.WriteLine(session.User.Name);
                Frame.Navigate(typeof(EventMainPage));
            }
            else
            {
                //Login failed
            }


        }
        private async void OnGet(object sender, RoutedEventArgs e)
        {
            string endpoint = "/me";
            PropertySet parameters = new PropertySet();
            parameters.Add("fields", "email");
            FBSingleValue value = new FBSingleValue(endpoint, parameters, User.FromJson);
            FBResult graphResult = await value.GetAsync();
            if (graphResult.Succeeded)
            {
                User profile = graphResult.Object as User;
                string email = profile?.Email;
                MessageDialog dialog = new MessageDialog(email);
                await dialog.ShowAsync();
            }
        }
        public Login()
        {
            this.InitializeComponent();
            db = new MyDatabase();
        }      
       

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Register));
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (db.Login(txtUser.Text, txtPassword.Password))
            {
                var username = txtUser.Text;
                var message = new MessageDialog("Login success");
                await message.ShowAsync();
                App.username_sqlite = txtUser.Text;
                (App.Current as App).NavigateUserNameText = txtUser.Text;
                Frame.Navigate(typeof(EventMainPage));
            }
            else
            {
                var message = new MessageDialog("Login failed");
                await message.ShowAsync();
            }
        }

    }
}
