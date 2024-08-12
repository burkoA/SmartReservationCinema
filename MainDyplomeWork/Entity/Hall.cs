using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.Entity
{
    public class Hall
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name = "Hall Name")]
        public string HallName { get; set; }
        [Range(0, 500)]
        [Display(Name = "Seat Number")]
        public int SeatNumber { get; set; }
        [Display(Name = "Cinema Name")]
        public int CinemaId { get; set; }
        [ForeignKey("CinemaId")]
        public Cinema Cinema { get; set; }
        public IEnumerable<HallSector> HallSectors { get; set; } = new List<HallSector>();
    }
}
