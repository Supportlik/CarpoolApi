using System.Collections.Generic;
using System.Linq;
using CarPoolApi.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarPoolApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private CarpoolContext _cpctx;
        private DbSet<User> _users;

        public UserController(CarpoolContext cpctx)
        {
            this._cpctx = cpctx;
            this._users = cpctx.Users;
        }

        [HttpGet]
        [Route("/login")]
        public ActionResult<User> Login()
        {
            var sub = HttpContext.User.Claims.FirstOrDefault(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            var user = _users.SingleOrDefault(user =>
                user.OauthId == sub);

            if (user == null)
                return NotFound();
            else
                return Ok(user);
        }
        
        [HttpGet("search/{searchString}")]
        public ActionResult<List<User>> Search(string searchString)
        {
            var sub = HttpContext.User.Claims.FirstOrDefault(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            var user = _users.SingleOrDefault(user =>
                user.OauthId == sub);

            var foundUser = _users.Where(u =>
                (u.Vorname.Contains(searchString) || u.Nachname.Contains(searchString)) && u.Id != user.Id);
            return Ok(foundUser);
        }
        

        [HttpPost]
        [Route("/register")]
        public ActionResult<User> Register(User userIn)
        {
            var sub = HttpContext.User.Claims.FirstOrDefault(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;
            var user = _users.SingleOrDefault(user => user.OauthId == sub);

            if (user == null)
            {
                userIn.OauthId = sub;
                _users.Add(userIn);
                _cpctx.SaveChanges();

                return Ok(userIn);
            }
            else
            {
                return Login();
            }
        }
    }
}