using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using UTask.Models;
using UTask.Services.Jwt;
using UTask.Services.Users;

namespace UTask.Web.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;

        private readonly IUserService userService;

        private readonly IJwtAuthenticator jwtAuthenticator;

        private readonly IMapper mapper; 

        public UserController(ILogger<UserController> logger, IUserService userService, IJwtAuthenticator jwtAuthenticator, IMapper mapper)
        {
            this.logger = logger;
            this.userService = userService;
            this.jwtAuthenticator = jwtAuthenticator;
            this.mapper = mapper;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            logger.LogInformation("Handling GetAll Users Request");

            var users = userService.GetAll();

            var usersDisplayInfo = users.Select(user => mapper.Map<UserDisplayInfo>(user));

            logger.LogInformation("Request processed. Returning response ...");

            return Ok(usersDisplayInfo);
        }

        [HttpGet("getbyname/{username}")]
        public IActionResult GetByUsername(string username)
        {
            logger.LogInformation("Handling Get User By Username Request");

            User user = null;
            try
            {
                user = userService.Get(username);
            }
            catch (ArgumentException)
            {
                return BadRequest(new { Message = "Username can not be null" });
            }
            catch (Exception exception)
            {
                logger.LogError("Unexpected Error while processing the request", exception);
                StatusCode(StatusCodes.Status500InternalServerError);
            }

            logger.LogInformation("Request processed successfully. Returning response ...");

            if (user == null)
            {
                return NotFound(new
                {
                    Message = $"User with Username { username } could not be found"
                });
            }

            var userDisplayInfo = mapper.Map<UserDisplayInfo>(user);
            return Ok(userDisplayInfo);
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetById(Guid id)
        {
            logger.LogInformation("Handling Get User By Id Request");

            User user = null;
            try
            {
                user = userService.Get(id);
            }
            catch (ArgumentException)
            {
                return BadRequest(new { Message = "Id can not be null" });
            }
            catch (Exception exception)
            {
                logger.LogError("Unexpected Error while processing the request", exception);
                StatusCode(StatusCodes.Status500InternalServerError);
            }

            logger.LogInformation("Request processed successfully. Returning response ...");

            if (user == null)
            {
                return NotFound(new
                {
                    Message = $"User with Id { id } could not be found"
                });
            }
            var userDisplayInfo = mapper.Map<UserDisplayInfo>(user);
            return Ok(userDisplayInfo);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials userCredentials)
        {
            logger.LogInformation("Handling Register User Request");

            User user = null;
            try
            {
                user = userService.Get(userCredentials.Username);
            }
            catch (ArgumentException)
            {
                return BadRequest(new { Message = "Username can not be null" });
            }
            catch (Exception exception)
            {
                logger.LogError("Unexpected Error while processing the request", exception);
                StatusCode(StatusCodes.Status500InternalServerError);
            }

            logger.LogInformation("Request processed successfully. Returning response ...");

            if (user == null)
            {
                return NotFound(new
                {
                    Message = $"User with Username { userCredentials.Username } could not be found"
                });
            }
            var jwtToken = jwtAuthenticator.Authenticate(userCredentials);

            logger.LogInformation("Request processed. Returning response ...");

            if (string.IsNullOrEmpty(jwtToken))
            {
                return Unauthorized();
            }

            return Ok(new
            {
                User = userCredentials,
                JwtToken = jwtToken
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            logger.LogInformation("Handling Register User Request");

            var userCredentials = new UserCredentials
            {
                Username = user.Username,
                Password = user.Password
            };

            var usernameIsTaken = userService.Get(user.Username) != null;

            if (usernameIsTaken)
            {
                return Conflict(new
                {
                    Message = "Usersername is taken!"
                });
            }

            try
            {
                user = userService.Create(user);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception exception)
            {
                logger.LogError("Unexpected Error while processing the request", exception);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            var jwtToken = jwtAuthenticator.Authenticate(userCredentials);

            if (string.IsNullOrEmpty(jwtToken))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { Message = "Authentification failed" });
            }
            logger.LogInformation("Request processed successfully. Returning response ...");
            return Ok(new
            {
                User = user,
                JwtToken = jwtToken
            });
        }

        [Authorize]
        [HttpPatch("update")]
        public IActionResult Update([FromBody] User user)
        {
            logger.LogInformation("Handling Update User Request");

            try
            {
                user = userService.Update(user);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception exception)
            {
                logger.LogError("Unexpected Error while processing the request", exception);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            logger.LogInformation("Request processed. Returning response ...");

            var userDisplayInfo = mapper.Map<UserDisplayInfo>(user);
            return Ok(userDisplayInfo);
        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult Delete(Guid id)
        {
            logger.LogInformation("Handling Update User Request");

            bool isDeleted;
            try
            {
                isDeleted = userService.Delete(id);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception exception)
            {
                logger.LogError("Unexpected Error while processing the request", exception);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

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
