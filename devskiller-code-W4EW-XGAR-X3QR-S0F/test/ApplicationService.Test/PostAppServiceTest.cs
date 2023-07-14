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
    public class PostAppServiceTest
    {
        private readonly Mock<IPostBusiness> businessMock;
        private PostAppService service;
        private readonly Mock<IMapper> mapperMock;
        const string requiredValidationMessage = "Field is required";
        protected Guid postId = Guid.NewGuid();

        public PostAppServiceTest()
        {
            businessMock = new Mock<IPostBusiness>();
            mapperMock = new Mock<IMapper>();
            service = new PostAppService(businessMock.Object, mapperMock.Object);
        }

        [Fact]
        public void Constructor_NullArgument_ThrowsArgumentNullException()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new PostAppService(null, null));
        }


        [Fact]
        public async Task GetAll_WhenExists_ReturnAllPost()
        {
            // Arrange
            var postId = Guid.NewGuid();

            var comentsDTO = CreatPostDTO(5);

            var posts = CreatPost(5);

            var result = new ResultDTO<IEnumerable<PostDTO>>(comentsDTO);

            businessMock.Setup(s => s.GetAllAsync()).ReturnsAsync(posts);

            mapperMock.Setup(mapper => mapper.Map<IEnumerable<PostDTO>>(posts)).Returns(comentsDTO);

            // Act
            var actual = await service.GetAllAsync();

            // Assert
            businessMock.Verify(s => s.GetAllAsync(), Times.Once);
            Assert.Equal(result.Content.Count(), actual.Content.Count());
        }

        [Fact]
        public async Task GetAll_PostIdIsInvalid_ThrowException()
        {
            // Arrange
            businessMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new RequiredException(requiredValidationMessage));

            // Act
            var actual = await Assert.ThrowsAsync<RequiredException>(() => service.GetAllAsync());

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(requiredValidationMessage, actual.Message);
            mapperMock.Verify(mapper => mapper.Map<PostDTO>(It.IsAny<Post>()), Times.Never());
            businessMock.Verify(s => s.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenExists_ReturnAllPost()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var postCreateDTO = new PostCreateDTO
            {
                Title = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var postDTO = new PostDTO
            {
                Id = id,
                Title = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var post = new Post
            {
                Id = id,
                Title = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var result = new ResultDTO<PostDTO>(postDTO);

            businessMock.Setup(s => s.CreateAsync(It.IsAny<Post>())).ReturnsAsync(post);

            mapperMock.Setup(mapper => mapper.Map<PostDTO>(post)).Returns(postDTO);

            // Act
            var actual = await service.CreateAsync(postCreateDTO);

            // Assert
            businessMock.Verify(s => s.CreateAsync(It.IsAny<Post>()), Times.Once);
            Assert.Equal(result.Content.Id, actual.Content.Id);
        }

        [Fact]
        public async Task CreateAsync_PostIdIsInvalid_ThrowException()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var postCreateDTO = new PostCreateDTO
            {
                Title = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var post = new Post
            {
                Id = id,
                Title = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            businessMock.Setup(s => s.CreateAsync(It.IsAny<Post>())).ThrowsAsync(new RequiredException(requiredValidationMessage));

            mapperMock.Setup(mapper => mapper.Map<Post>(post)).Returns(post);

            // Act
            var actual = await Assert.ThrowsAsync<RequiredException>(() => service.CreateAsync(postCreateDTO));

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(requiredValidationMessage, actual.Message);
            mapperMock.Verify(mapper => mapper.Map<Post>(It.IsAny<PostCreateDTO>()), Times.Once());
            mapperMock.Verify(mapper => mapper.Map<PostDTO>(It.IsAny<Post>()), Times.Never());
            businessMock.Verify(s => s.CreateAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenValid_ReturnAllPost()
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

            businessMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new RequiredException(requiredValidationMessage));

            // Act
            var actual = await Assert.ThrowsAsync<RequiredException>(() => service.DeleteAsync(postId));

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(requiredValidationMessage, actual.Message);
            businessMock.Verify(s => s.DeleteAsync(postId), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenExists_ReturnAllPost()
        {
            // Arrange

            var PostDTO = new PostDTO
            {
                Id = postId,
                Title = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var Post = new Post
            {
                Id = postId,
                Title = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var result = new ResultDTO<PostDTO>(PostDTO);

            businessMock.Setup(s => s.UpdateAsync(It.IsAny<Post>())).ReturnsAsync(Post);

            mapperMock.Setup(mapper => mapper.Map<PostDTO>(Post)).Returns(PostDTO);
            mapperMock.Setup(mapper => mapper.Map<Post>(PostDTO)).Returns(Post);

            // Act
            var actual = await service.UpdateAsync(PostDTO);

            // Assert
            businessMock.Verify(s => s.UpdateAsync(It.IsAny<Post>()), Times.Once);
            mapperMock.Verify(mapper => mapper.Map<Post>(It.IsAny<PostDTO>()), Times.Once());
            mapperMock.Verify(mapper => mapper.Map<PostDTO>(It.IsAny<Post>()), Times.Once());
            Assert.Equal(result.Content.Id, actual.Content.Id);
        }

        [Fact]
        public async Task UpdateAsync_IdIsInvalid_ThrowException()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var PostDTO = new PostDTO
            {
                Title = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var Post = new Post
            {
                Id = id,
                Title = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            businessMock.Setup(s => s.UpdateAsync(It.IsAny<Post>())).ThrowsAsync(new RequiredException(requiredValidationMessage));

            mapperMock.Setup(mapper => mapper.Map<Post>(Post)).Returns(Post);

            // Act
            var actual = await Assert.ThrowsAsync<RequiredException>(() => service.UpdateAsync(PostDTO));

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(requiredValidationMessage, actual.Message);
            mapperMock.Verify(mapper => mapper.Map<Post>(It.IsAny<PostDTO>()), Times.Once());
            mapperMock.Verify(mapper => mapper.Map<PostDTO>(It.IsAny<Post>()), Times.Never());
            businessMock.Verify(s => s.UpdateAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task Get_WhenExists_ReturnAllPost()
        {
            // Arrange
            
            var result = new ResultDTO<PostDTO>(It.IsAny<PostDTO>());
            var Post = It.IsAny<Post>();

            businessMock.Setup(s => s.GetAsync(postId)).ReturnsAsync(Post);

            // Act
            var actual = await service.GetAsync(postId);

            // Assert
            businessMock.Verify(s => s.GetAsync(postId), Times.Once);
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


        private List<PostDTO> CreatPostDTO(int many)
        {
            var Post = new List<PostDTO>();

            for (int i = 0; i < many; i++)
            {
                Post.Add(new PostDTO
                {
                    Id = Guid.NewGuid(),
                    Title = $"Author  {i}",
                    Content = $"Content {i}",
                    CreationDate = DateTime.Now
                }); ;

            }

            return Post;
        }


        private List<Post> CreatPost(int many)
        {
            var Post = new List<Post>();

            for (int i = 0; i < many; i++)
            {
                Post.Add(new Post
                {
                    Id = Guid.NewGuid(),
                    Title = $"Author  {i}",
                    Content = $"Content {i}",
                    CreationDate = DateTime.Now
                }); ;

            }

            return Post;
        }
    }
}

