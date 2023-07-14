using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CrossCutting.CustonException;
using Model.Entities;
using Model.Repository;

namespace Model.Business
{
	public class PostBusiness : IPostBusiness
	{
        private readonly IPostRepository repository;

        public PostBusiness(IPostRepository repository)
		{
            this.repository = Guard.Against.Null(repository);
		}

        public async Task<Post> CreateAsync(Post post)
        {
            IsPostValidForCreation(post);

            return await this.repository.CreateAsync(post);
        }

        public async Task DeleteAsync(Guid id)
        {
            var post = await GetAsync(id);

            if(post == null)
            {
                throw new NotExistException("Post not found");
            }

            this.repository.Delete(post);
        }

        public async Task<Post> GetAsync(Guid id)
        {
            if(Guid.Empty == id)
            {
                throw new RequiredException("Id is required");
            }

            return await this.repository.GetAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetComments(Guid id)
        {
            var result = await GetAsync(id);

            return result?.Comments;
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await this.repository.GetAllAsync();
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            await ValidatePostId(post.Id);

            await this.repository.UpdateAsync(post);

            return post;
        }

        public async Task ValidatePostId(Guid id)
        {
            if (Guid.Empty == id)
            {
                throw new RequiredException("Id is required");
            }

            var post = await GetAsync(id);

            if (post == null)
            {
                throw new NotExistException($"A Post with id {id} does not exist.");
            }

        }

        private void IsPostValidForCreation(Post post)
        {
            if (string.IsNullOrEmpty(post.Title))
            {
                throw new RequiredException("Title is a Required field");
            }
            if (string.IsNullOrEmpty(post.Content))
            {
                throw new RequiredException("Content is a Required field");
            }
        }


    }
}
