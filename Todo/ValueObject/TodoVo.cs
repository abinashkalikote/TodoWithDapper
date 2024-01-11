using System.ComponentModel.DataAnnotations;
using Todo.Constants;

namespace Todo.ValueObject;

public class TodoVo
{
    public int Id { get; set; }
    public string Task { get; set; }
    public string  Description { get; set; }
    public bool IsDone { get; set; } = TodoStatus.Pending;
}