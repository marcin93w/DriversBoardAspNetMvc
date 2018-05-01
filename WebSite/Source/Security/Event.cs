using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Driver.WebSite.Source.Security
{
    public class Event
    {
        public User User { set; get; }
        public DetectionPoint DetectionPoint { set; get; }
        public DateTime Timestamp { get; set; }

        public Event()
        {
            Timestamp = DateTime.Now;
        }
    }

    public class User
    {
        public string Username { set; get; }
    }

    public class DetectionPoint
    {
        public string Label { set; get; }
        public string Category { set; get; }
    }
}