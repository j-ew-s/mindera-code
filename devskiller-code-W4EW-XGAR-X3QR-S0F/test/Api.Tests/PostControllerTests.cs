using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Controllers;
using AppServices.Models;
using AppServices.Models.Creation;
using AppServices.Models.Results;
using AppServices.Services;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Entities;
using Moq;
using Xunit;

namespace Api.Tests
{
    public class PostControllerTests
    {
        private readonly Mock<IPostAppService> serviceMock;
        private PostController controller;
        private IEnumerable<object> expected;
        Guid id = Guid.NewGuid();

        public PostControllerTests()
        {
            serviceMock = new Mock<IPostAppService>();
            var logger = new Mock<ILogger<PostController>>();
            controller = new PostController(serviceMock.Object, logger.Object);
        }

        [Fact]
        public void PostController_Constructor_NullArgument_ThrowsArgumentNullException()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new PostController(null, null));
        }

        [Fact]
        public async Task Get_WhenExists_ReturnComments()
        {
            // Arrange
            var postDTO = new PostDTO
            {
                Id = id,
                Title = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var result = new ResultDTO<PostDTO>(postDTO);

            serviceMock.Setup(s => s.GetAsync(id)).ReturnsAsync(result);

            // Act
            var actual = await controller.Get(id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual);
            Assert.Equal(result, okObjectResult.Value);
        }

        [Fact]
        public async Task GetAll_NoPostFound_ReturnResultDTOWithoutContent()
        {
            // Arrange
            var result = new ResultDTO<PostDTO>(null);

            serviceMock.Setup(s => s.GetAsync(id)).ReturnsAsync(result);

            // Act
            var actual = await controller.Get(id);

            // Assert
            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task GetAll_PostIdIsInvalid_ReturnBadRequest()
        {
            // Arrange
            // Act
            var actual = await controller.Get(Guid.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task Create_WhenValid_ReturnPost()
        {
            // Arrange

            var post = new PostCreateDTO
            {
                Title = $"Title ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };


            var postDto = new PostDTO
            {
                Title = $"Title ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var result = new ResultDTO<PostDTO>(postDto);

            serviceMock.Setup(s => s.CreateAsync(It.IsAny<PostCreateDTO>())).ReturnsAsync(result);

            // Act
            var actual = await controller.Post(post);

            // Assert
            Assert.IsType<CreatedResult>(actual);
        }

        [Fact]
        public async Task Create_PostCreateDTOIsInvalid_ReturnBadRequest()
        {
            // Arrange
            var commentCreateDTO = new PostCreateDTO
            {
                Title = $"Author ",
                CreationDate = DateTime.Now
            };

            controller.ModelState.AddModelError("Error", "Error");

            // Act
            var actual = await controller.Post(commentCreateDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task Update_WhenValidIput_ReturnPost()
        {
            // Arrange
            var post = new PostDTO
            {

                Id = id,
                Title = $"Title ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var result = new ResultDTO<PostDTO>(post);

            serviceMock.Setup(s => s.UpdateAsync(post)).ReturnsAsync(result);

            // Act
            var actual = await controller.Put(id, post);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async Task Update_WhenMissmatchingIds_ReturnBadRequest()
        {

            // Arrange
            var post = new PostDTO
            {
                Id = Guid.NewGuid(),
                Title = $"Title ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var result = new ResultDTO<PostDTO>(post);

            serviceMock.Setup(s => s.UpdateAsync(post)).ReturnsAsync(result);

            // Act
            var actual = await controller.Put(id, post);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task Update_InvalidInput_ReturnBadRequest()
        {

            // Arrange
            var post = new PostDTO
            {
                Id = Guid.NewGuid(),
                Title = $"Author ",
                CreationDate = DateTime.Now
            };

            controller.ModelState.AddModelError("Error", "Error");

            var result = new ResultDTO<PostDTO>(post);

            serviceMock.Setup(s => s.UpdateAsync(post)).ReturnsAsync(result);

            // Act
            var actual = await controller.Put(id, post);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actual);
            serviceMock.Verify(s => s.UpdateAsync(post), Times.Never);
        }

        [Fact]
        public async Task Delete_ValidInput_ReturnBadRequest()
        {
            // Arrange
            serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(It.IsAny<ResultDTO<string>>());
            // Act
            var actual = await controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(actual);
            serviceMock.Verify(s => s.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidInput_ReturnBadRequest()
        {
            // Arrange
            var postId = Guid.Empty;
            serviceMock.Setup(s => s.DeleteAsync(postId)).ReturnsAsync(It.IsAny<ResultDTO<string>>());
            // Act
            var actual = await controller.Delete(postId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actual);
            serviceMock.Verify(s => s.DeleteAsync(postId), Times.Never);
        }

        private IEnumerable<PostDTO> CreatPostDTO(int many)
        {
            var comments = new List<PostDTO>();

            for (int i = 0; i < many; i++)
            {
                comments.Add(new PostDTO
                {
                    Id = Guid.NewGuid(),
                    Title = $"Author  {i}",
                    Content = $"Content {i}",
                    CreationDate = DateTime.Now
                }); ;

            }

            return comments;
        }
    }
}