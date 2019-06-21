using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace cs_belt.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}

        [Required]
        [MinLength(2)]
        public string FirstName {get; set;}

        [Required]
        [MinLength(2)]
        public string LastName {get; set;}

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email {get; set;}

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password {get; set;}

        [NotMapped]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Confirm {get; set;}

        public DateTime CreatedAt {get;set;}

        // public List<Message> Posts {get; set;}
        // public List<Comment> Comments {get; set;}
    }

    public class LoginUser {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string liEmail {get; set;}

        [Required]
        [DataType(DataType.Password)]
        public string liPassword {get; set;}
    }

    public class UserViewModel {

        public User regUser {get; set;}
        public LoginUser liUser {get; set;}
    }
}