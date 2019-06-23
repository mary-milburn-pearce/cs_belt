using System;
using System.Collections.Generic;

namespace cs_belt.Models
{
    public class ViewModel
    {
        public Event activity { get; set; }
        public DateTime time { get; set; }
        public int totalGuests { get; set; }
        public string action { get; set; }
    }
    public class BeltViewModel
    {
        public List<ViewModel> eventList {get; set;}
        public User currUser {get; set;}
    }
}
