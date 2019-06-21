using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace cs_belt.Models
{
    public class Response
    {
        [Key]
        public int ResponseId { get; set; }
        public int UserId { get; set; }
        public User Attendee { get; set; }
        public int EventId { get; set; }
        public Event Activity { get; set; }
        public int NumGuests { get; set; }
    }
}