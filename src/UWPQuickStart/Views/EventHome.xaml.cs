// Copyright (c) Microsoft. All rights reserved
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UWPQuickStart.Models;
using UWPQuickStart.Utils;
using Windows.ApplicationModel.Appointments;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Security.Authentication.Web;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using winsdkfb;

namespace UWPQuickStart.Views
{
    public sealed partial class EventHome : UserControl
    {
        MyDatabase db;
        List<EventModel> _events;
        public EventHome()
        {
            db = new MyDatabase();
            _events = new List<EventModel>();

            string sid = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            Debug.WriteLine(sid);
            InitializeComponent();
            DataContext = App.EventModel;


            FBSession session = FBSession.ActiveSession;

            if (session != null && App.username_sqlite == null)
            {
                try
                {
                    string username = session.User.Name;
                    Debug.WriteLine("hi" + username);
                    usernameTxt.Text = "Hi " + username + " Welcome To the Event Manager";
                }
                catch
                {

                }
            }else
            {
                usernameTxt.Text = "Hi " + App.username_sqlite + " : Welcome To the Event Manager";
            }
           


            

            if (App.EventModel.EventAddress != null && App.EventModel.EventStartTime != null && App.EventModel.EventName != null)
            {
                ev_address.Text = App.EventModel.EventName + "  @  " + App.EventModel.EventAddress;
                var dateFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("{month.full} {day.integer}");
                var eventdate = dateFormatter.Format(App.EventModel.EventStartTime);
                ev_time.Text = eventdate + "  At  " + App.EventModel.EventDuration;
            }
        }



    


        private async void ReserveEvent(object sender, RoutedEventArgs e)
        {
            if (control_calendar.Date != null)
            {
                var new_event = new EventModel()
                {
                    EventStartTime = control_calendar.Date.Value.Date,
                    EventDuration = control_time.Time,
                    //EventName = control_name.Text,
                    EventName = (_linedUpEvents.SelectedItem
                                as ComboBoxItem).Content as string,
                    EventAddress = "",
                    EventInviteText = "",
                    RSVPEmail = "",


                };



                App.EventModel = new_event;
                if (new_event.EventName == "John's Band Night")
                {
                    new_event.EventAddress = "Pike Place Market, Vancouver";
                   
                    new_event.RSVPEmail = "myemail@myemailprovider.com";
                }
                if (new_event.EventName == "Hackathon")
                {
                    new_event.EventAddress = "NewFoundland Place Market, Montreal";
                    new_event.RSVPEmail = "myemail@myemailprovider.com";
                }
                if (new_event.EventName == "Ariana Grande Musical")
                {
                    new_event.EventAddress = "Playdium Centre, Toronto";
                    new_event.RSVPEmail = "myemail@myemailprovider.com";
                }
                if (new_event.EventName == "Microsoft Ignite")
                {
                    new_event.EventAddress = "Celebration Centre, Ottawa";
                    new_event.RSVPEmail = "myemail@myemailprovider.com";
                }
                _events.Add(new_event);
                MessageDialog md =
                new MessageDialog($"{_events.Count} event reserved\n" +
                        "You have your event booked on "+$"{new_event.EventStartTime.Month}/" +
                        $"{new_event.EventStartTime.Day}/" +
                        $"{new_event.EventStartTime.Year}" +
                        $" at {new_event.EventDuration}" +
                        $"for {new_event.EventName}");
                await md.ShowAsync();               
                ev_address.Text = new_event.EventName+"  @  "+new_event.EventAddress;
                var dateFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("{month.full} {day.integer}");
                var eventdate = dateFormatter.Format(new_event.EventStartTime);
                ev_time.Text = eventdate +"  At  "+new_event.EventDuration;
                control_calendar.Date = null;
            }
            else
            {
                MessageDialog md =
                new MessageDialog("Select a day first");
                await md.ShowAsync();
            }
        }
        private async void AddEventToCalendar(object sender, RoutedEventArgs e)
        {
            var new_event = new EventModel();
            _events.Add(new_event);

            var appointment = new Appointment
            {
                Subject = App.EventModel.EventName,
                StartTime = App.EventModel.EventStartTime,
                Duration = App.EventModel.EventDuration,
                Details =
                    @"<html><body><div><p>" + App.EventModel.EventInviteText + @"</p>" +
                    @"<p>Driving directions: <a href='bingmaps:?rtp=~adr." + App.EventModel.EventAddress + @"'>" +
                    App.EventModel.EventAddress + @"</a></p></div></body></html>",
                DetailsKind = AppointmentDetailsKind.Html
            };

            // Get the selection rect of the button pressed to add this appointment 
            var rect = GetElementRect(sender as FrameworkElement);
            await AppointmentManager.ShowAddAppointmentAsync(appointment, rect, Placement.Default);
        }
        public static Rect GetElementRect(FrameworkElement element)
        {
            var transform = element.TransformToVisual(null);
            var point = transform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        private async void GetDirections(object sender, RoutedEventArgs e)
        {
            var directionsUri = new Uri("bingmaps:?rtp=~adr." + App.EventModel.EventAddress);
            await Launcher.LaunchUriAsync(directionsUri);
        }

       



        //private void rsvpButtonHandler(object sender, RoutedEventArgs e)
        //{
        //    var eventPage = (Window.Current.Content as Frame)?.Content as EventMainPage;
        //    var rootSplitView = eventPage?.FindName("rootSplitView") as SplitView;

        //    AppNavigationUtil.SetSplitViewContent(rootSplitView, typeof (RSVP), true);
        //}

    }
    

}