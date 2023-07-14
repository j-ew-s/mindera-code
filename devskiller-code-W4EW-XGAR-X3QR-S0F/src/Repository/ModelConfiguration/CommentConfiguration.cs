using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Repository.ModelConfiguration
{
	public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comment");

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Author).IsRequired();

            builder.Property(p => p.Content).IsRequired();

            builder.Property(p => p.CreationDate).IsRequired();

            builder.HasOne(o => o.Post)
                .WithMany(m => m.Comments)
                .HasForeignKey(f => f.PostId)
                .IsRequired();
        }
    }
}

