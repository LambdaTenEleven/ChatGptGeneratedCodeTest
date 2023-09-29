using ChatGptGeneratedCodeTest.SecondTask.Models;
using ChatGptGeneratedCodeTest.SecondTask.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatGptGeneratedCodeTest.SecondTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookStoreDbContext _context;

    public BooksController(BookStoreDbContext context)
    {
        _context = context;
    }

    // GET: api/Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks(string title, int? authorId, int? genreId)
    {
        IQueryable<Book> booksQuery = _context.Books.Include(b => b.Author).Include(b => b.Genre);

        if (!string.IsNullOrEmpty(title))
        {
            booksQuery = booksQuery.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        if (authorId.HasValue)
        {
            booksQuery = booksQuery.Where(b => b.AuthorId == authorId);
        }

        if (genreId.HasValue)
        {
            booksQuery = booksQuery.Where(b => b.GenreId == genreId);
        }

        return await booksQuery.ToListAsync();
    }

    // GET: api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return book;
    }

    // PUT: api/Books/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }

        _context.Entry(book).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Books.Any(b => b.Id == id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/Books
    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    // DELETE: api/Books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}