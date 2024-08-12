using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.Entity
{
    public class Genre_Film
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int FilmId { get; set; }
        [Required]
        public int GenreId { get; set; }
        [ForeignKey("FilmId")]
        public Film Film { get; set; }
        [ForeignKey("GenreId")]
        public Genre Genre { get; set; }

    }
}
