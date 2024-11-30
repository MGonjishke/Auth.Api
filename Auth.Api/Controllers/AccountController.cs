using Auth.Domain.Dtos;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRegisterService _registerServices;
        private readonly ILoginService _loginService;

        public AccountController(IRegisterService registerService, ILoginService loginService)
        {
            _registerServices = registerService;
            _loginService = loginService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var creatUser = await _registerServices.RegisterAsync(registerDto);

                if (creatUser.RegisterResult.Succeeded)
                {
                    return Ok(creatUser.NewUserDtoResult);
                }
                else
                {
                    var errors = creatUser.RegisterResult;
                    return BadRequest(new { Errors = errors });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
            }
            try
            {
                var loginUser = await _loginService.LoginAsync(loginDto);

                if (!loginUser.IsSuccess)
                {
                    return Unauthorized(loginUser.Message);
                }

                return Ok(new { Message = loginUser.Message, Token = loginUser.Token, UsernameOrEmail = loginUser.UsernameOrEmail });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}
