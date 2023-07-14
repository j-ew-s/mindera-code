using System;
using AppServices.Models;
using AppServices.Models.Creation;
using AppServices.Models.Results;
using AppServices.Services;
using AutoMapper;
using CrossCutting.CustonException;
using Model.Business;
using Model.Entities;
using Moq;
using Xunit.Sdk;

namespace AppService.Tests
{
    public class CommentsAppServiceTest
    {
        private readonly Mock<ICommentBusiness> businessMock;
        private CommentAppService service;
        private readonly Mock<IMapper> mapperMock;
        const string requiredValidationMessage = "Field is required";

        public CommentsAppServiceTest()
        {
            businessMock = new Mock<ICommentBusiness>();
            mapperMock = new Mock<IMapper>();
            service = new CommentAppService(businessMock.Object, mapperMock.Object);
        }

        [Fact]
        public void Constructor_NullArgument_ThrowsArgumentNullException()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new CommentAppService(null, null));
        }

        [Fact]
        public async Task GetByPostIdAsync_WhenExists_ReturnAllComments()
        {
            // Arrange
            var postId = Guid.NewGuid();

            var comentsDTO = CreatCommentDTO(5);

            var coments = CreatComment(5);

            var result = new ResultDTO<IEnumerable<CommentDTO>>(comentsDTO);

            businessMock.Setup(s => s.GetByPostIdAsync(postId)).ReturnsAsync(coments);

            mapperMock.Setup(mapper => mapper.Map<IEnumerable<CommentDTO>>(coments)).Returns(comentsDTO);

            // Act
            var actual = await service.GetByPostIdAsync(postId);

            // Assert
            businessMock.Verify(s => s.GetByPostIdAsync(postId), Times.Once);
            Assert.Equal(result.Content.Count(), actual.Content.Count());
        }

        [Fact]
        public async Task GetAll_PostIdIsInvalid_ThrowException()
        {
            // Arrange
            var postId = Guid.NewGuid();

            businessMock.Setup(s => s.GetByPostIdAsync(postId)).ThrowsAsync(new RequiredException(requiredValidationMessage));

            // Act
            var actual = await Assert.ThrowsAsync<RequiredException>(() => service.GetByPostIdAsync(postId));

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(requiredValidationMessage, actual.Message);
            mapperMock.Verify(mapper => mapper.Map<CommentDTO>(It.IsAny<Comment>()), Times.Never());
            businessMock.Verify(s => s.GetByPostIdAsync(postId), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenExists_ReturnAllComments()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var comentCreateDTO = new CommentCreateDTO
            {
                PostId = postId,
                Author = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var comentDTO = new CommentDTO
            {
                PostId = postId,
                Id = id,
                Author = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var coments = new Comment
            {
                PostId = postId,
                Id = id,
                Author = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var result = new ResultDTO<CommentDTO>(comentDTO);

            businessMock.Setup(s => s.CreateAsync(It.IsAny<Comment>())).ReturnsAsync(coments);

            mapperMock.Setup(mapper => mapper.Map<CommentDTO>(coments)).Returns(comentDTO);

            // Act
            var actual = await service.CreateAsync(comentCreateDTO);

            // Assert
            businessMock.Verify(s => s.CreateAsync(It.IsAny<Comment>()), Times.Once);
            Assert.Equal(result.Content.Id, actual.Content.Id);
        }

        [Fact]
        public async Task CreateAsync_PostIdIsInvalid_ThrowException()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var commentCreateDTO = new CommentCreateDTO
            {
                PostId = postId,
                Author = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var coments = new Comment
            {
                PostId = postId,
                Id = id,
                Author = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            businessMock.Setup(s => s.CreateAsync(It.IsAny<Comment>())).ThrowsAsync(new RequiredException(requiredValidationMessage));

            mapperMock.Setup(mapper => mapper.Map<Comment>(coments)).Returns(coments);

            // Act
            var actual = await Assert.ThrowsAsync<RequiredException>(() => service.CreateAsync(commentCreateDTO));

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(requiredValidationMessage, actual.Message);
            mapperMock.Verify(mapper => mapper.Map<Comment>(It.IsAny<CommentCreateDTO>()), Times.Once());
            mapperMock.Verify(mapper => mapper.Map<CommentDTO>(It.IsAny<Comment>()), Times.Never());
            businessMock.Verify(s => s.CreateAsync(It.IsAny<Comment>()), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenValid_ReturnAllComments()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var id = Guid.NewGuid();


            var result = new ResultDTO<string>("Done");

            businessMock.Setup(s => s.DeleteAsync(id));

            // Act
            var actual = await service.DeleteAsync(id);

            // Assert
            businessMock.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_IdIsInvalid_ThrowException()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var id = Guid.NewGuid();

            businessMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new RequiredException(requiredValidationMessage));

            // Act
            var actual = await Assert.ThrowsAsync<RequiredException>(() => service.DeleteAsync(id));

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(requiredValidationMessage, actual.Message);
            businessMock.Verify(s => s.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenExists_ReturnAllComments()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var id = Guid.NewGuid();
            var commentDTO = new CommentDTO
            {
                PostId = postId,
                Id = id,
                Author = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var comments = new Comment
            {
                PostId = postId,
                Id = id,
                Author = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var result = new ResultDTO<CommentDTO>(commentDTO);

            businessMock.Setup(s => s.UpdateAsync(It.IsAny<Comment>())).ReturnsAsync(comments);

            mapperMock.Setup(mapper => mapper.Map<CommentDTO>(comments)).Returns(commentDTO);
            mapperMock.Setup(mapper => mapper.Map<Comment>(commentDTO)).Returns(comments);

            // Act
            var actual = await service.UpdateAsync(commentDTO);

            // Assert
            businessMock.Verify(s => s.UpdateAsync(It.IsAny<Comment>()), Times.Once);
            mapperMock.Verify(mapper => mapper.Map<Comment>(It.IsAny<CommentDTO>()), Times.Once());
            mapperMock.Verify(mapper => mapper.Map<CommentDTO>(It.IsAny<Comment>()), Times.Once());
            Assert.Equal(result.Content.Id, actual.Content.Id);
        }

        [Fact]
        public async Task UpdateAsync_IdIsInvalid_ThrowException()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var commentDTO = new CommentDTO
            {
                PostId = postId,
                Author = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var comment = new Comment
            {
                PostId = postId,
                Id = id,
                Author = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            businessMock.Setup(s => s.UpdateAsync(It.IsAny<Comment>())).ThrowsAsync(new RequiredException(requiredValidationMessage));

            mapperMock.Setup(mapper => mapper.Map<Comment>(comment)).Returns(comment);

            // Act
            var actual = await Assert.ThrowsAsync<RequiredException>(() => service.UpdateAsync(commentDTO));

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(requiredValidationMessage, actual.Message);
            mapperMock.Verify(mapper => mapper.Map<Comment>(It.IsAny<CommentDTO>()), Times.Once());
            mapperMock.Verify(mapper => mapper.Map<CommentDTO>(It.IsAny<Comment>()), Times.Never());
            businessMock.Verify(s => s.UpdateAsync(It.IsAny<Comment>()), Times.Once);
        }

        [Fact]
        public async Task Get_WhenExists_ReturnAllComments()
        {
            // Arrange
            var id = Guid.NewGuid();


            var result = new ResultDTO<CommentDTO>(It.IsAny<CommentDTO>());
            var comment = It.IsAny<Comment>();

            businessMock.Setup(s => s.GetAsync(id)).ReturnsAsync(comment);

            // Act
            var actual = await service.GetAsync(id);

            // Assert
            businessMock.Verify(s => s.GetAsync(id), Times.Once);
        }

        [Fact]
        public async Task Get_IdIsInvalid_ThrowException()
        {
            // Arrange

            var id = Guid.NewGuid();

            businessMock.Setup(s => s.GetAsync(id)).ThrowsAsync(new RequiredException(requiredValidationMessage));

            // Act
            var actual = await Assert.ThrowsAsync<RequiredException>(() => service.GetAsync(id));

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(requiredValidationMessage, actual.Message);
            businessMock.Verify(s => s.GetAsync(id), Times.Once);
        }


        private List<CommentDTO> CreatCommentDTO(int many)
        {
            var comments = new List<CommentDTO>();

            for (int i = 0; i < many; i++)
            {
                comments.Add(new CommentDTO
                {
                    Id = Guid.NewGuid(),
                    Author = $"Author  {i}",
                    Content = $"Content {i}",
                    CreationDate = DateTime.Now
                }); ;

            }

            return comments;
        }


        private List<Comment> CreatComment(int many)
        {
            var comments = new List<Comment>();

            for (int i = 0; i < many; i++)
            {
                comments.Add(new Comment
                {
                    Id = Guid.NewGuid(),
                    Author = $"Author  {i}",
                    Content = $"Content {i}",
                    CreationDate = DateTime.Now
                }); ;

            }

            return comments;
        }
    }
}

