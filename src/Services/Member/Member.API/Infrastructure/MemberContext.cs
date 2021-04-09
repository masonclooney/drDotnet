using drDotnet.Services.Member.API.Infrastructure.EntityConfigurations;
using drDotnet.Services.Member.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace drDotnet.Services.Member.API.Infrastructure
{
    public class MemberContext : DbContext
    {
        public MemberContext(DbContextOptions<MemberContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ContactEntityTypeConfiguration());
        }

        public class MemberContextDesignFactory : IDesignTimeDbContextFactory<MemberContext>
        {
            public MemberContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<MemberContext>()
                    .UseSqlServer("Server=localhost; Database=drDotnetMember; User=sa; Password=Meysam@1374");

                return new MemberContext(optionsBuilder.Options);
            }
        }
    }
}