using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.FilmContext
{
	public class HallSector
	{
		[Key]
		public int Id { get; set; }
		[Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name = "Sector Name")]
        public string SectorName { get; set; }
        [Display(Name = "Hall Name")]
        public int HallId { get; set; }
		[ForeignKey("HallId")]
		public Hall Hall { get; set; }
	}
}
