using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V_Okta.Models
{
    public class MoviesHomeModel
    {
        public List<Movie> Movies { get; set; }

        public MoviesHomeModel(List<Data.Entities.Movie> movies, int userId)
        {
            Movies = new List<Movie>();

            foreach(var movie in movies)
            {
                Movies.Add(new Movie(movie, userId));
            }
        }
    }
}
