using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.FilmContext
{
    [Index("Email","Time")]
    [Index("IPAddress","Time")]
    public class FailedLogin
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string IPAddress { get; set; }
        [Required]
        public DateTime Time { get; set; }
    }
}
