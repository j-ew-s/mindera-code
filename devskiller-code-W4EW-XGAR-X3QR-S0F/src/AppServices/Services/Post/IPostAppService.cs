using AppServices.Models;
using AppServices.Models.Results;
using Model.Entities;

namespace AppServices.Services
{
	public interface IPostAppService 
    {
        public Task<ResultDTO<IEnumerable<PostDTO>>> GetAllAsync();

        public Task<ResultDTO<PostDTO>> GetAsync(Guid id);

        public Task<ResultDTO<PostDTO>> CreateAsync(PostCreateDTO entity);

        public Task<ResultDTO<PostDTO>> UpdateAsync(PostDTO entity);
        
        public Task<ResultDTO<string>> DeleteAsync(Guid id);

        public Task<ResultDTO<IEnumerable<CommentDTO>>> GetComments(Guid id);
    }
}

