using System.Collections.Generic;
using System.Linq;
using CarPoolApi.DB;
using CarPoolApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarPoolApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class FahrgemeinschaftController : ControllerBase
    {
        private CarpoolContext _cpctx;
        private DbSet<User> _users;
        private DbSet<Fahrgemeinschaft> _fahrgemeinschafts;
        private DbSet<FahrgemeinschaftMitglied> _fahrgemeinschaftMitglieds;
        private DbSet<Fahrt> _fahrts;

        public FahrgemeinschaftController(CarpoolContext cpctx)
        {
            _cpctx = cpctx;
            _users = cpctx.Users;
            _fahrgemeinschafts = cpctx.Fahrgemeinschafts;
            _fahrgemeinschaftMitglieds = cpctx.FahrgemeinschaftMitglieds;
            _fahrts = _cpctx.Fahrts;
        }

        // list
        [HttpGet]
        public ActionResult<List<Fahrgemeinschaft>> Get()
        {
            var sub = HttpContext.User.Claims.FirstOrDefault(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            var user = _users
                // .Include(u => u.Fahrgemeinschafts)
                .SingleOrDefault(user =>
                    user.OauthId == sub);
            if (user == null)
                return NotFound();

            var fahrgemeinschaften = _fahrgemeinschafts
                .Where(f => f.FahrgemeinschaftMitglieds.Any(fm => fm.UserId == user.Id)).ToList();

            return Ok(fahrgemeinschaften);
        }


        // get
        [HttpGet("{id}")]
        public ActionResult<Fahrgemeinschaft> Get(int id)
        {
            var fahrgemeinschaft = _fahrgemeinschafts.FirstOrDefault(f => f.Id == id);
            if (fahrgemeinschaft == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(fahrgemeinschaft);
            }
        }

        // create
        [HttpPost]
        public ActionResult<Fahrgemeinschaft> Post(FahrgemeinschaftCreateRequest fcr)
        {
            var sub = HttpContext.User.Claims.FirstOrDefault(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            var user = _users.SingleOrDefault(user =>
                user.OauthId == sub);
            if (user == null)
                return NotFound();

            var fahrgemeinschaft = new Fahrgemeinschaft
            {
                CreatorId = user.Id,
                Name = fcr.Name,
                FahrgemeinschaftMitglieds = {new FahrgemeinschaftMitglied {UserId = user.Id}}
            };

            _fahrgemeinschafts.Add(fahrgemeinschaft);
            _cpctx.SaveChanges();
            return CreatedAtAction(nameof(Get), new {id = fahrgemeinschaft.Id}, fahrgemeinschaft);
        }

        // update
        [HttpPut("{id}")]
        public ActionResult<Fahrgemeinschaft> Put(int id, FahrgemeinschaftCreateRequest fcr)
        {
            var fahrgemeinschaft = _fahrgemeinschafts.FirstOrDefault(f => f.Id == id);
            if (fahrgemeinschaft == null)
                return NotFound();
            else
            {
                fahrgemeinschaft.Name = fcr.Name;
                _fahrgemeinschafts.Update(fahrgemeinschaft);
                _cpctx.SaveChanges();
                return NoContent();
            }
        }

        // delte
        [HttpDelete("{id}")]
        public ActionResult<Fahrgemeinschaft> Delete(int id)
        {
            var fahrgemeinschaft = _fahrgemeinschafts.FirstOrDefault(f => f.Id == id);
            if (fahrgemeinschaft == null)
                return NotFound();
            else
            {
                _fahrgemeinschafts.Remove(fahrgemeinschaft);
                _cpctx.SaveChanges();
                return NoContent();
            }
        }


        [HttpPost("{id}/{userId}")]
        public ActionResult<Fahrgemeinschaft> AddFM(int id, int userId)
        {
            var fahrgemeinschaft = _fahrgemeinschafts.FirstOrDefault(f => f.Id == id);

            if (fahrgemeinschaft == null)
                return NotFound();
            else
            {
                var fm = fahrgemeinschaft.FahrgemeinschaftMitglieds.FirstOrDefault(fm => fm.UserId == userId);
                if (fm != null)
                {
                    return Conflict();
                }

                var user = _users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    return NotFound();
                }

                var newFm = new FahrgemeinschaftMitglied
                {
                    FahrgemeinschaftId = fahrgemeinschaft.Id,
                    UserId = userId
                };

                _fahrgemeinschaftMitglieds.Add(newFm);
                _cpctx.SaveChanges();
                return CreatedAtAction(nameof(Get), new {id = fahrgemeinschaft.Id}, fahrgemeinschaft);
            }
        }

        [HttpDelete("{id}/{userId}")]
        public ActionResult<Fahrgemeinschaft> RemoveFM(int id, int userId)
        {
            var fahrgemeinschaft = _fahrgemeinschafts.FirstOrDefault(f => f.Id == id);

            if (fahrgemeinschaft == null)
                return NotFound();
            else
            {
                var fm = fahrgemeinschaft.FahrgemeinschaftMitglieds.FirstOrDefault(fm => fm.UserId == userId);
                if (fm == null)
                {
                    return NotFound();
                }

                if (fm.UserId == fahrgemeinschaft.CreatorId)
                {
                    _fahrgemeinschafts.Remove(fahrgemeinschaft);
                }
                else
                {
                    _fahrgemeinschaftMitglieds.Remove(fm);
                }

                _cpctx.SaveChanges();
                return NoContent();
            }
        }

        [HttpPost("{id}/fahrt")]
        public ActionResult<Fahrgemeinschaft> AddFahrt(int id, FahrtCreateRequest fcr)
        {
            var fahrgemeinschaft = _fahrgemeinschafts.FirstOrDefault(f => f.Id == id);

            if (fahrgemeinschaft == null)
                return NotFound();
            else
            {
                var fm = fahrgemeinschaft.FahrgemeinschaftMitglieds.FirstOrDefault(fm => fm.Id == fcr.FahrerId);
                if (fm == null)
                {
                    return NotFound();
                }

                var newFahrt = new Fahrt
                {
                    FahrgemeinschaftId = fahrgemeinschaft.Id,
                    FahrerId = fcr.FahrerId,
                    Date = fcr.Date
                };

                _fahrts.Add(newFahrt);
                _cpctx.SaveChanges();
                return CreatedAtAction(nameof(Get), new {id = fahrgemeinschaft.Id}, fahrgemeinschaft);
            }
        }

        [HttpDelete("{id}/fahrt")]
        public ActionResult<Fahrgemeinschaft> RemoveFM(int id)
        {
            var fahrt = _fahrts.FirstOrDefault(f => f.Id == id);

            if (fahrt == null)
                return NotFound();
            else
            {
                _fahrts.Remove(fahrt);

                _cpctx.SaveChanges();
                return NoContent();
            }
        }
    }
}