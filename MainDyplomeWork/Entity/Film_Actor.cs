using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.Entity
{
    public class Film_Actor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ActorId { get; set; }
        [Required]
        public int FilmId { get; set; }
        [ForeignKey("ActorId")]
        public Actor Actor { get; set; }
        [ForeignKey("FilmId")]
        public Film Film { get; set; }
    }
}
