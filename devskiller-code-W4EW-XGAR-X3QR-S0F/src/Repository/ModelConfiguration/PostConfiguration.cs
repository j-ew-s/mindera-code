
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Repository.ModelConfiguration
{
	public class PostConfiguration : IEntityTypeConfiguration<Post>
    {

        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");

            builder.HasKey(k => k.Id);

            builder.HasIndex(i => i.Id).IsUnique();

            builder.Property(p => p.Title).IsRequired();

            builder.Property(p => p.Content).IsRequired();

            builder.Property(p => p.CreationDate).IsRequired();

            builder.HasMany(m => m.Comments)
                .WithOne(m => m.Post)
                .HasForeignKey(f => f.PostId)
                .IsRequired();

        }
    }
}

