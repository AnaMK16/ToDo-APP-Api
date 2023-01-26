using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.DB;
using TodoApp.Api.Models.Entities;
using TodoApp.Api.Models.Requests;

namespace TodoApp.Api.Repositories;

public interface IToDoRepository
{
    Task InsertAsync(TodoEntity entity);
    Task SaveChangesAsync();
    List<TodoEntity> Read();
    List<TodoEntity> Search(SearchRequest request);
    Task UpdateToDoAsync(UpdateToDoRequest request);
    Task ChangeStatus(ChangeToDoStatusRequest request);
}
public class ToDoRepository : IToDoRepository
{
    private readonly AppDbContext _db;

    public ToDoRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task InsertAsync(TodoEntity entity)
    {
       await  _db.Todos.AddAsync(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public List<TodoEntity> Read()
    {
        var data =  _db.Todos.OrderBy(x => x.DeadLine).ToList();
        return data;

    }
    public List<TodoEntity> Search(SearchRequest request)
    {
        var entities = _db.Todos
            .Where(t => t.Title.Contains(request.Filter) || t.Description.Contains(request.Filter))
            .Skip(request.PageIndex * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return entities;
    }

    public async Task UpdateToDoAsync(UpdateToDoRequest request)
    {
        var toDo =  _db.Todos.Where(t => t.Title == request.CurrentTitle).First();
        if (!string.IsNullOrEmpty(request.NewTitle))
        {
            toDo.Title = request.NewTitle;
        }

        if (!string.IsNullOrEmpty(request.NewDescription))
        {
            toDo.Description = request.NewDescription;
        }

        if (request.NewDeadLine.HasValue)
        {
            toDo.DeadLine = request.NewDeadLine;
        }
        
        
    }

    public async Task ChangeStatus(ChangeToDoStatusRequest request)
    {
        var toDo =  _db.Todos.Where(t => t.Title == request.Title).First();

        toDo.Status = request.Status;
        
    }
    
    
    
  
}

