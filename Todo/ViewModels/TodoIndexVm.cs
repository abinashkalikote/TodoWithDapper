using Todo.ValueObject;

namespace Todo.ViewModels;

public class TodoIndexVm
{
    public List<TodoVo> Items = new List<TodoVo>();

    public int TodoCount;
    public string Task { get; set; }
}