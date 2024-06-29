﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.FilmContext
{
    public class Actor
    {
        [Key]
        public int Id_Actor { get; set; }
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name ="Actor Name")]
        public string Actor_Name { get; set; } = "";
        [Required]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "The field cannot contain numbers.")]
        [Display(Name ="Nationality")]
        public string Nationality { get; set; } = "";
        public List<Film_Actor> Films { get; set; }
    }
}
