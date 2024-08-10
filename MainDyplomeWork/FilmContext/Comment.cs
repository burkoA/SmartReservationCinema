using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.FilmContext
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(100)]
        public string Text { get; set; }
        [Required]
        public int FilmId { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey("FilmId")]
        public Film Film { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}
