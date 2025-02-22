﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.Entity
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name = "Genre Name")]
        public string GenreName { get; set; }

        public List<Genre_Film> Films { get; set; }
    }
}
