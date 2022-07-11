using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MovieOnline.Models.MovieModels
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<SelectListItem> DrpGenres { get; set; }

        [Display(Name ="Genres")]
        public int[]  GenreIds { get; set; }


    }
}
