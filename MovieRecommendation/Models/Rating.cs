using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MovieRecommendation.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public int RatingValue { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
    }


    public class RatingParams
    {
        public int RatingValue { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
    }
}
