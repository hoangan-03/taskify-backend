
namespace TodoAppBackend.Controllers
{
    public class MessageSentDTO
    {
        public string? MessageText { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
