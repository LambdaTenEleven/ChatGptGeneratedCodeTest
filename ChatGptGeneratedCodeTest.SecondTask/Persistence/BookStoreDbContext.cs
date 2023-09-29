using ChatGptGeneratedCodeTest.SecondTask.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatGptGeneratedCodeTest.SecondTask.Persistence;

public class BookStoreDbContext : DbContext
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
}