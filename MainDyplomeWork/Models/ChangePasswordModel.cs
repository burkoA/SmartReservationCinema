﻿using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords are different!")]
        public string RepeatPassword { get; set; }
    }
}
