using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.FilmContext
{
	public class TicketPrice
	{
		[Key]
		public int Id { get; set; }
		[Range(0,500)]
		public double Price { get; set; }
        [Display(Name = "Session")]
        public int SessionId { get; set; }
        [Display(Name = "Sector")]
        public int SectorId { get; set; }
		[ForeignKey("SessionId")]
		public Session Session { get; set; }
		[ForeignKey("SectorId")]
		public HallSector HallSector { get; set; }
	}
}
