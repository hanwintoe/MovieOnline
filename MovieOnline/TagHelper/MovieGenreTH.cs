using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using MovieOnline.Data;
using MovieOnline.Models.MovieModels;

namespace MovieOnline.TagHelper
{
    [HtmlTargetElement("td", Attributes ="i-movie")]
    public class MovieGenreTH : ITagHelper
    {
        private readonly ApplicationDbContext db;

        public MovieGenreTH(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HtmlAttributeName("i-movie")]
        public string mov { get; set; }

        int ITagHelperComponent.Order => throw new NotImplementedException();

        void ITagHelperComponent.Init(TagHelperContext context)
        {
            //throw new NotImplementedException();
        }

       public async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> genName = new();
            List<MovieGenre> movieGenre = new();
            List<Genre> genre = new();
            genre = db.Genre.ToList();

            Movie movie = db.Movie.Include("MovieGenres").FirstOrDefault(x => x.Id == int.Parse(mov));
            movie.MovieGenres.ToList().ForEach(result => movieGenre.Add(result));

            if(movieGenre.Count > 0)
            {
                foreach(var gen in movieGenre)
                {
                    genName.Add(genre[gen.GenreId - 1].Name);
                }
            }
            output.Content.SetContent(genName.Count == 0 ? "Select Genre" : string.Join(", ", genName));
        }
    }
}
