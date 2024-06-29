﻿using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.Models
{
    public class RegistrationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; } = "";
        public string City { get; set; }
        public string Address { get; set; }
        [Compare("Password",ErrorMessage ="Passwords are different!")]
        public string PasswordRepeat { get; set; }
    }
}
