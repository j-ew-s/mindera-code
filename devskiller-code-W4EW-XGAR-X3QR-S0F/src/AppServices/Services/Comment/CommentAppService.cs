using AppServices.Models;
using AppServices.Models.Creation;
using AppServices.Models.Results;
using Ardalis.GuardClauses;
using AutoMapper;
using Model.Business;
using Model.Entities;

namespace AppServices.Services
{
	public class CommentAppService : ICommentAppService
	{
        private readonly ICommentBusiness business;
        private readonly IMapper mapper;

        public CommentAppService(ICommentBusiness business, IMapper mapper)
		{
            this.business = Guard.Against.Null(business);
            this.mapper = Guard.Against.Null(mapper);
		}

        public async Task<ResultDTO<CommentDTO>> CreateAsync(CommentCreateDTO entity)
        {
            var comment = mapper.Map<Comment>(entity);

            var businessResult = await business.CreateAsync(comment);

            return BaseReturn(businessResult);
        }
        
        public async Task<ResultDTO<string>> DeleteAsync(Guid id)
        {
            await business.DeleteAsync(id);

            return new ResultDTO<string>($"Comment with {id} has been deleted.");
        }

        public async Task<ResultDTO<CommentDTO>> UpdateAsync(CommentDTO entity)
        {
            var comment = mapper.Map<Comment>(entity);

            var businessResult = await business.UpdateAsync(comment);

            return BaseReturn(businessResult);
        }

        public async Task<ResultDTO<CommentDTO>> GetAsync(Guid id)
        {
            var comment = await business.GetAsync(id);

            return BaseReturn(comment);
        }

        public async Task<ResultDTO<IEnumerable<CommentDTO>>> GetAllAsync(Guid postId)
        {
            var businessResult = await business.GetByPostIdAsync(postId);

            var result = mapper.Map<IEnumerable<CommentDTO>>(businessResult);

            return new ResultDTO<IEnumerable<CommentDTO>>(result);
        }

        public async Task<ResultDTO<IEnumerable<CommentDTO>>> GetByPostIdAsync(Guid postId)
        {
            var businessResult = await business.GetByPostIdAsync(postId);

            var result = mapper.Map<IEnumerable<CommentDTO>>(businessResult);

            return new ResultDTO<IEnumerable<CommentDTO>>(result);
        }

        /// <summary>
        /// Execute Mapper and create the ResultDTO obeject after Business Execution Result.
        /// </summary>
        /// <param name="comment">Comment</param>
        /// <returns>The Mapped Comment busines object to a ResultDTO<CommentDTO></returns>
        private ResultDTO<CommentDTO> BaseReturn(Comment comment)
        {
            var result = mapper.Map<CommentDTO>(comment);

            return new ResultDTO<CommentDTO>(result);
        }
    }
}

