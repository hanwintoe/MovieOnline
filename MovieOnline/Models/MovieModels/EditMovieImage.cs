using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieOnline.Models.MovieModels
{
    public class EditMovieImage : UploadMovieImage
    {
        [Key]
        public int Id { get; set; }

        public string ExistingImageHiLight { get; set; }

        public string ExistingImageView { get; set; }

        public string ExistingImageBanner { get; set; }
    }
}
