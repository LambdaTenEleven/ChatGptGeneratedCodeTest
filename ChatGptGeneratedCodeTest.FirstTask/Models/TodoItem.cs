using System.ComponentModel.DataAnnotations;

namespace ChatGptGeneratedCodeTest.FirstTask.Models;

public class TodoItem
{
    public long Id { get; set; }

    [Required] public string Title { get; set; }
    public string Description { get; set; }
}