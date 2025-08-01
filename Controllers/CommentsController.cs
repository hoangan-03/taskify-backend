
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace TodoAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            var comments = await _context.Comments
                .Include(t => t.User)
                .Select(t => new CommentsDto
                {
                    CommentId = t.CommentId,
                    CommentText = t.CommentText,
                    State = t.State,
                    Timeline = t.Timeline,
                    TaskId = t.TaskId,
                    UserId = t.UserId,
                    User = t.User,
                    UserName = t.User != null ? t.User.FullName : null,
                })
                .ToListAsync();
            return Ok(comments);
        }

        [HttpGet("between/{userId1}/{userId2}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBetweenUsers(int userId1, int userId2)
        {
            var messages = await _context.Messages
                .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) || (m.SenderId == userId2 && m.ReceiverId == userId1))
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .ToListAsync();

            return Ok(messages);
        }




        [HttpPost]
        public async Task<ActionResult<Comment>> AddComment(CommentsDtoForSubbmit newCommentDto)
        {
            if (newCommentDto == null)
            {
                return BadRequest("Comment is null.");
            }

            var task = await _context.Tasks.FindAsync(newCommentDto.TaskId);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            var newComment = new Comment
            {
                CommentId = newCommentDto.CommentId,
                CommentText = newCommentDto.CommentText,
                State = newCommentDto.State,
                Timeline = newCommentDto.Timeline,
                TaskId = newCommentDto.TaskId,
                UserId = newCommentDto.UserId,
            };

            task.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComments), new { id = newComment.CommentId }, newComment);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommentState(int id, [FromBody] CommentsDtoForUpdate updateCommentDto)
        {
            if (updateCommentDto == null || id != updateCommentDto.CommentId)
            {
                return BadRequest("Invalid comment data.");
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound("Comment not found.");
            }

            comment.State = updateCommentDto.State;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound("Comment not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}
