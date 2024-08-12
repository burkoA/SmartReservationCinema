using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.Entity
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name = "Actor Name")]
        public string Name { get; set; } = "";
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name = "Nationality")]
        public string Nationality { get; set; } = "";
        public List<Film_Actor> Films { get; set; }
    }
}
