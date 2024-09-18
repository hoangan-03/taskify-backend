using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAppBackend;

namespace TodoAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(ProjectDTO newProjectDto)
        {
            if (newProjectDto == null)
            {
                return BadRequest("Project is null.");
            }

            var newProject = new Project
            {
                Title = newProjectDto.Title,
                Description = newProjectDto.Description,
                CreateAt = newProjectDto.CreateAt,
                Tasks = new List<Task>()
            };

            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProjects), new { id = newProject.ProjectId }, newProject);
        }
    }
}