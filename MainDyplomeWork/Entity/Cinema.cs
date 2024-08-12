using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.Entity
{
    public class Cinema
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name = "Cinema Name")]
        public string CinemaName { get; set; }
        [Required]
        [MinLength(3)]
        public string Localisation { get; set; }
        public double LongCoordinate { get; set; }
        public double LatCoordinate { get; set; }
        [Required]
        [Range(1, 5)]
        [Display(Name = "Cinema Rating")]
        public double CinemaRating { get; set; }
        public string Image { get; set; } = "";
        //public IEnumerable<Town> TownsCinema { get; set; } = new List<Town>();
        [Display(Name = "Choose Town")]
        public int TownId { get; set; }
        [ForeignKey("TownId")]
        public Town Town { get; set; }
        public List<Session> Sessions { get; set; } = new List<Session>();
    }
}
