using AppServices.Models;
using AppServices.Models.Results;
using Ardalis.GuardClauses;
using AutoMapper;
using Model.Business;
using Model.Entities;

namespace AppServices.Services
{
	public class PostAppService : IPostAppService
	{
        private readonly IPostBusiness business;
        private readonly IMapper mapper;

        public PostAppService(IPostBusiness business, IMapper mapper)
        {
			this.business = Guard.Against.Null(business);
			this.mapper = Guard.Against.Null(mapper);
		}

        public async Task<ResultDTO<PostDTO>> CreateAsync(PostCreateDTO entity)
        {
            var post = mapper.Map<Post>(entity);

            var businessResult = await business.CreateAsync(post);

            return BaseReturn(businessResult);
        }

        public async Task<ResultDTO<PostDTO>> UpdateAsync(PostDTO entity)
        {
            var post = mapper.Map<Post>(entity);

            var businessResult = await business.UpdateAsync(post);

            return BaseReturn(businessResult);
        }

        public async Task<ResultDTO<string>> DeleteAsync(Guid id)
        {
            await business.DeleteAsync(id);

            return new ResultDTO<string>($"Post with {id} has been deleted.");
        }

        public async Task<ResultDTO<PostDTO>> GetAsync(Guid id)
        {
            var post = await business.GetAsync(id);

            return BaseReturn(post);
        }

        public async Task<ResultDTO<IEnumerable<PostDTO>>> GetAllAsync()
        {
            var businessResult = await business.GetAllAsync();

            var result = mapper.Map<IEnumerable<PostDTO>>(businessResult);

            return new ResultDTO<IEnumerable<PostDTO>>(result);
        }

        public async Task<ResultDTO<IEnumerable<CommentDTO>>> GetComments(Guid id)
        {
            var comments = await business.GetComments(id);

            var result = mapper.Map<IEnumerable<CommentDTO>>(comments);

            return new ResultDTO<IEnumerable<CommentDTO>>(result);
        }

        /// <summary>
        /// Execute Mapper and create the ResultDTO obeject after Business Execution Result.
        /// </summary>
        /// <param name="post">Post</param>
        /// <returns>The Mapped Comment busines object to a ResultDTO<PostDTO></returns>
        private ResultDTO<PostDTO> BaseReturn(Post post)
        {
            var result = mapper.Map<PostDTO>(post);

            return new ResultDTO<PostDTO>(result);
        }
    }
}
