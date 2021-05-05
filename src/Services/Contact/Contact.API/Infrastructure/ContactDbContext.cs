using drDotnet.Services.Contact.API.Infrastructure.EntityConfiguration;
using drDotnet.Services.Contact.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace drDotnet.Services.Contact.API.Infrastructure
{
    public class ContactDbContext : DbContext
    {
        public ContactDbContext(DbContextOptions<ContactDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Model.Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ContactEntityTypeConfiguration());
        }

        public class ContactDbContextDesignFactory : IDesignTimeDbContextFactory<ContactDbContext>
        {
            public ContactDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ContactDbContext>()
                    .UseSqlServer("Server=localhost; Database=drDotnetContact; User=sa; Password=Meysam@1374");

                return new ContactDbContext(optionsBuilder.Options);
            }
        }
    }
}