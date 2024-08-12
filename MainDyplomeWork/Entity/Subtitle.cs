using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.Entity
{
    public class Subtitle
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Is subtitle?")]
        public bool IsSubtitle { get; set; }
        [Display(Name = "Language Name")]
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public Language Language { get; set; }
        [Display(Name = "Film Name")]
        public int FilmId { get; set; }
        [ForeignKey("FilmId")]
        public Film Film { get; set; }
    }
}
