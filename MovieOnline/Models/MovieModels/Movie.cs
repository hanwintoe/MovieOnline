using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieOnline.Models.MovieModels
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        //Date
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        //Data Actor

        //Data Actress

        //Data Director

        //Country

        //Rating
        public int RateCount { get; set; }
        public int RateTotal { get; set; }

        //Genre
        [Display(Name ="Genre")]
        public List<MovieGenre> MovieGenres { get; set; }

        //Images
        [Display(Name = "HiLight Image")]
        public string ImageHiLight { get; set; }

        [Display(Name = "View Image")]
        public string ImageView { get; set; }

        [Display(Name = "Banner Image")]
        public string ImageBanner { get; set; }
    }
}
