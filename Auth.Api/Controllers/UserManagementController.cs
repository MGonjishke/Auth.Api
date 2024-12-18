﻿using Auth.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserManagementController(IUserService userService) 
            {
                _userService = userService;
            }
       
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _userService.GetAllUsers();

            return Ok(users);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody]string? id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.DeleteUserAsync(id);

            if (result.Succeeded)
            {
                return Ok(new {Result = result, Message = "User Deleted"});
            }
            else
            {
                var errors = result.Errors;
                return BadRequest(errors);
            }        
        }
    }
}
