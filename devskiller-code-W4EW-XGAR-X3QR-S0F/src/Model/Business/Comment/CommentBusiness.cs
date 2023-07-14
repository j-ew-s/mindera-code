using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CrossCutting.CustonException;
using Model.Entities;
using Model.Repository;

namespace Model.Business
{
    public class CommentBusiness : ICommentBusiness
    {
        private readonly ICommentRepository repository;
        private readonly IPostBusiness postBusiness;

        public CommentBusiness(ICommentRepository repository, IPostBusiness postBusiness)
        {
            this.repository = Guard.Against.Null(repository);
            this.postBusiness = Guard.Against.Null(postBusiness);
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await postBusiness.ValidatePostId(comment.PostId);

            ValidateComment(comment);

            await repository.CreateAsync(comment);

            return comment;
        }

        public async Task DeleteAsync(Guid id)
        {
            var comment = await GetAsync(id);

            if (comment != null)
            {
                throw new NotExistException("Comment not found");
            }

            repository.Delete(comment);
        }

        public async Task<Comment> GetAsync(Guid id)
        {
            if (Guid.Empty == id)
            {
                throw new RequiredException("Comment Id is required");
            }

            return await repository.GetAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId)
        {
            return  await postBusiness.GetComments(postId);
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            await ValidateCommentId(comment.Id);

            await repository.UpdateAsync(comment);

            return comment;
        }

        private void ValidateComment(Comment comment)
        {
            if (string.IsNullOrEmpty(comment.Author))
            {
                throw new RequiredException("Author is a required field");
            }

            if (string.IsNullOrEmpty(comment.Content))
            {
                throw new RequiredException("Content is a required field");
            }

            if (Guid.Empty == comment.PostId)
            {
                throw new RequiredException("Post Id is a required field");
            }
        }

        private async Task ValidateCommentId(Guid id)
        {
            if (Guid.Empty == id)
            {
                throw new RequiredException("Id is required");
            }

            var comment = await GetAsync(id);

            if (comment == null)
            {
                throw new NotExistException($"A Comment with id {id} does not exist.");
            }
        }


    }
}
