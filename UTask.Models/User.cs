using System;
using System.Collections.Generic;

namespace UTask.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public ICollection<Workspace> Workspaces { get; set; }
    }
}
