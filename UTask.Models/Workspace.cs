using System;
using System.Collections.Generic;

namespace UTask.Models
{
    public class Workspace
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Visibility { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}
