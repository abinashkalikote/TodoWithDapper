using System.ComponentModel.DataAnnotations;
using Dapper;
using Todo.Constants;

namespace Todo.Models;

public class Todos
{
    
    public int Id { get; set; }
    public string Task { get; set; }
    public string  Description { get; set; }
    public bool IsDone { get; set; } = TodoStatus.Pending;

    public Todos(){}

    public void Update( string task, string description, bool isdone)
    {
        Task = task;
        Description = description;
        IsDone = isdone;
    }
}