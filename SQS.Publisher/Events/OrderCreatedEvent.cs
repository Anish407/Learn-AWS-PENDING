namespace SQS.Publisher.Events;

public class OrderCreatedEvent
{
    public int Id { get; set; }
    public string Details { get; set; }
    public DateTime Date { get; set; }
    public string UserName { get; set; }
}