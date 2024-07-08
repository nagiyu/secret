using Microsoft.EntityFrameworkCore;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SecretDbContext : DbContext
    {
        public SecretDbContext(DbContextOptions<SecretDbContext> options) : base(options) { }

        public DbSet<Secret> Secrets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Secret>().ToTable("Secrets");
        }
    }
}
