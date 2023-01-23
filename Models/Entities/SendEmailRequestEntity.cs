namespace TodoApp.Api.Models.Entities;

public enum SendEmailRequestStatus
{
    New, 
    Sent, 
    Failed
}
public class SendEmailRequestEntity
{
    public int  Id { get; set; }
    public string ToAddress { get; set; }
    public string Body { get; set; }
    public  DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public SendEmailRequestStatus Status { get; set; }
}