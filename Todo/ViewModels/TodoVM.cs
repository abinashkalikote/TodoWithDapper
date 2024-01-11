using System.ComponentModel.DataAnnotations;

namespace Todo.ViewModels;

public class TodoVM
{
    [Required]
    public string Task { get; set; }
    [Required]
    public string Description { get; set; }
}