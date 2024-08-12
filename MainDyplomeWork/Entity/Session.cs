using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.Entity
{
    public class Session
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Cinema Name")]
        public int CinemaId { get; set; }
        [ForeignKey("CinemaId")]
        public Cinema Cinema { get; set; }
        [Display(Name = "Hall Name")]
        public int? HallId { get; set; }
        [ForeignKey("HallId")]
        public Hall Hall { get; set; }
        [Display(Name = "Film Name")]
        public int FilmId { get; set; }
        [ForeignKey("FilmId")]
        public Film Film { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Choose Language")]
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public Language Language { get; set; }
        [Display(Name = "Subtitle")]
        public int SubtitleLanguageId { get; set; }
        [ForeignKey("SubtitleLanguageId")]
        public Language SubtitleLanguage;

        public IEnumerable<TicketPrice> TicketPrices { get; set; } = new List<TicketPrice>();
    }
}
