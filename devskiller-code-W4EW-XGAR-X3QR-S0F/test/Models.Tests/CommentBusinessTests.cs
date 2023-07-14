using CrossCutting.CustonException;
using Model.Business;
using Model.Entities;
using Model.Repository;
using Moq;

namespace Models.Tests;

public class CommentBusinessTests
{
    private Mock<ICommentRepository> repository;
    private Mock<IPostBusiness> postBusiness;
    private CommentBusiness business;
    private Guid  postId = Guid.NewGuid();
    private Guid id = Guid.NewGuid();



    public CommentBusinessTests()
    {
        repository = new Mock<ICommentRepository>();
        postBusiness = new Mock<IPostBusiness>();
        business = new CommentBusiness(repository.Object, postBusiness.Object);
    }

    [Fact]
    public void Constructor_NullArgument_ThrowsArgumentNullException()
    {
        // Act and assert
        Assert.Throws<ArgumentNullException>(() => new CommentBusiness(null, null));
    }

    [Fact]
    public async Task Create_WhenCommentValid_ReturComment()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var coment = new Comment
        {
            PostId = postId,
            Author = $"Author ",
            Content = $"Content",
            CreationDate = DateTime.Now
        };

        repository.Setup(s => s.CreateAsync(coment)).ReturnsAsync(coment);

        // Act
        var actual = await business.CreateAsync(coment);

        // Assert
        repository.Verify(s => s.CreateAsync(coment), Times.Once);
    }

    [Fact]
    public async Task Create_WhenCommentInvalid_ThrowError()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var comment = new Comment
        {
            PostId = postId,
            Content = $"Content",
            CreationDate = DateTime.Now
        };

        repository.Setup(s => s.CreateAsync(comment)).ReturnsAsync(comment);

        // Act
        var actual = await Assert.ThrowsAsync<RequiredException>(() => business.CreateAsync(comment));

        // Assert
        repository.Verify(s => s.CreateAsync(comment), Times.Never);
    }



    [Fact]
    public async Task Update_WhenCommentValid_ReturComment()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var comment = new Comment
        {
            PostId = postId,
            Id = id, 
            Author = $"Author ",
            Content = $"Content",
            CreationDate = DateTime.Now
        };

        repository.Setup(s => s.UpdateAsync(comment));

        // Act
        var actual = await business.CreateAsync(comment);

        // Assert
        repository.Verify(s => s.CreateAsync(comment), Times.Once);
    }

    [Fact]
    public async Task Update_WhenCommentInvalid_ThrowError()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var comment = new Comment
        {
            PostId = postId,
            Content = $"Content",
            CreationDate = DateTime.Now
        };

        repository.Setup(s => s.UpdateAsync(comment));

        // Act
        var actual = await Assert.ThrowsAsync<RequiredException>(() => business.UpdateAsync(comment));

        // Assert
        repository.Verify(s => s.CreateAsync(comment), Times.Never);
    }





    [Fact]
    public async Task Get_WhenCommentValid_ReturComment()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var comment = new Comment
        {
            PostId = postId,
            Id = id,
            Author = $"Author ",
            Content = $"Content",
            CreationDate = DateTime.Now
        };

        repository.Setup(s => s.GetAsync(id)).ReturnsAsync(comment);

        // Act
        var actual = await business.GetAsync(id);

        // Assert
        repository.Verify(s => s.GetAsync(id), Times.Once);
    }

    [Fact]
    public async Task Get_WhenCommentInvalid_ThrowError()
    {
        // Arrange
        repository.Setup(s => s.GetAsync(It.IsAny<Guid>()));

        // Act
        var actual = await Assert.ThrowsAsync<RequiredException>(() => business.GetAsync(Guid.Empty));

        // Assert
        repository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task GetByPostIdAsync_WhenCommentValid_ReturComment()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var comment = new Comment
        {
            PostId = postId,
            Id = id,
            Author = $"Author ",
            Content = $"Content",
            CreationDate = DateTime.Now
        };

        postBusiness.Setup(s => s.GetComments(postId)).ReturnsAsync(CreatComment(5));

        // Act
        var actual = await business.GetByPostIdAsync(postId);

        // Assert
        postBusiness.Verify(s => s.GetComments(postId), Times.Once);
    }

    [Fact]
    public async Task GetByPostIdAsync_WhenCommentInvalid_ThrowError()
    {
        // Arrange
        var postId = Guid.Empty;

        postBusiness.Setup(s => s.GetComments(postId)).ThrowsAsync(new RequiredException("some message"));

        // Act
        var actual = await Assert.ThrowsAsync<RequiredException>(() => business.GetByPostIdAsync(postId));

        // Assert
        postBusiness.Verify(s => s.GetComments(postId), Times.Once);
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
