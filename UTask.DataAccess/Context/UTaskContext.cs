using Microsoft.EntityFrameworkCore;
using UTask.Models;

namespace UTask.DataAccess.Context
{
    public sealed class UTaskContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Settings> Settings { get; set; }

        public DbSet<Workspace> Workspaces { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public UTaskContext(DbContextOptions<UTaskContext> dbContext) : base(dbContext)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
