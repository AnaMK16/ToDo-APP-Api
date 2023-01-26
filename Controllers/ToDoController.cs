using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Models.Entities;
using TodoApp.Api.Models.Requests;
using TodoApp.Api.Repositories;

namespace TodoApp.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ToDoController : ControllerBase
{
   
    private readonly IToDoRepository _toDoRepository;
    private readonly UserManager<UserEntity> _userManager;

    public ToDoController(IToDoRepository toDoCreateRepository, UserManager<UserEntity> userManager)
    {
        _toDoRepository = toDoCreateRepository;
        _userManager = userManager;
    }
    
    //create
    [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
    [HttpPost("ToDoCreate")]
    public async Task<IActionResult> ToDoCreate(ToDoCreateRequest request)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound("User not found");
        }

        var userId = user.Id;
        var entity = new TodoEntity();
        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.DeadLine = request.DeadLine;
        entity.CreatedAt = DateTime.UtcNow;
        entity.Status = ToDoStatus.New;
        entity.UserId = userId;
        
        _toDoRepository.InsertAsync(entity);
         await _toDoRepository.SaveChangesAsync();
         return Ok();

    }
    [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
    [HttpGet("ReadAllToDos")]
    public  List<TodoEntity> GetAllToDos()
    {
        var user = _userManager.GetUserAsync(User);

        if (user == null)
        {
            return null;
        }

        return  _toDoRepository.Read();
    }

    [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
    [HttpPost("Search-By-Title-Or-Description")]
    public List<TodoEntity> Search(SearchRequest request)
    {
        var user =  _userManager.GetUserAsync(User);

        if (user == null)
        {
            return null;
        }

        return _toDoRepository.Search(request);
    
    }

    [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
    [HttpPost("UpdateToDo")]
    public async Task<IActionResult> UpdateToDo(UpdateToDoRequest request)
    {
        var user = await  _userManager.GetUserAsync(User);

        if (user == null)
        {
            return null;
        }

        await _toDoRepository.UpdateTitleAsync(request);
        await _toDoRepository.SaveChangesAsync();
        return Ok();
        

    }

    [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
    [HttpPost("UpdateToDoStatus")]
    public async Task<IActionResult> ChangeToDoStatus(ChangeToDoStatusRequest request)
    {
        var user =  _userManager.GetUserAsync(User);

        if (user == null)
        {
            return null;
        }

        await _toDoRepository.ChangeStatus(request);
        await _toDoRepository.SaveChangesAsync();
        return Ok();
    }
    
    
    
    
    
    
    
}