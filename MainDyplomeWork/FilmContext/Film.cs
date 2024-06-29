using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.FilmContext
{
    public class Film
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(30)]
        public string Description { get; set; } = "";
        [Required]
        [MinLength(4)]
        [MaxLength(100)]
        [Display(Name = "Film Name")]
        public string FilmName { get; set; } = "";
        [Required]
        [Range(0,500)]
        [Display(Name = "Duration")]
        public int Time { get; set; }
        [MaxLength(50)]
        public string Image { get; set; } = "";
        [Required]
        [DataType(DataType.Date)]
        public DateTime Realese { get; set; }
        [Range(1,5)]
        public double Rating { get; set; }
        public IEnumerable<Genre_Film> Genres { get; set; }=new List<Genre_Film>();
        [Display(Name = "Director Name")]
        public int DirectorId { get; set; }
        [ForeignKey("DirectorId")]
        public Director Director { get; set; }
        public IEnumerable<Film_Actor> Actors { get; set; } = new List<Film_Actor>();
        public IEnumerable<Subtitle> Subtitles { get; set; } = new List<Subtitle>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public IEnumerable<FilmMark> Marks { get; set; } = new List<FilmMark>();
        public IEnumerable<FavouriteFilm> FavouriteFilms { get; set; } = new List<FavouriteFilm>();
    }
}
