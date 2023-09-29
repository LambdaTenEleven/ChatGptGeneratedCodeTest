using System.ComponentModel.DataAnnotations;

namespace ChatGptGeneratedCodeTest.SecondTask.Models;

public class Author
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<Book> Books { get; set; }
}