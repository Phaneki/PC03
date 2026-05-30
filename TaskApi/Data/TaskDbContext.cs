using Microsoft.EntityFrameworkCore;
using TaskApi.Models.Entities;

namespace TaskApi.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
        }

        public DbSet<Tarea> Tareas { get; set; }
    }
}
