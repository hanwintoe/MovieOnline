using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MovieOnline.Models.MovieModels
{
    public class UploadMovieImage
    {
        [Display(Name = "HiLight Image")]
        public IFormFile MovieImageHiLight { get; set; }


        [Display(Name = "View Image")]
        public IFormFile MovieImageView { get; set; }

        [Display(Name = "Banner Image")]
        public IFormFile MovieImageBanner { get; set; }
    }
}
