using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieOnline.Models.MovieModels
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Genre")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
    }
}
