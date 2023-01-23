namespace TodoApp.Api.Models.Entities;

public class TodoEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DeadLine { get; set; }
    
}