using System.Text;
using TodoApp.Api.DB;
using TodoApp.Api.Models.Entities;

namespace TodoApp.Api.Repositories;

public interface IToDoCreateRepository
{
    void Insert(TodoEntity entity);
    Task SaveChangesAsync();
    List<TodoEntity> Read();
}
public class ToDoCreateRepository : IToDoCreateRepository
{
    private readonly AppDbContext _db;

    public ToDoCreateRepository(AppDbContext db)
    {
        _db = db;
    }
    public void Insert(TodoEntity entity)
    {
        _db.Todos.Add(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public List<TodoEntity> Read()
    {
        var data = _db.Todos.OrderBy(x => x.DeadLine).ToList();
        return data;

    }
    
    
  
}

