using Auth.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserManagementController(IUserService userService) 
            {
                _userService = userService;
            }
       
        [HttpGet("get all")]
        public async Task<IActionResult> GetAllUsers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _userService.GetAllUsers();

            if (!users.Any())
            {
                return NotFound("There is no user.");
            }

            return Ok(users);

        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUser([FromQuery] string query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query parameter cannot be empty.");
            }

            var user = await _userService.SearchUserAsync(query);

            if (!user.Any())
            {
                return NotFound("there is no user.");
            }

            return Ok(user);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("delete user")]
        public async Task<IActionResult> DeleteUser([FromBody]string? id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.DeleteUserAsync(id);

            if (result.Succeeded)
            {
                return Ok(new {Result = result, Message = "User Deleted."});
            }
            else
            {
                var errors = result.Errors;
                return BadRequest(errors);
            }        
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("change role")]
        public async Task<IActionResult> ChangeRoleToAdmin([FromBody]string id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.ChangeRoleToAdmin(id);

            if (result.Succeeded)
            {
                return Ok(new { Result = result, Message = "Role succeesfully changed" });
            }

            else
            {
                return BadRequest(result);
            }
        }
    }
}
