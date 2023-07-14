using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Model.Entities;
using Model.Repository;
using Repository.Context;

namespace Repository
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(BlogContext context) : base(context)
        {
        }

        public async Task<Post> GetAsync(Guid id)
        {
            return await Table.AsNoTracking().Include(i=> i.Comments).FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}