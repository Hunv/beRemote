using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beRemote.Database
{
    public class brDbContext : DbContext
    {
        public brDbContext()
        {
            System.Data.Entity.Database.SetInitializer<brDbContext>(null);
        }

        public DbSet<TblConnectionHost> ConnectionHosts { get; set; }
        public DbSet<TblConnectionProtocols> ConnectionProtocols { get; set; }
        public DbSet<TblConnectionSettings> ConnectionSettings { get; set; }
        public DbSet<TblCredentials> Credentials { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Database does not pluralize table names
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
