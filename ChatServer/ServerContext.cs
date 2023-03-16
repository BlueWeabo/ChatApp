using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataClasses;
using MySql.Data.EntityFramework;
using MySql.Data.MySqlClient;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration;

namespace ChatServer
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    internal class ServerContext : DbContext
    {
        public ServerContext() : base(new MySqlConnection("server=localhost;database=chatapp;pwd=1234;uid=root").ConnectionString) { }

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
