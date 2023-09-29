using ChatGptGeneratedCodeTest.SecondTask.Controllers;
using ChatGptGeneratedCodeTest.SecondTask.Models;
using ChatGptGeneratedCodeTest.SecondTask.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChatGptGeneratedCodeTest.SecondTask.UnitTests.Controllers;

[TestClass]
public class GenresControllerTests
{
    private BookStoreDbContext _dbContext;
    private GenresController _genresController;

    [TestInitialize]
    public void Initialize()
    {
        var options = new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseInMemoryDatabase(databaseName: $"{System.Guid.NewGuid()}")
            .Options;

        _dbContext = new BookStoreDbContext(options);
        _genresController = new GenresController(_dbContext);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext?.Dispose();
    }

    [TestMethod]
    public async Task GetGenres_ReturnsAllGenres()
    {
        // Arrange
        _dbContext.Genres.Add(new Genre { Id = 1, Name = "Fiction" });
        _dbContext.Genres.Add(new Genre { Id = 2, Name = "Non-Fiction" });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _genresController.GetGenres();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Value.Count());
    }

    [TestMethod]
    public async Task GetGenre_ReturnsGenreIfFound()
    {
        // Arrange
        _dbContext.Genres.Add(new Genre { Id = 1, Name = "Fiction" });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _genresController.GetGenre(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Value, typeof(Genre));
        Assert.AreEqual(1, result.Value.Id);
        Assert.AreEqual("Fiction", result.Value.Name);
    }

    [TestMethod]
    public async Task GetGenre_ReturnsNotFoundIfNotFound()
    {
        // Arrange

        // Act
        var result = await _genresController.GetGenre(1);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task CreateGenre_AddsGenreToDatabase()
    {
        // Arrange
        Genre newGenre = new Genre { Name = "Fiction" };

        // Act
        var result = await _genresController.CreateGenre(newGenre);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        var createdGenre = (result.Result as CreatedAtActionResult)?.Value as Genre;
        Assert.IsNotNull(createdGenre);
        Assert.AreEqual(newGenre.Name, createdGenre.Name);
    }

    [TestMethod]
    public async Task UpdateGenre_UpdatesGenreInDatabase()
    {
        // Arrange
        _dbContext.Genres.Add(new Genre { Id = 1, Name = "Fiction" });
        await _dbContext.SaveChangesAsync();

        var originalGenre = await _dbContext.Genres.FindAsync(1);
        Assert.IsNotNull(originalGenre);

        originalGenre.Name = "Updated Fiction";

        // Act
        var updateResult = await _genresController.UpdateGenre(1, originalGenre);
        var getResult = await _genresController.GetGenre(1);

        // Assert
        Assert.IsInstanceOfType(updateResult, typeof(NoContentResult));
        Assert.IsNotNull(getResult);
        Assert.AreEqual(originalGenre.Id, getResult.Value.Id);
        Assert.AreEqual(originalGenre.Name, getResult.Value.Name);
    }

    [TestMethod]
    public async Task DeleteGenre_RemovesGenreFromDatabase()
    {
        // Arrange
        _dbContext.Genres.Add(new Genre { Id = 1, Name = "Fiction" });
        await _dbContext.SaveChangesAsync();

        // Act
        var deleteResult = await _genresController.DeleteGenre(1);
        var getResult = await _genresController.GetGenre(1);

        // Assert
        Assert.IsInstanceOfType(deleteResult, typeof(NoContentResult));
        Assert.IsInstanceOfType(getResult.Result, typeof(NotFoundResult));
    }
}