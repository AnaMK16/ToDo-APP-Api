using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Models.Entities;
using TodoApp.Api.Models.Requests;
using TodoApp.Api.Repositories;

namespace TodoApp.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ToDoController : ControllerBase
{
   
    private readonly IToDoCreateRepository _toDoCreateRepository;

    public ToDoController(IToDoCreateRepository toDoCreateRepository)
    {
        _toDoCreateRepository = toDoCreateRepository;
    }
    
    //create
    [HttpPost("ToDoCreate")]
    public async Task<IActionResult> ToDoCreate(ToDoCreateRequest request)
    {
        var entity = new TodoEntity();
        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.DeadLine = request.DeadLine;
        
        _toDoCreateRepository.Insert(entity);
         await _toDoCreateRepository.SaveChangesAsync();
         return Ok();

    }

    [HttpGet("ReadAllToDos")]
    public List<TodoEntity> GetAllToDos()
    {
        return _toDoCreateRepository.Read();
    }
    
}