using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportPlaces.Models
{
    public class EntitiesContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SportObject> SportObjects { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<SportKind> SportKinds { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Record> Records { get; set; }

        public EntitiesContext(DbContextOptions<EntitiesContext> option) : base(option)
        {
            Database.EnsureCreated();
        }
    }
}
