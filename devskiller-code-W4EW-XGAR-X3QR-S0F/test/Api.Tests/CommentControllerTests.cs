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
using Moq;
using Xunit;
using result = Microsoft.AspNetCore.Mvc;

namespace Api.Tests
{
    public class CommentControllerTests
    {

        private readonly Mock<ICommentAppService> serviceMock;
        private CommentController commentController;
        private Fixture fixture;
        private IEnumerable<object> expected;

        public CommentControllerTests()
        {
            serviceMock = new Mock<ICommentAppService>();
            var logger = new Mock<ILogger<CommentController>>();
            commentController = new CommentController(serviceMock.Object, logger.Object);
            fixture = new Fixture();
        }


        [Fact]
        public void CommentController_Constructor_NullArgument_ThrowsArgumentNullException()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new CommentController(null, null));
        }

        [Fact]
        public async Task Get_WhenExists_ReturnComments()
        {
            // Arrange
            var commentId = Guid.NewGuid();

            var comments = new CommentDTO
            {
                Id = commentId,
                Author = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var result = new ResultDTO<CommentDTO>(comments);

            serviceMock.Setup(s => s.GetAsync(commentId)).ReturnsAsync(result);

            // Act
            var actual = await commentController.Get(commentId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual);
            Assert.Equal(result, okObjectResult.Value);
        }

        [Fact]
        public async Task GetAll_NoCommentFound_ReturnResultDTOWithoutContent()
        {
            // Arrange
            var commentid = Guid.NewGuid();

            var result = new ResultDTO<CommentDTO>(null);

            serviceMock.Setup(s => s.GetAsync(commentid)).ReturnsAsync(result);

            // Act
            var actual = await commentController.Get(commentid);

            // Assert
            Assert.IsType<result.NotFoundResult>(actual);
        }

        [Fact]
        public async Task GetAll_CommentIdIsInvalid_ReturnBadRequest()
        {
            // Arrange
            var commentId = Guid.Empty;

            // Act
            var actual = await commentController.Get(commentId);

            // Assert
            var badRequestResult = Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task Create_WhenValid_ReturnComment()
        {
            // Arrange

            var comment = new CommentCreateDTO
            {
                Author = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var result = new ResultDTO<CommentDTO>(It.IsAny<CommentDTO>());

            serviceMock.Setup(s => s.CreateAsync(comment)).ReturnsAsync(result);

            // Act
            var actual = await commentController.Post(comment);

            // Assert
            var okObjectResult = Assert.IsType<CreatedResult>(actual);
        }

        [Fact]
        public async Task Create_CommentCreateDTOIsInvalid_ReturnBadRequest()
        {
            // Arrange
            var commentCreateDTO = new CommentCreateDTO
            {
                Author = $"Author ",
                CreationDate = DateTime.Now
            };

            commentController.ModelState.AddModelError("Error", "Error");

            // Act
            var actual = await commentController.Post(commentCreateDTO);

            // Assert
            var badRequestResult = Assert.IsType<result.BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task Update_WhenValidIput_ReturnComment()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var comment = new CommentDTO
            {
                PostId = Guid.NewGuid(),
                Id = commentId,
                Author = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var result = new ResultDTO<CommentDTO>(comment);

            serviceMock.Setup(s => s.UpdateAsync(comment)).ReturnsAsync(result);

            // Act
            var actual = await commentController.Put(commentId,comment);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async Task Update_WhenMissmatchingIds_ReturnBadRequest()
        {

            // Arrange
            var commentId = Guid.NewGuid();
            var comment = new CommentDTO
            {
                PostId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Author = $"Author ",
                Content = $"Content",
                CreationDate = DateTime.Now
            };

            var result = new ResultDTO<CommentDTO>(comment);

            serviceMock.Setup(s => s.UpdateAsync(comment)).ReturnsAsync(result);

            // Act
            var actual = await commentController.Put(commentId, comment);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task Update_InvalidInput_ReturnBadRequest()
        {

            // Arrange
            var commentId = Guid.NewGuid();
            var comment = new CommentDTO
            {
                PostId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Author = $"Author ",
                CreationDate = DateTime.Now
            };

            commentController.ModelState.AddModelError("Error", "Error");

            var result = new ResultDTO<CommentDTO>(comment);

            serviceMock.Setup(s => s.UpdateAsync(comment)).ReturnsAsync(result);

            // Act
            var actual = await commentController.Put(commentId, comment);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actual);
            serviceMock.Verify(s => s.UpdateAsync(comment), Times.Never);
        }

        [Fact]
        public async Task Delete_ValidInput_ReturnBadRequest()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            serviceMock.Setup(s => s.DeleteAsync(commentId)).ReturnsAsync(It.IsAny<ResultDTO<string>>());
            // Act
            var actual = await commentController.Delete(commentId);

            // Assert
            Assert.IsType<NoContentResult>(actual);
            serviceMock.Verify(s => s.DeleteAsync(commentId), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidInput_ReturnBadRequest()
        {
            // Arrange
            var commentId = Guid.Empty;
            serviceMock.Setup(s => s.DeleteAsync(commentId)).ReturnsAsync(It.IsAny<ResultDTO<string>>());
            // Act
            var actual = await commentController.Delete(commentId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actual);
            serviceMock.Verify(s => s.DeleteAsync(commentId), Times.Never);
        }

        private IEnumerable<CommentDTO> CreatCommentDTO(int many)
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
    }
}