using ChatGptGeneratedCodeTest.SecondTask.Controllers;
using ChatGptGeneratedCodeTest.SecondTask.Models;
using ChatGptGeneratedCodeTest.SecondTask.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChatGptGeneratedCodeTest.SecondTask.UnitTests.Controllers;

[TestClass]
public class BooksControllerTests
{
    private BookStoreDbContext _dbContext;
    private BooksController _booksController;

    [TestInitialize]
    public void Initialize()
    {
        var options = new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseInMemoryDatabase(databaseName: $"{System.Guid.NewGuid()}")
            .Options;

        _dbContext = new BookStoreDbContext(options);
        _booksController = new BooksController(_dbContext);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext?.Dispose();
    }

    [TestMethod]
    public async Task GetBooks_ReturnsAllBooks()
    {
        // Arrange
        await AddSampleDataAsync();

        // Act
        var result = await _booksController.GetBooks(null, null, null);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Value.Count());
    }

    [TestMethod]
    public async Task GetBooks_ReturnsFilteredBooksByTitle()
    {
        // Arrange
        await AddSampleDataAsync();

        // Act
        var result = await _booksController.GetBooks("Ender's", null, null); // Use "Ender's" instead of "Enders"

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Value.Count());
        Assert.AreEqual("Ender's Game", result.Value.First().Title);
    }

    [TestMethod]
    public async Task GetBooks_ReturnsFilteredBooksByAuthorId()
    {
        // Arrange
        await AddSampleDataAsync();
        int authorId = _dbContext.Authors.Single(a => a.Name == "Arthur C. Clarke").Id;

        // Act
        var result = await _booksController.GetBooks(null, authorId, null);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Value.Count());
        Assert.AreEqual("2001: A Space Odyssey", result.Value.First().Title);
    }

    [TestMethod]
    public async Task GetBooks_ReturnsFilteredBooksByGenreId()
    {
        // Arrange
        await AddSampleDataAsync();
        int genreId = _dbContext.Genres.Single(g => g.Name == "Science Fiction").Id;

        // Act
        var result = await _booksController.GetBooks(null, null, genreId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Value.Count());
    }

    [TestMethod]
    public async Task GetBook_ReturnsBookIfFound()
    {
        // Arrange
        await AddSampleDataAsync();
        int bookId = _dbContext.Books.Single(b => b.Title == "Ender's Game").Id;

        // Act
        var result = await _booksController.GetBook(bookId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Value, typeof(Book));
        Assert.AreEqual(bookId, result.Value.Id);
        Assert.AreEqual("Ender's Game", result.Value.Title);
    }

    [TestMethod]
    public async Task GetBook_ReturnsNotFoundIfNotFound()
    {
        // Arrange

        // Act
        var result = await _booksController.GetBook(1);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task CreateBook_AddsBookToDatabase()
    {
        // Arrange
        var author = new Author { Name = "Orson Scott Card" };
        var genre = new Genre { Name = "Science Fiction" };

        _dbContext.Authors.Add(author);
        _dbContext.Genres.Add(genre);
        await _dbContext.SaveChangesAsync();

        Book newBook = new Book
        {
            Title = "Ender's Game",
            AuthorId = author.Id,
            GenreId = genre.Id,
            Price = 9.99m,
            QuantityAvailable = 5
        };

        // Act
        var result = await _booksController.CreateBook(newBook);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        var createdBook = (result.Result as CreatedAtActionResult)?.Value as Book;
        Assert.IsNotNull(createdBook);
        Assert.AreEqual(newBook.Title, createdBook.Title);
        Assert.AreEqual(newBook.AuthorId, createdBook.AuthorId);
        Assert.AreEqual(newBook.GenreId, createdBook.GenreId);
        Assert.AreEqual(newBook.Price, createdBook.Price);
        Assert.AreEqual(newBook.QuantityAvailable, createdBook.QuantityAvailable);
    }

    [TestMethod]
    public async Task UpdateBook_UpdatesBookInDatabase()
    {
        // Arrange
        await AddSampleDataAsync();
        var originalBook = await _dbContext.Books.SingleAsync(b => b.Title == "Ender's Game");

        // Update original book properties
        originalBook.Title = "Ender's Game - Updated";
        originalBook.Price = 11.99m;
        originalBook.QuantityAvailable = 2;

        // Act
        var updateResult = await _booksController.UpdateBook(originalBook.Id, originalBook);
        var getResult = await _booksController.GetBook(originalBook.Id);

        // Assert
        Assert.IsInstanceOfType(updateResult, typeof(NoContentResult));
        Assert.IsNotNull(getResult);
        Assert.AreEqual(originalBook.Id, getResult.Value.Id);
        Assert.AreEqual(originalBook.Title, getResult.Value.Title);
        Assert.AreEqual(originalBook.AuthorId, getResult.Value.AuthorId);
        Assert.AreEqual(originalBook.GenreId, getResult.Value.GenreId);
        Assert.AreEqual(originalBook.Price, getResult.Value.Price);
        Assert.AreEqual(originalBook.QuantityAvailable, getResult.Value.QuantityAvailable);
    }

    [TestMethod]
    public async Task DeleteBook_RemovesBookFromDatabase()
    {
        // Arrange
        await AddSampleDataAsync();
        int bookId = _dbContext.Books.Single(b => b.Title == "Ender's Game").Id;

        // Act
        var deleteResult = await _booksController.DeleteBook(bookId);
        var getResult = await _booksController.GetBook(bookId);

        // Assert
        Assert.IsInstanceOfType(deleteResult, typeof(NoContentResult));
        Assert.IsInstanceOfType(getResult.Result, typeof(NotFoundResult));
    }

    private async Task AddSampleDataAsync()
    {
        var author1 = new Author { Name = "Orson Scott Card" };
        var author2 = new Author { Name = "Arthur C. Clarke" };

        var genre1 = new Genre { Name = "Science Fiction" };
        var genre2 = new Genre { Name = "Fantasy" };

        _dbContext.Authors.Add(author1);
        _dbContext.Authors.Add(author2);
        _dbContext.Genres.Add(genre1);
        _dbContext.Genres.Add(genre2);

        await _dbContext.SaveChangesAsync();

        _dbContext.Books.Add(new Book
        {
            Title = "Ender's Game",
            AuthorId = author1.Id,
            GenreId = genre1.Id,
            Price = 9.99m,
            QuantityAvailable = 5
        });

        _dbContext.Books.Add(new Book
        {
            Title = "2001: A Space Odyssey",
            AuthorId = author2.Id,
            GenreId = genre1.Id,
            Price = 11.99m,
            QuantityAvailable = 3
        });

        _dbContext.Books.Add(new Book
        {
            Title = "A Game of Thrones",
            AuthorId = author1.Id,
            GenreId = genre2.Id,
            Price = 14.99m,
            QuantityAvailable = 10
        });

        await _dbContext.SaveChangesAsync();
    }
}