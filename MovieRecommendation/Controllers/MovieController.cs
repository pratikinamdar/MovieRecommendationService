using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MovieRecommendation.Models;

namespace MovieRecommendation.Controllers
{
    public class MovieController : ControllerBase
    {
        private MovieDatabase db;
        public MovieController(IConfiguration config)
        {
            db = new MovieDatabase(config.GetConnectionString("connstr"));
        }

        [HttpPost("/", Name = "PostMovies")]
        public void PostMovie(MovieParams movie)
        {
            Movie newMovie = new Movie() { Name = movie.Name, Genre = movie.Genre, Rating = 0 };
            db.Movie.Add(newMovie);
            db.SaveChanges();
        }

        [HttpGet("/", Name = "GetMovies")]
        public IEnumerable<Movie> GetAllMovie()
        {
            return (from m in db.Movie
                    select m).ToList();
        }


        [HttpDelete("/{MovieId}", Name = "DeleteGetMovies")]
        public void DeleteMovie(int MovieId)
        {
            var movie = db.Movie.Find(MovieId);
            db.Movie.Remove(movie);
            db.SaveChanges();
        }


        [HttpGet("Recommendation/{UserId}", Name = "GetRecommendation")]
        
       public  IEnumerable<Movie> GetMovie(int UserId)
        {
            /*var g = db.Movie.Select(m => m.Genre).Distinct();
            foreach (var gen in g) 
            {
                if (genre != gen)
                {
                    throw new Exception("Movie genre doesnt exist in the library. Please list correct genre");
                }
            } */

            var genAvgRating = (from m in db.Movie
                                join r in db.Rating on m.MovieId equals r.MovieId
                                group new { m, r } by m.Genre into genreGrp
                                select new
                                {
                                    Genre = genreGrp.Key,
                                    AverageRating = genreGrp.Average(x => x.r.RatingValue)
                                });
            var highestRatedGenre = genAvgRating.OrderByDescending(g => g.AverageRating).FirstOrDefault();
            List<Movie> movies = null;
            if (highestRatedGenre != null)
            {
                 movies = (from m in db.Movie
                              where m.Genre == highestRatedGenre.Genre
                              select m).ToList();
            }
            if (movies == null)
            {
                throw new Exception("No movies recommended");
            }
            return movies;

        }

    }
}
