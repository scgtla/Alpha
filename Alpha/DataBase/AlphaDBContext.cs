using System.Collections.Generic;
using Alpha.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Alpha.DataBase
{
    public partial class AlphaDBContext : DbContext
    {
        public AlphaDBContext()
        {
        }

        public AlphaDBContext(DbContextOptions<AlphaDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=AlphaDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new List<Role>()
            {
                new Role()
                {
                    RoleId = 1,
                    Name = "OfficeManager",
                },
                new Role()
                {
                    RoleId = 2,
                    Name = "Employee"
                }
            });
        }
    }
}