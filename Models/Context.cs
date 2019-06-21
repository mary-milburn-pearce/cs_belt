using System;
using Microsoft.EntityFrameworkCore;

namespace cs_belt.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options) {
            
        }
        public DbSet<User> Users { get; set; }
        // public DbSet<Wedding> Weddings { get; set; }
        // public DbSet<Response> Responses { get; set; }
    }
}