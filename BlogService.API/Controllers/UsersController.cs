﻿using BlogService.Core.Services.Interfaces;
using BlogService.Models;
using BlogService.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // POST api/user/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO user)
        {
            try
            {
                var token = await _userService.Login(user.UserName, user.Password);

                if (token == null || token == String.Empty)
                    return BadRequest(new { message = "User name or password is incorrect" });

                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(AppUser user)
        {
            try
            {
                var registeredUser = await _userService.Register(user);
            
                return Ok(registeredUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
