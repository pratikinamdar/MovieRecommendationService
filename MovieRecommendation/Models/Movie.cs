using System.ComponentModel.DataAnnotations;

namespace MovieRecommendation.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        public string Name { get; set; }

        public string Genre { get; set; }

        public double Rating { get; set; }
    }

    public class MovieParams
    {
        public string Name { get; set; }

        public string Genre { get; set; }
    }
}
