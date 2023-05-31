using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetResult()
        {   
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {   
            var thing = _context.Users.Find(-1);
            
            if(thing == null) return NotFound();

            return thing;
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {   
            try
            {
                var thing = _context.Users.Find(-1);

                var thingToReturn = thing.ToString();

                return thingToReturn;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {Message = "Computer says no!", Error=ex.Message});
            }
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {   
            return BadRequest("This is not a good request");
        }
    }
}