using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace drDotnet.Services.Contact.API.Infrastructure.EntityConfiguration
{
    public class ContactEntityTypeConfiguration : IEntityTypeConfiguration<Model.Contact>
    {
        public void Configure(EntityTypeBuilder<Model.Contact> builder)
        {
            builder.ToTable("Contact");

            builder.HasKey(c => new { c.OwnerId, c.UserId });

            builder.Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(c => c.Owner)
                .WithMany()
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}