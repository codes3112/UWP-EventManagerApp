// Copyright (c) Microsoft. All rights reserved
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace UWPQuickStart.Models
{
    public class EventModel
    {
        /// <summary>
        ///     The EventModel object constructor defines information about the event. Update the event details here to match your
        ///     event!
        /// </summary>
        public EventModel()
        {
            if (EventName == "John's Band Night")
            {
                EventAddress = "Pike Place Market, Vancouver";
                EventStartTime = new DateTime(2017, 11, 4, 20, 0, 0);
                EventDuration = new TimeSpan(4, 0, 0);
                EventInviteText = "It's a Friday night and I feel like jamming. Join me for some good music!";
                EventStartTimeFriendly = EventStartTime.ToString("dddd, MMMM dd, yyyy") + " at " +
                                         EventStartTime.ToString("h:mm tt");
                RSVPEmail = "myemail@myemailprovider.com";
            }
            if (EventName == "Hackathon")
            {
                EventAddress = "NewFoundland Place Market, Montreal";
                EventStartTime = new DateTime(2019, 02, 4, 20, 0, 0);
                EventDuration = new TimeSpan(4, 0, 0);
                EventInviteText = "Get Ready for some coding Fun";
                EventStartTimeFriendly = EventStartTime.ToString("dddd, MMMM dd, yyyy") + " at " +
                                         EventStartTime.ToString("h:mm tt");
                RSVPEmail = "myemail@myemailprovider.com";
            }
            if (EventName == "Ariana Grande Musical")
            {
                EventAddress = "Playdium Centre, Toronto";
                EventStartTime = new DateTime(2019, 09, 4, 20, 0, 0);
                EventDuration = new TimeSpan(4, 0, 0);
                EventInviteText = "Get set Go for the High Performance Musical";
                EventStartTimeFriendly = EventStartTime.ToString("dddd, MMMM dd, yyyy") + " at " +
                                         EventStartTime.ToString("h:mm tt");
                RSVPEmail = "myemail@myemailprovider.com";
            }
        }

        public string EventName { get; set; }
        public string EventAddress { get; set; }
        public DateTime EventStartTime { get; set; }
        public TimeSpan EventDuration { get; set; }
        public string EventInviteText { get; set; }
        public string EventStartTimeFriendly { get; set; }
        public string RSVPEmail { get; set; }
    }
}