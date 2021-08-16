using System;

namespace UTask.Models
{
    public class Task
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public Guid WorkspaceId { get; set; }

        public Workspace Workspace { get; set; }
    }
}
