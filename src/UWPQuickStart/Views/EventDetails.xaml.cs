// Copyright (c) Microsoft. All rights reserved
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using UWPQuickStart.Models;
using UWPQuickStart.Utils;
using Windows.ApplicationModel.Appointments;
using Windows.Devices.Geolocation;
using Windows.Foundation;
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

namespace UWPQuickStart.Views
{
    

    public sealed partial class EventDetails : UserControl
    {
        MyDatabase db;       
        public Library Library = new Library();
        public int indexPosition = 1;
       


        public EventDetails()
        {
            InitializeComponent();
            db = new MyDatabase();
            DataContext = App.EventModel;
            if (App.EventModel.EventName == "John's Band Night")
            {
                App.EventModel.EventAddress = "Vancouver";
                App.EventModel.RSVPEmail = "myemail@myemailprovider.com";
            }
            if (App.EventModel.EventName == "Hackathon")
            {
                App.EventModel.EventAddress = "Montreal";
                App.EventModel.RSVPEmail = "myemail@myemailprovider.com";
            }
            if (App.EventModel.EventName == "Ariana Grande Musical")
            {
                App.EventModel.EventAddress = "Toronto";
                App.EventModel.RSVPEmail = "myemail@myemailprovider.com";
            }
            if (App.EventModel.EventName == " Microsoft Ignite")
            {
                App.EventModel.EventAddress = "Ottawa";
                App.EventModel.RSVPEmail = "myemail@myemailprovider.com";
            }
           
            InitializeMap();
        }
       

     
        protected async override void OnHolding(HoldingRoutedEventArgs e)
        {

               
                Geopoint myPoint = await Library.Position();
                mapControl.ZoomLevel = 16;
                mapControl.Center = myPoint;
                AddIconToLocation(myPoint, "Position" + indexPosition);
                indexPosition++;           
        }
      

        private void AddIconToLocation(Geopoint location, string name)
        {
            //create an icon and get text
            MapIcon mapIcon = new MapIcon();
            mapIcon.Location = location;
            mapIcon.Title = String.Format("{0}\nLatLng: {1}\nLatLng:{2}", name, location.Position.Latitude, location.Position.Longitude);

            //add custom image for pushpin
            mapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/mappin.png"));
            mapIcon.ZIndex = 0;
            mapControl.MapElements.Add(mapIcon);
        }

        private async void ShowRouteOnMap(Geopoint startPoint, Geopoint endPoint)
        {
            MapRouteFinderResult routeResult = await MapRouteFinder.GetDrivingRouteAsync
                (startPoint, endPoint, MapRouteOptimization.Time, MapRouteRestrictions.None);
            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                mapControl.Routes.Clear();//remove all old routes
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.Yellow;
                viewOfRoute.OutlineColor = Colors.Black;
                mapControl.Routes.Add(viewOfRoute);
                await mapControl.TrySetViewBoundsAsync(routeResult.Route.BoundingBox, null,MapAnimationKind.None);
            }
            else
            {
                var message = new MessageDialog("Can't find Routes");
                await message.ShowAsync();
            }
        }
        
       

      
        

        private async void Button_Click(object sender,RoutedEventArgs e)
        {
            //Geopoint position = await Library.Position();
            //DependencyObject marker = Library.Marker();
            //mapControl.Children.Add(marker);
            //MapControl.SetLocation(marker, position);
            //MapControl.SetNormalizedAnchorPoint(marker, new Point(0.5, 0.5));
            //mapControl.ZoomLevel = 12;
            //mapControl.Center = position;
         }


        private async void mapControl_Maptapped(MapControl sender, MapInputEventArgs args)
        {
            AddIconToLocation(args.Location, "Position:" + indexPosition);
            indexPosition++;
            Geopoint startPoint = await Library.Position();
            ShowRouteOnMap(startPoint, args.Location);//draw path when you click on map


        }
        private async void InitializeMap()
        {
            var queryHintGeoPosition = new BasicGeoposition
            {
                Latitude = 43.5890,
                Longitude= 79.6441
            };
            if (App.EventModel.EventAddress != "")
            {
                var result =
                await
                    MapLocationFinder.FindLocationsAsync(App.EventModel.EventAddress, new Geopoint(queryHintGeoPosition));
                if (result != null && result.Locations.Count != 0)
                {
                    await mapControl.TrySetViewAsync(result.Locations[0].Point, 16, 0, 0, MapAnimationKind.None);
                }

            }
            
            

            var mapIconStreamReference =
                RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/mappin.png"));
            var mapIcon = new MapIcon
            {
                Location = mapControl.Center,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = "Event location",
                Image = mapIconStreamReference,
                ZIndex = 0
            };

            mapControl.MapElements.Add(mapIcon);
        }

        //private async void AddEventToCalendar(object sender, RoutedEventArgs e)
        //{
        //    var new_event = new EventModel();
        //    _events.Add(new_event);

        //    var appointment = new Appointment
        //    {
        //        Subject = App.EventModel.EventName,
        //        StartTime = App.EventModel.EventStartTime,
        //        Duration = App.EventModel.EventDuration,
        //        Details =
        //            @"<html><body><div><p>" + App.EventModel.EventInviteText + @"</p>" +
        //            @"<p>Driving directions: <a href='bingmaps:?rtp=~adr." + App.EventModel.EventAddress + @"'>" +
        //            App.EventModel.EventAddress + @"</a></p></div></body></html>",
        //        DetailsKind = AppointmentDetailsKind.Html
        //    };

        //    // Get the selection rect of the button pressed to add this appointment 
        //    var rect = GetElementRect(sender as FrameworkElement);
        //    await AppointmentManager.ShowAddAppointmentAsync(appointment, rect, Placement.Default);
        //}

        //Gets the physical position of the visual in tree.
        //public static Rect GetElementRect(FrameworkElement element)
        //{
        //    var transform = element.TransformToVisual(null);
        //    var point = transform.TransformPoint(new Point());
        //    return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        //}

        //private async void GetDirections(object sender, RoutedEventArgs e)
        //{
        //    var directionsUri = new Uri("bingmaps:?rtp=~adr." + App.EventModel.EventAddress);
        //    await Launcher.LaunchUriAsync(directionsUri);
        //}
    }
   
}