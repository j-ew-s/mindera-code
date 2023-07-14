using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Model.Entities;
using Model.Repository;
using Repository.Context;

namespace Repository
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(BlogContext context) : base(context)
        {
        }
    }
}