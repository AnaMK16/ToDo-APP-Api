using TodoApp.Api.Models.Entities;

namespace TodoApp.Api.Models.Requests;

public class ChangeToDoStatusRequest
{
    public string Title { get; set; }
    public ToDoStatus Status { get; set; }
}