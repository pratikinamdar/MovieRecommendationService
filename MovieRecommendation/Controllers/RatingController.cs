using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MovieRecommendation.Models;

namespace MovieRecommendation.Controllers
{
    public class RatingController:ControllerBase
    {
        private MovieDatabase db;
        public RatingController(IConfiguration config)
        {
            db = new MovieDatabase(config.GetConnectionString("connstr"));
        }

        [HttpPost("Rating", Name = "PostRating")]
        public void PostUser(RatingParams rating)
        {

            var x = (from ra in db.Rating
                    where rating.UserId == ra.UserId && rating.MovieId == ra.MovieId
                    select ra).FirstOrDefault();
            if (x != null) 
            {
                throw new Exception("This used has already provided rating for this movie");
            }

            Rating newRating = new Rating() { RatingValue = rating.RatingValue, MovieId = rating.MovieId, UserId=rating.UserId};
            if (newRating.RatingValue > 5 || newRating.RatingValue < 0)
            {
                throw new Exception("Rating ID is not acceptable. Enter value between 0-5");
            }
            db.Rating.Add(newRating);
            db.SaveChanges();


            List<int> r = (
                               from ra in db.Rating
                               where ra.MovieId == rating.MovieId
                               select ra.RatingValue
                              ).ToList();
            int sum = 0;
            int count = 0;
            foreach (var rat in r)
            {
                count++;
                sum += rat;
            }
           double avgRat =(double) sum / count;

           /* var avgRating = db.Rating.GroupBy(rating => rating.MovieId)
                .Select(g => new
                {
                    MovieId = g.Key,
                    AverageRating = g.Average(rating => rating.RatingValue),
                }
                ).FirstOrDefault(); */

            // replace 3
            this.ModifyMovie(rating.MovieId, avgRat);
        }


        [HttpPut("Rating", Name = "ModifyRating")]
        void ModifyMovie(int MovieId, double Rating)
        {
            Movie movie = (from m in db.Movie
                           where m.MovieId == MovieId
                           select m).FirstOrDefault();
            movie.Rating = Rating;
            db.Movie.Update(movie);
            db.SaveChanges();

        }

        [HttpGet("Rating", Name = "GetRatings")]
        public IEnumerable<Rating> GetAllRating()
        {
            return (from r in db.Rating
                    select r).ToList();
        }

    }
}
