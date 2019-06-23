using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace cs_belt.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        
        [Required]
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        [Required]
        [MinLength(2)]
        public string Title {get; set;}

        [Required]
        public DateTime EventDateTime {get;set;}

        [Required]
        public float Duration {get; set;}

        [Required]
        public string DurIncrement {get; set;}

        [Required]
        public string Details {get; set;}

        public DateTime CreatedAt {get;set;}

        public List<Response> Guests { get; set; }
    }

    public class NewEvent 
    {
        public Event newActivity {get; set;}
        public DateTime time {get; set;}
    }
}