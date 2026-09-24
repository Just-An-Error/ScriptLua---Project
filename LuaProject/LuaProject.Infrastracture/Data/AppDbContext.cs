using LuaProject.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace LuaProject.Infrastracture.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Execution> Executions { get; set; }
        public DbSet<RuleVersion> RuleVersions { get; set; }
    }
}
