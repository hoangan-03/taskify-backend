using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace TodoAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TasksDto>>> GetTasks()
        {
            var tasks = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .Include(t => t.Comments)
                    .ThenInclude(tt => tt.User)
                .Select(t => new TasksDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    CreatedAt = t.CreatedAt,
                    Deadline = t.Deadline,
                    Type = t.Type,
                    State = t.State,
                    TaskTags = t.TaskTags.Select(tt => new TagsDto
                    {
                        Name = tt.Tag != null ? tt.Tag.TagName : null,
                        Color = tt.Tag != null ? tt.Tag.Color : null
                    }).ToList(),
                    Comments = t.Comments.Select(tt => new CommentsDto
                    {
                        CommentId = tt.CommentId,
                        CommentText = tt.CommentText,
                        State = tt.State,
                        Timeline = tt.Timeline,
                        TaskId = tt.TaskId,
                        UserId = tt.UserId,
                        User = tt.User,
                        UserName = tt.User != null ? tt.User.FullName : null,
                    }).ToList(),
                    Attachments = t.Attachments,
                    ProjectName = t.Project != null ? t.Project.Title : null,
                    AssignerId = t.AssignerId,
                    AssigneeId = t.AssigneeId,
                    Order = t.Order,
                })
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<Task>> AddTask(TasksDto newTaskDto)
        {
            if (newTaskDto == null)
            {
                return BadRequest("Task is null.");
            }

            var newTask = new Task
            {
                Title = newTaskDto.Title,
                Description = newTaskDto.Description,
                CreatedAt = newTaskDto.CreatedAt,
                Deadline = newTaskDto.Deadline,
                State = newTaskDto.State,
                Type = newTaskDto.Type,
                ProjectId = newTaskDto.ProjectId,
                AssignerId = newTaskDto.AssignerId,
                AssigneeId = newTaskDto.AssigneeId,
                Attachments = newTaskDto.Attachments
            };

            foreach (var tagDto in newTaskDto.TaskTags)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.TagName == tagDto.Name);
                if (tag == null)
                {
                    tag = new Tag { TagName = tagDto.Name, Color = tagDto.Color };
                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (tag.Color != tagDto.Color)
                    {
                        tag.Color = tagDto.Color;
                        _context.Tags.Update(tag);
                        await _context.SaveChangesAsync();
                    }
                }
                newTask.TaskTags.Add(new TaskTag { TagId = tag.TagId, Tag = tag });
            }

            foreach (var commentDto in newTaskDto.Comments)
            {
                var comment = new Comment
                {
                    CommentId = commentDto.CommentId,
                    CommentText = commentDto.CommentText,
                    UserId = commentDto.UserId,
                    Timeline = commentDto.Timeline,
                    State = commentDto.State
                };
                newTask.Comments.Add(comment);
            }

            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTasks), new { id = newTask.Id }, newTask);
        }
        [HttpPut("modify/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TasksDTOForModify updateTaskDto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound(new { message = "Task not found." });
            }

            task.Title = updateTaskDto.Title;
            task.Description = updateTaskDto.Description;
            task.Deadline = updateTaskDto.Deadline;
            task.Type = updateTaskDto.Type;
            task.ProjectId = updateTaskDto.ProjectId;
            task.AssignerId = updateTaskDto.AssignerId;
            task.AssigneeId = updateTaskDto.AssigneeId;
            task.Attachments = updateTaskDto.Attachments;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound(new { message = "Task not found." });
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        [HttpPost("save-task-order")]
        public async Task<ActionResult> SaveTaskOrder(TaskOrderUpdateDto taskOrderUpdateDto)
        {
            if (taskOrderUpdateDto == null || taskOrderUpdateDto.Tasks == null || !taskOrderUpdateDto.Tasks.Any())
            {
                return BadRequest(new { message = "Task order is null or empty." });
            }

            try
            {
                foreach (var taskOrderDto in taskOrderUpdateDto.Tasks)
                {
                    var task = await _context.Tasks.FindAsync(taskOrderDto.Id);
                    if (task == null)
                    {
                        return NotFound(new { message = $"Task with ID {taskOrderDto.Id} not found." });
                    }

                    task.Order = taskOrderDto.Order;
                    _context.Tasks.Update(task);
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Task order saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving the task order.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskState(int id, [FromBody] TasksDtoForUpdate updateTaskDto)
        {
            if (updateTaskDto == null || id != updateTaskDto.Id)
            {
                return BadRequest("Invalid Task data.");
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            task.State = updateTaskDto.State;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound("Task not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
    
}