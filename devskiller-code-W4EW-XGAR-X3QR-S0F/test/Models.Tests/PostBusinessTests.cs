using CrossCutting.CustonException;
using Model.Business;
using Model.Entities;
using Model.Repository;
using Moq;

namespace Models.Tests;

public class PostBusinessTests
{
    private Mock<IPostRepository> repository;
    private PostBusiness business;
    private Guid  postId = Guid.NewGuid();
    private Guid id = Guid.NewGuid();



    public  PostBusinessTests()
    {
        repository = new Mock<IPostRepository>();
        business = new PostBusiness(repository.Object);
    }

    [Fact]
    public void Constructor_NullArgument_ThrowsArgumentNullException()
    {
        // Act and assert
        Assert.Throws<ArgumentNullException>(() => new PostBusiness(null));
    }

    [Fact]
    public async Task Create_WhenPostValid_ReturPost()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var coment = new Post
        {
            Title = $"Title ",
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
    public async Task Create_WhenPostInvalid_ThrowError()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var Post = new Post
        {
            Content = $"Content",
            CreationDate = DateTime.Now
        };

        repository.Setup(s => s.CreateAsync(Post)).ReturnsAsync(Post);

        // Act
        var actual = await Assert.ThrowsAsync<RequiredException>(() => business.CreateAsync(Post));

        // Assert
        repository.Verify(s => s.CreateAsync(Post), Times.Never);
    }



    [Fact]
    public async Task Update_WhenPostValid_ReturPost()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var Post = new Post
        {
            Id = id, 
            Title = $"Title ",
            Content = $"Content",
            CreationDate = DateTime.Now
        };

        repository.Setup(s => s.UpdateAsync(Post));

        // Act
        var actual = await business.CreateAsync(Post);

        // Assert
        repository.Verify(s => s.CreateAsync(Post), Times.Once);
    }

    [Fact]
    public async Task Update_WhenPostInvalid_ThrowError()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var Post = new Post
        {
            Content = $"Content",
            CreationDate = DateTime.Now
        };

        repository.Setup(s => s.UpdateAsync(Post));

        // Act
        var actual = await Assert.ThrowsAsync<RequiredException>(() => business.UpdateAsync(Post));

        // Assert
        repository.Verify(s => s.CreateAsync(Post), Times.Never);
    }





    [Fact]
    public async Task Get_WhenPostValid_ReturPost()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var Post = new Post
        {
            Id = id,
            Title = $"Title ",
            Content = $"Content",
            CreationDate = DateTime.Now
        };

        repository.Setup(s => s.GetAsync(id)).ReturnsAsync(Post);

        // Act
        var actual = await business.GetAsync(id);

        // Assert
        repository.Verify(s => s.GetAsync(id), Times.Once);
    }

    [Fact]
    public async Task Get_WhenPostInvalid_ThrowError()
    {
        // Arrange
        repository.Setup(s => s.GetAsync(It.IsAny<Guid>()));

        // Act
        var actual = await Assert.ThrowsAsync<RequiredException>(() => business.GetAsync(Guid.Empty));

        // Assert
        repository.Verify(s => s.GetAsync(It.IsAny<Guid>()), Times.Never);
    }

   

    private List<Post> CreatPost(int many)
    {
        var Posts = new List<Post>();

        for (int i = 0; i < many; i++)
        {
            Posts.Add(new Post
            {
                Id = Guid.NewGuid(),
                Title = $"Title  {i}",
                Content = $"Content {i}",
                CreationDate = DateTime.Now
            }); ;

        }

        return Posts;
    }

}
