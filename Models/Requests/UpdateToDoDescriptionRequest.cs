namespace TodoApp.Api.Models.Requests;

public class UpdateToDoDescriptionRequest
{
    public string Title { get; set; }
    public string NewDescription { get; set; }
}