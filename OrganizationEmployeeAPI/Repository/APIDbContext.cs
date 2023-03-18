using Microsoft.EntityFrameworkCore;
using OrganizationEmployeeAPI.Models;

namespace OrganizationEmployeeAPI.Repository
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options)
        {

        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<FileDetails> FileDetails { get; set; }
    }

}
