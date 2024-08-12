using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.FilmContext
{
    public class Director
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name = "Director Name")]
        public string Name { get; set; } = "";
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name = "Birth Place")]
        public string BirthPlace { get; set; } = "";
        [Required]
        [Range(1, 100)]
        [Display(Name = "Work Expretience")]
        public int WorkExperience { get; set; }
        [Required]
        [Range(1, 100)]
        [Display(Name = "Movie Number")]
        public int MovieNumber { get; set; }

        public List<Film> Films { get; set; }
    }
}
