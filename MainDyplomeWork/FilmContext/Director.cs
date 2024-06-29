using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.FilmContext
{
    public class Director
    {
        [Key]
        public int Id_Director { get; set; }
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name = "Director Name")]
        public string Name_Director { get; set; } = "";
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name = "Birth Place")]
        public string Birth_Place { get; set; } = "";
        [Required]
        [Range(1,100)]
        [Display(Name = "Work Expretience")]
        public int Work_Experience { get; set; }
        [Required]
        [Range(1,100)]
        [Display(Name = "Movie Number")]
        public int Movie_Number { get; set; }

        public List<Film> Films { get; set; }
    }
}
