using Microsoft.EntityFrameworkCore;
using SmartTaskApi.Model;
using Task = SmartTaskApi.Models.Task;

namespace SmartTaskApi.Data
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Registering the Task entity
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
