using drDotnet.Services.Contact.API.Infrastructure.EntityConfiguration;
using drDotnet.Services.Contact.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace drDotnet.Services.Contact.API.Infrastructure
{
    public class ContactContext : DbContext
    {
        public ContactContext(DbContextOptions<ContactContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Model.Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ContactEntityTypeConfiguration());
        }

        public class ContactContextDesignFactory : IDesignTimeDbContextFactory<ContactContext>
        {
            public ContactContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ContactContext>()
                    .UseSqlServer("Server=localhost; Database=drDotnetContact; User=sa; Password=Meysam@1374");

                return new ContactContext(optionsBuilder.Options);
            }
        }
    }
}