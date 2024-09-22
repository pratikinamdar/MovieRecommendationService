using System.ComponentModel.DataAnnotations;

namespace MovieRecommendation.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class UserParams
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
