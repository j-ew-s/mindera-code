using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Entities;

namespace Model.Business
{
	public interface IPostBusiness:IBaseBusiness<Post>
	{
        Task<IEnumerable<Comment>> GetComments(Guid id);

        Task ValidatePostId(Guid id);
    }
}

