using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UTask.DataAccess;
using UTask.Models;
using UTask.Services.Cryptography;

namespace UTask.Services.Jwt
{
    public class JwtAuthenticator : IJwtAuthenticator
    {
        private readonly string tokenKey;

        private readonly ILogger<JwtAuthenticator> logger;

        private readonly IRepository<User> userRepository;

        private readonly ICryptographyService cryptographyService;

        public JwtAuthenticator(string tokenKey, IRepository<User> userRepository, ILogger<JwtAuthenticator> logger, ICryptographyService cryptographyService)
        {
            this.tokenKey = tokenKey;
            this.userRepository = userRepository;
            this.logger = logger;
            this.cryptographyService = cryptographyService;
        }

        public string Authenticate(UserCredentials userCredentials)
        {
            logger.LogInformation($"Trying to authenticate User with username \"{ userCredentials.Username }\" ...");
            var users = userRepository.GetAll();

            Func<User, bool> checkCredentials = user =>
                user.Username == userCredentials.Username && 
                user.Password == cryptographyService.GetPasswordSHA3Hash(userCredentials.Password);

            logger.LogInformation("Start Verifing credentials");
            var authenticationResult = users.Any(checkCredentials);

            if (!authenticationResult)
            {
                logger.LogInformation($"Authentication failed: Wrong UserCredentials");
                return null;
            }

            logger.LogInformation($"Successfully authenticated");

            var tokenHandler = new JwtSecurityTokenHandler();

            logger.LogInformation($"Start creating new jwtToken");

            var key = Encoding.ASCII.GetBytes(tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userCredentials.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            logger.LogInformation($"New JwtToken created: { jwtToken }");

            return jwtToken;
        }
    }
}
