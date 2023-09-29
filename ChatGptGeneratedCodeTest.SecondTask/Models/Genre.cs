using System.ComponentModel.DataAnnotations;

namespace ChatGptGeneratedCodeTest.SecondTask.Models;

public class Genre
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<Book> Books { get; set; }
}