using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataClasses;

namespace ChatServer
{
    internal class ServerContext : DbContext
    {
        public ServerContext() : base("name=ServerContext") { }

        public DbSet<User> Users { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
