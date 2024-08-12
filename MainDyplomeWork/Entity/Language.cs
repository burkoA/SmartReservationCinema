using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.Entity
{
    public class Language
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name = "Language Name")]
        public string LanguageName { get; set; }
    }
}
