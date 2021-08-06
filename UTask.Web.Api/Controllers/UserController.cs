using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using UTask.Models;
using UTask.Services.Users;

namespace UTask.Web.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;

        private readonly IUserService userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            logger.LogInformation("Handling GetAll Users Request");

            var users = userService.GetAll();

            logger.LogInformation("Request processed. Returning response ...");

            return Ok(users);
        }

        [HttpGet("getbyusername/{username}")]
        public IActionResult GetAll(string username)
        {
            logger.LogInformation("Handling Get User By Username Request");

            var user = userService.Get(username);

            logger.LogInformation("Request processed. Returning response ...");

            if (user == null)
            {
                return NotFound(new
                {
                    Message = $"User with Username { username } could not be found"
                });
            }
            return Ok(user);
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetAll(Guid id)
        {
            logger.LogInformation("Handling Get User By Id Request");

            var user = userService.Get(id);

            logger.LogInformation("Request processed. Returning response ...");

            if (user == null)
            {
                return NotFound(new
                {
                    Message = $"User with Id { id } could not be found"
                });
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public User Register([FromBody] User user)
        {
            logger.LogInformation("Handling Register User Request");

            user = userService.Create(user);

            logger.LogInformation("Request processed. Returning response ...");

            return user;
        }

        [HttpPatch("update")]
        public User Update([FromBody] User user)
        {
            logger.LogInformation("Handling Update User Request");

            user = userService.Update(user);

            logger.LogInformation("Request processed. Returning response ...");

            return user;
        }

        [HttpDelete("delete")]
        public IActionResult Delete(Guid id)
        {
            logger.LogInformation("Handling Update User Request");

            var isDeleted = userService.Delete(id);

            logger.LogInformation("Request processed. Returning response ...");

            if (isDeleted)
            {
                return Ok(new
                {
                    Deleted = true,
                    Message = "User Deleted Successfully"
                });
            }
            return NotFound(new
            {
                Deleted = false,
                Message = $"User with Id { id } could not be deleted"
            });
        }
    }
}
