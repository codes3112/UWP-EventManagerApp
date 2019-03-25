// Copyright (c) Microsoft. All rights reserved
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWPQuickStart.Utils;
using UWPQuickStart.Views;
using System;
using winsdkfb;
using Facebook;
using Facebook.Client;

namespace UWPQuickStart
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventMainPage : Page
    {
        string AccessToken = "Your_Token";
        //Declare the top level nav items
        private readonly List<NavMenuItem> _navList = new List<NavMenuItem>(
            new[]
            {
                new NavMenuItem
                {
                    Symbol = Symbol.Home,
                    Label = "HOME",
                    DestPage = typeof (EventHome)
                },
                new NavMenuItem
                {
                    Symbol = Symbol.Directions,
                    Label = "EVENT DETAILS",
                    DestPage = typeof (EventDetails)
                },
                new NavMenuItem
                {
                    Symbol = Symbol.Camera,
                    Label = "PHOTOS",
                    DestPage = typeof (Photos)
                },
                 new NavMenuItem
                {
                    Symbol = Symbol.Undo,
                    Label = "LOGOUT",
                    DestPage = typeof (Login)
                }
            });

        public EventMainPage()
        {
            InitializeComponent();
            navMenuList.ItemsSource = _navList;
            DataContext = App.EventModel;
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }

        internal Rect TogglePaneButtonRect { get; set; }

        /// <summary>
        ///     Callback when the SplitView's Pane is toggled open or close.  When the Pane is not visible
        ///     then the floating hamburger may be occluding other content in the app unless it is aware.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogglePaneButton_Checked(object sender, RoutedEventArgs e)
        {
            CheckTogglePaneButtonSizeChanged();
        }

        /// <summary>
        ///     Check for the conditions where the navigation pane does not occupy the space under the floating
        ///     hamburger button and trigger the event.
        /// </summary>
        private void CheckTogglePaneButtonSizeChanged()
        {
            AppNavigationUtil.SplitViewPaneHandler(this, rootSplitView, TogglePaneButton);
            TogglePaneButtonRectChanged?.DynamicInvoke(this, TogglePaneButtonRect);
        }

        /// <summary>
        ///     An event to notify listeners when the hamburger button may occlude other content in the app.
        ///     The custom "PageHeader" user control is using this.
        /// </summary>
        internal event TypedEventHandler<EventMainPage, Rect> TogglePaneButtonRectChanged;

        private void NavMenu_ItemClickHandler(object sender, ItemClickEventArgs e)
        {
            var destPage = (e.ClickedItem as NavMenuItem)?.DestPage;
            
             if(destPage.Name == "Login")
             {
             try
              {
                    FBSession session = FBSession.ActiveSession;                   
                  

                    var myFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
                    var cookieManager = myFilter.CookieManager;
                    var myCookieJar=cookieManager.GetCookies(new Uri("https://facebook.com"));
                    foreach(var cookie in myCookieJar)
                    {
                        cookieManager.DeleteCookie(cookie);
                    }        
                    
                        GetLogoutURL(AccessToken);                      
                        Frame.Navigate(typeof(Login));
                        App.EventModel.EventAddress = "";
                        App.EventModel.EventName = "";
                        App.EventModel.EventStartTime = new DateTime(0000, 01, 1, 01, 0, 0);
                        App.username_sqlite = "";

                        //Application.Current.Exit();




                }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
            AppNavigationUtil.SetSplitViewContent(rootSplitView, destPage, true);
            rootSplitView.IsPaneOpen = false;
        }

        public static string GetLogoutURL(string AccessToken)
        {
            FBSession Session = FBSession.ActiveSession;
            if (Session != null)
            {
                var fb = new FacebookClient();

                var logoutUrl = fb.GetLogoutUrl(new { access_token = AccessToken, next = "https://www.facebook.com/connect/login_success.html" });
                
                return logoutUrl.ToString();

            }
            return null;
        }

       
        

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            AppNavigationUtil.SetSplitViewContent(rootSplitView, null, false);
        }
    }
}