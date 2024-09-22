using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MovieRecommendation.Models;

namespace MovieRecommendation.Controllers
{

    public class UserController: ControllerBase
    {
        private MovieDatabase db;
        public UserController(IConfiguration config)
        {
            db = new MovieDatabase(config.GetConnectionString("connstr"));
        }

        [HttpPost("User", Name = "PostUsers")]
        public void PostUser(UserParams user)
        {
            User newUser = new User() { Name = user.Name, Age = user.Age};
            db.User.Add(newUser);
            db.SaveChanges();
        }

        [HttpGet("User", Name = "GetUsers")]
        public IEnumerable<User> GetAllUser()
        {
            return (from u in db.User
                    select u).ToList();
        }


        [HttpDelete("User/{UserId}", Name = "DeleteUser")]
        public void DeleteMovie(int UserId)
        {

            var rat = (from r in db.Rating
                       where r.UserId == UserId
                       select r).ToList();
            foreach (var r in rat)
            {
                db.Rating.Remove(r);
            }
            db.SaveChanges();

            var user = db.User.Find(UserId);
            db.User.Remove(user);
            db.SaveChanges();
        }


    }
}
