using drDotnet.Services.Member.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace drDotnet.Services.Member.API.Infrastructure.EntityConfigurations
{
    public class ContactEntityTypeConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contact");

            builder.Property(c => c.Id)
                .UseHiLo("contact_hilo")
                .IsRequired();

            builder.Property(c => c.Name)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Owner)
                .WithMany()
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => new { c.OwnerId, c.UserId });
        }
    }
}