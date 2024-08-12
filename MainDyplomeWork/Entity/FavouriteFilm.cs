using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.Entity
{
    public class FavouriteFilm
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FilmId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("FilmId")]
        public Film Film { get; set; }
    }
}
