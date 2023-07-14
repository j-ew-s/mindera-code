using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Repository.ModelConfiguration;

namespace Repository.Context
{
    // this is used for our verification tests, don't rename or change the access modifier
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Post> Posts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
        }
    }
}