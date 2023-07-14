using AppServices.Models;
using AppServices.Models.Creation;
using AppServices.Models.Results;

namespace AppServices.Services
{
	public interface ICommentAppService 
	{
        public Task<ResultDTO<IEnumerable<CommentDTO>>> GetAllAsync(Guid postId);

        public Task<ResultDTO<CommentDTO>> GetAsync(Guid id);

        public Task<ResultDTO<CommentDTO>> CreateAsync(CommentCreateDTO entity);

        public Task<ResultDTO<CommentDTO>> UpdateAsync(CommentDTO entity);

        public Task<ResultDTO<string>> DeleteAsync(Guid id);

        public Task<ResultDTO<IEnumerable<CommentDTO>>> GetByPostIdAsync(Guid postId);
    }
}

