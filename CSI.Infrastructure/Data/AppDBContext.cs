using CSI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.Infrastructure.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) 
        {
            Analytics = Set<Analytics>();
            Departments = Set<Department>();
        }

        public DbSet<Analytics> Analytics { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Analytics>()
                .ToTable("tbl_analytics");

            modelBuilder.Entity<Department>()
                .ToTable("tbl_department_code");
        }
    }
}
