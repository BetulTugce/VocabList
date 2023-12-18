﻿using Microsoft.AspNetCore.Mvc;
using VocabList.Core.DTOs.Identity;
using VocabList.Core.Services;


namespace VocabList.API.Controllers
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

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUser model)
        {
            try
            {
                var createdUser = await _userService.CreateAsync(model);
                return CreatedAtAction(nameof(CreateUser), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                //return StatusCode(500, new { ErrorMessage = "Kullanıcı kayıt işlemi başarısız oldu. " + ex.Message });
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new { ErrorMessage = "Kullanıcı bulunamadı." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = "Kullanıcıyı getirme işlemi başarısız oldu. " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                return Ok(await _userService.GetAllUsersAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = "Kullanıcıları listeleme işlemi başarısız oldu. " + ex.Message });
            }
        }
    }
}
