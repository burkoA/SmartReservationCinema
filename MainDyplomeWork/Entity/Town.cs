using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.Entity
{
    public class Town
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name = "Town Name")]
        public string TownName { get; set; }
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        public string Region { get; set; }

        public List<Cinema> Cinemas { get; set; }
    }
}
