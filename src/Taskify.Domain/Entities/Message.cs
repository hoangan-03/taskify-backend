namespace Taskify.Domain.Entities;

public class Message : BaseEntity
{
    public string MessageText { get; set; } = string.Empty;
    public DateTime Timeline { get; set; }

    // Foreign keys
    public int ReceiverId { get; set; }
    public int SenderId { get; set; }

    // Navigation properties
    public User? Receiver { get; set; }
    public User? Sender { get; set; }
}