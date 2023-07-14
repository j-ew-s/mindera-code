using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Entities;

namespace Model.Business
{
	public interface ICommentBusiness : IBaseBusiness<Comment>
	{
        public Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId);
    }
}

