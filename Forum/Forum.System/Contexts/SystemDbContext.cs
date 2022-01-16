using Forum.Membership.Entities;
using Forum.System.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.Contexts
{
    public class SystemDbContext : DbContext, ISystemDbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public SystemDbContext(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                dbContextOptionsBuilder.UseSqlServer(
                    _connectionString,
                    m => m.MigrationsAssembly(_migrationAssemblyName));
            }

            base.OnConfiguring(dbContextOptionsBuilder);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ApplicationUser>()
        //        .ToTable("AspNetUsers", t => t.ExcludeFromMigrations())
        //        .HasMany<Comment>()
        //        .WithOne(g => g.ApplicationUser);

        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<Board> Boards { get; set; }
        public DbSet<Topic> Topics { get; set; }

    }
}