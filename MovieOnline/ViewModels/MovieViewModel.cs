using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MovieOnline.Models.MovieModels;

namespace MovieOnline.ViewModels
{
    public class MovieViewModel : EditMovieImage
    {
        [Required]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "Movie Title")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        public int RateCount { get; set; }
        public int RateTotal { get; set; }


    }
}
