using Dapper;
using Microsoft.AspNetCore.Mvc;
using Todo.Dto;
using Todo.Models;
using Todo.Provider;
using Todo.ValueObject;

namespace Todo.Services;

public class TodoService
{
    private readonly SqlConnectionProvider _sqlConnectionProvider;

    public TodoService(SqlConnectionProvider sqlConnectionProvider)
    {
        _sqlConnectionProvider = sqlConnectionProvider;
    }
    
    public async Task<List<TodoVo>> GetAllTodos(string task = "")
    {
        await using var connection = _sqlConnectionProvider.GetSqlConnection();
        var query = @"SELECT * FROM Todo WHERE Task LIKE @task";
        return (await connection.QueryAsync<TodoVo>(query, new{ task = "%" + task + "%" })).ToList();
    }

    public async Task<Todos?> GetTodo(long id)
    {
        await using var connection = _sqlConnectionProvider.GetSqlConnection();
        var query = @"SELECT * FROM Todo WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<Todos>(query, new
        {
            Id = id
        });
    }

    public async Task<int> GetCount()
    {
        await using var conn = _sqlConnectionProvider.GetSqlConnection();
        const string query = @"SELECT COUNT(*) FROM Todo";
        return await conn.ExecuteScalarAsync<int>(query);
        
    }

    public async Task<int> Create(TodoDto todoDto)
    {
        await using var connection = _sqlConnectionProvider.GetSqlConnection();
        var query = @"INSERT INTO Todo (Task, Description, IsDone) VALUES (@Task, @Description, @IsDone)";
        return await connection.ExecuteAsync(query, new
        {
            Task = todoDto.Task,
            Description = todoDto.Description,
            IsDone = todoDto.IsDone
        });
    }

    public async Task<int> DeleteTodo(long id)
    {
        await using var conn = _sqlConnectionProvider.GetSqlConnection();
        const string query = @"DELETE FROM Todo WHERE Id = @id";
        return await conn.ExecuteAsync(query, new
        {
            id = id
        });
    }

    public async Task Update(Todos? todos, TodoDto todoDto)
    {
        await using var conn = _sqlConnectionProvider.GetSqlConnection();
        todos.Update(todoDto.Task, todoDto.Description, todoDto.IsDone);
        const string query = @"UPDATE Todo SET Task=@task, Description=@description, IsDone=@isdone WHERE Id = @id";
        await conn.ExecuteAsync(query, new
        {
            id = todos.Id,
            task = todos.Task,
            description = todos.Description,
            isdone = todos.IsDone
        });
        
    }

    public async Task MarkAsCompleted(long id)
    {
        await using var conn = _sqlConnectionProvider.GetSqlConnection();
        try
        {
            const string query = @"UPDATE Todo SET IsDone = @done WHERE Id = @id";
            await conn.ExecuteAsync(query, new { done = true, id = id });
        }
        catch (Exception e)
        {
            throw new Exception("Something went wrong while updating !" + e.Message);
        }
    }
}