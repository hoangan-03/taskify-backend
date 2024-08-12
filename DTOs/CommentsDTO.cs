namespace TodoAppBackend.Controllers
{
    public class CommentsDto
    {
        public int CommentId { get; set; }
        public CommentState State { get; set; }
        public string? CommentText { get; set; }
        public DateTime Timeline { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public string? UserName { get; set; }
        public int? TaskId { get; set; }
    }
    public class CommentsDtoForSubbmit
    {
        public int CommentId { get; set; }
        public CommentState State { get; set; }
        public string? CommentText { get; set; }
        public DateTime Timeline { get; set; }
        public int? UserId { get; set;}
        public int? TaskId { get; set; }
    }
    public class CommentsDtoForUpdate
    {
        public int CommentId { get; set; }
        public CommentState State { get; set; }
    }
}
