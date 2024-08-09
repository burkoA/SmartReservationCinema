using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.FilmContext
{
    public class Film_Actor
    {
        [Key]
        public int Id { get; set; }
        public int ActorId { get; set; }
        public int FilmId { get; set; }
        [ForeignKey("ActorId")]
        public Actor Actor { get; set; }
        [ForeignKey("FilmId")]
        public Film Film { get; set; }
    }
}
