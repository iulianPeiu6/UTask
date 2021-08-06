using System;
using System.Collections.Generic;
using UTask.Models;

namespace UTask.Services.Users
{
    public interface IUserService
    {
        User Create(User user);
        void Delete(Guid id);
        User Get(Guid id);
        User Get(string username);
        IList<User> GetAll();
        User Update(User user);
    }
}