using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return Ok(await _userRepository.GetUsersAsync());
        }
        
        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult<AppUser>> GetUserById(int id)
        {
            return Ok(await _userRepository.GetUserByIdAsync(id));
        }

        [HttpGet("GetUserByUsername/{username}")]
        public async Task<ActionResult<AppUser>> GetUserByUsername(string username)
        {
            return Ok(await _userRepository.GetUserByUsernameAsync(username));
        }
    }
}