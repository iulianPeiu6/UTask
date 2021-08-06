using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using UTask.DataAccess;
using UTask.Models;
using Vanguard;

namespace UTask.Services.Users
{
    public class UserService : IUserService
    {
        private readonly ILogger<IUserService> logger;

        private readonly IRepository<User> userRepository;

        private readonly IRepository<Settings> settingsRepository;

        private readonly IUnitOfWork unitOfWork;

        public UserService(ILogger<IUserService> logger, IRepository<User> userRepository, IRepository<Settings> settingsRepository, IUnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.settingsRepository = settingsRepository;
            this.unitOfWork = unitOfWork;
        }

        public User Create(User user)
        {
            Guard.ArgumentNotNull(user, nameof(user), "User can not be null");
            Guard.ArgumentNotNullOrEmpty(user.Username, nameof(user.Username), "Username can not be null or empty");
            Guard.ArgumentNotNullOrEmpty(user.Password, nameof(user.Password), "Password can not be null or empty");

            User createdUser = null;
            try
            {
                logger.LogInformation("Start adding new user in database");
                createdUser = userRepository.Add(user);
                unitOfWork.Commit();
                logger.LogInformation($"User created: Id \"{ createdUser.Id }\", Username \"{ createdUser.Username }\"");
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Could not add user in the database!");
            }

            return createdUser;
        }

        public IList<User> GetAll()
        {
            logger.LogInformation("Start Getting all the user from database");

            var users = userRepository.GetAll();

            logger.LogInformation($"Got { users.Count } users from database");

            return users;
        }

        public User Get(string username)
        {
            logger.LogInformation($"Start Getting User with Username \"{ username }\" from database ...");
            Guard.ArgumentNotNullOrEmpty(username, nameof(username), "Usename can not be null or empty");

            var user = userRepository.Query()
                .Where(user => user.Username == username)
                .FirstOrDefault();

            if (user == null)
            {
                logger.LogWarning($"User with UserName \"{ username }\" does not exists");
                return null;
            }

            logger.LogInformation($"User with username \"{ username }\" was found. Id is \"{ user.Id }\"");

            return user;
        }

        public User Get(Guid id)
        {
            logger.LogInformation($"Getting User with Id \"{ id }\" from database ...");
            Guard.ArgumentNotNullOrEmpty(id, nameof(id), "User id can not be null or empty");

            var user = userRepository.GetById(id);

            if (user == null)
            {
                logger.LogWarning($"User with Id \"{ id }\" does not exists");
                return null;
            }


            logger.LogInformation($"User with id \"{ user.Id }\" was found. Username is \"{ user.Username }\"");

            return user;
        }

        public User Update(User user)
        {
            Guard.ArgumentNotNull(user, nameof(user), "User can not be null");
            Guard.ArgumentNotNullOrEmpty(user.Id, nameof(user.Id), "Id can not be null or empty");
            Guard.ArgumentNotNullOrEmpty(user.Username, nameof(user.Username), "Username can not be null or empty");
            Guard.ArgumentNotNullOrEmpty(user.Password, nameof(user.Password), "Password can not be null or empty");
            try
            {
                logger.LogInformation($"Start updating User with Id \"{ user.Id }\" ...");
                var updatedUser = userRepository.Update(user);
                unitOfWork.Commit();
                logger.LogInformation($"User updated successfully");
                return user;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, $"Could not update user with Id \"{ user.Id }\"");
                return null;
            }
        }

        public bool Delete(Guid id)
        {
            Guard.ArgumentNotNullOrEmpty(id, nameof(id), "Id can not be null or empty");
            logger.LogInformation($"Start getting User with Id = \"{ id }\" from Database");

            var user = userRepository.GetById(id);

            if (user == null)
            {
                logger.LogInformation($"Didn't find any User with Id \"{ id }\" in Database");
                return false;
            }

            logger.LogInformation($"Got user entity with Id \"{ id }\". Username is \"{ user.Username }\"");

            try
            {
                logger.LogInformation($"Start deleting User with Id = \"{ id }\" from Database");
                userRepository.Delete(user);
                unitOfWork.Commit();
                logger.LogInformation($"User with Id = \"{ id }\" is deleted from database");
                return true;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, $"User with Id \"{ id }\" caould not be deleted");
                return false;
            }
        }
    }
}
