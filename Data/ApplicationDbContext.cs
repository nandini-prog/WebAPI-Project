using Microsoft.EntityFrameworkCore;
using Task = SmartTaskApi.Models.Task;

namespace SmartTaskApi.Data
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Registering the Task entity
        public DbSet<Task> Tasks { get; set; }
    }
}
