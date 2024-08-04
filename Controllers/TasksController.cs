using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppBackend;

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
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
        {
            var tasks = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    CreatedAt = t.CreatedAt,
                    Deadline = t.Deadline,
                    Type = t.Type,
                    TaskTags = t.TaskTags.Select(tt => new TagDto
                    {
                        Name = tt.Tag.TagName,
                        Color = tt.Tag.Color
                    }).ToList(),
                    Comments = t.Comments,
                    Attachments = t.Attachments,
                    ProjectName = t.Project.Title
                })
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<Task>> AddTask(Task newTask)
        {
            if (newTask == null)
            {
                return BadRequest("Task is null.");
            }

            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTasks), new { id = newTask.Id }, newTask);
        }
    }

    public class TaskDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Deadline { get; set; }
        public string[] Type { get; set; }
        public string? ProjectName { get; set; }
        public List<TagDto> TaskTags { get; set; } = new List<TagDto>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
    public class TagDto
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}