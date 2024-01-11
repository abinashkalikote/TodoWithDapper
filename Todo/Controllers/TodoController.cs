using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Dto;
using Todo.Models;
using Todo.Services;
using Todo.ViewModels;

namespace Todo.Controllers;

public class TodoController : Controller
{
    private readonly TodoService _todoService;

    public TodoController(TodoService todoService)
    {
        _todoService = todoService;
    }

    // GET
    public async Task<IActionResult> Index(TodoIndexVm vm)
    {
        var items = await _todoService.GetAllTodos(vm.Task);
        var count = await _todoService.GetCount();
        vm.Items = items;
        vm.TodoCount = count;
        return View(vm);
    }

    [HttpGet]
    public IActionResult CreateTodo()
    {
        return View(new TodoVM());
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodo(TodoVM todo)
    {
        if (!ModelState.IsValid) return View(todo);
        var dto = new TodoDto()
        {
            Task = todo.Task,
            Description = todo.Description
        };
        await _todoService.Create(dto);
        TempData["success"] = "Todo Added Successfully !";
        return RedirectToAction("CreateTodo", "Todo");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(long id)
    {
        var todo = await _todoService.GetTodo(id);
        if (todo == null)
            throw new Exception("Todo not found !");

        var result = await _todoService.DeleteTodo(id);
        if (result == 1)
        {
            TempData["success"] = "Todo Successfully Deleted";
        }
        else
        {
            TempData["error"] = "Something Went wrong !";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var todo = await _todoService.GetTodo(id);
            var todoVm = new TodoVM()
            {
                Task = todo.Task,
                Description = todo.Description
            };
            return View(todoVm);
        }
        catch (Exception e)
        {
            throw new Exception("Something went wrong !");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long id, TodoVM todoVm)
    {
        if (!ModelState.IsValid) return View(todoVm);
        var todoDto = new TodoDto()
        {
            Task = todoVm.Task,
            Description = todoVm.Description
        };

        var todo = await _todoService.GetTodo(id);
        await _todoService.Update(todo, todoDto);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> MarkAsCompleted(long id)
    {
        var todo = _todoService.GetTodo(id);
        if (todo == null) throw new Exception("Something Went Wrong !");
        await _todoService.MarkAsCompleted(id);
        return RedirectToAction(nameof(Index));
    }
}