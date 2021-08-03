using System;

namespace UTask.Models
{
    public class Settings
    {
        public Guid Id { get; set; }

        public bool NotifyMe { get; set; }

        public bool KeepMeLoggedIn { get; set; }
    }
}
