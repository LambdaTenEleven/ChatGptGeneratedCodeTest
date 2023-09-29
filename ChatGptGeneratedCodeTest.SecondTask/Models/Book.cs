using System.ComponentModel.DataAnnotations;

namespace ChatGptGeneratedCodeTest.SecondTask.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; }

    public int GenreId { get; set; }
    public Genre Genre { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int QuantityAvailable { get; set; }
}