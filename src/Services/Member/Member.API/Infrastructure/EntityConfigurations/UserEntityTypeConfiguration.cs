using drDotnet.Services.Member.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace drDotnet.Services.Member.API.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .UseHiLo("user_hilo")
                .IsRequired();

            builder.Property(u => u.Name)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(u => u.Sub)
                .IsRequired(true);
            
            builder.HasIndex(u => u.Sub)
                .IsUnique();

            builder.HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}