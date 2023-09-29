using ChatGptGeneratedCodeTest.SecondTask.Controllers;
using ChatGptGeneratedCodeTest.SecondTask.Models;
using ChatGptGeneratedCodeTest.SecondTask.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChatGptGeneratedCodeTest.SecondTask.UnitTests.Controllers;

[TestClass]
public class AuthorsControllerTests
{
    private BookStoreDbContext _dbContext;
    private AuthorsController _authorsController;

    [TestInitialize]
    public void Initialize()
    {
        var options = new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseInMemoryDatabase(databaseName: $"{System.Guid.NewGuid()}")
            .Options;

        _dbContext = new BookStoreDbContext(options);
        _authorsController = new AuthorsController(_dbContext);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task GetAuthor_ReturnsAuthorIfFound()
    {
        // Arrange
        _dbContext.Authors.Add(new Author { Id = 1, Name = "Author Name" });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _authorsController.GetAuthor(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Value, typeof(Author));
        Assert.AreEqual(1, result.Value.Id);
        Assert.AreEqual("Author Name", result.Value.Name);
    }

    [TestMethod]
    public async Task GetAuthor_ReturnsNotFoundIfNotFound()
    {
        // Arrange

        // Act
        var result = await _authorsController.GetAuthor(1);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task CreateAuthor_AddsAuthorToDatabase()
    {
        // Arrange
        Author newAuthor = new Author { Name = "New Author" };

        // Act
        var result = await _authorsController.CreateAuthor(newAuthor);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        var createdAuthor = (result.Result as CreatedAtActionResult)?.Value as Author;
        Assert.IsNotNull(createdAuthor);
        Assert.AreEqual(newAuthor.Name, createdAuthor.Name);
    }

    [TestMethod]
    public async Task UpdateAuthor_UpdatesAuthorInDatabase()
    {
        // Arrange
        _dbContext.Authors.Add(new Author { Id = 1, Name = "Initial Name" });
        await _dbContext.SaveChangesAsync();

        var originalAuthor = await _dbContext.Authors.FindAsync(1);
        Assert.IsNotNull(originalAuthor);

        originalAuthor.Name = "Updated Name";

        // Act
        var updateResult = await _authorsController.UpdateAuthor(1, originalAuthor);
        var getResult = await _authorsController.GetAuthor(1);

        // Assert
        Assert.IsInstanceOfType(updateResult, typeof(NoContentResult));
        Assert.IsNotNull(getResult);
        Assert.AreEqual(originalAuthor.Id, getResult.Value.Id);
        Assert.AreEqual(originalAuthor.Name, getResult.Value.Name);
    }

    [TestMethod]
    public async Task DeleteAuthor_RemovesAuthorFromDatabase()
    {
        // Arrange
        _dbContext.Authors.Add(new Author { Id = 1, Name = "Author To Delete" });
        await _dbContext.SaveChangesAsync();

        // Act
        var deleteResult = await _authorsController.DeleteAuthor(1);
        var getResult = await _authorsController.GetAuthor(1);

        // Assert
        Assert.IsInstanceOfType(deleteResult, typeof(NoContentResult));
        Assert.IsInstanceOfType(getResult.Result, typeof(NotFoundResult));
    }
}