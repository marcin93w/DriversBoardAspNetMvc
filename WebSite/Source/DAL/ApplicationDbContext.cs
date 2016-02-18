using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using Driver.WebSite.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Driver.WebSite.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public ApplicationDbContext(DbConnection connetion)
            :base(connetion, true)
        {
        }

        public DbSet<Item> Items { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .Property(i => i.UpVotesCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            modelBuilder.Entity<Item>()
                .Property(i => i.DownVotesCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<Driver.WebSite.Models.Comment> Comments { get; set; }
    }
}
