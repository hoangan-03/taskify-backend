
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAppBackend;

namespace TodoAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var tasks = await _context.Events
                .Include(e => e.EventUsers)
                    .ThenInclude(eu => eu.User)
                .Select(e => new
                {
                    e.EventId,
                    e.EventName,
                    e.Description,
                    e.Color,
                    e.Date,
                    e.StartTime,
                    e.EndTime,
                    e.Location,
                    e.TaskId,
                    e.CreatorId,
                    EventUsers = e.EventUsers.Select(eu => new
                    {
                        eu.UserId,
                        FullName = eu.User != null ? eu.User.FullName : string.Empty
                    }).ToList()
                })
                .ToListAsync();
            return Ok(tasks);
        }


        [HttpPost]
        public async Task<ActionResult<Event>> AddEvent(EventsDtoForAdding newEventDTO)
        {
            if (newEventDTO == null)
            {
                return BadRequest("Event is null.");
            }

            var newEvent = new Event
            {
                EventName = newEventDTO.EventName,
                Description = newEventDTO.Description,
                Color = newEventDTO.Color,
                Date = newEventDTO.Date,
                StartTime = newEventDTO.StartTime,
                EndTime = newEventDTO.EndTime,
                Location = newEventDTO.Location,
                TaskId = newEventDTO.TaskId,
                CreatorId = newEventDTO.CreatorId,
            };
            foreach (var eventUser in newEventDTO.EventUsers)
            {
                newEvent.EventUsers.Add(new EventUser { EventId = newEvent.EventId, UserId = eventUser.UserId });
            }
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvents), new { id = newEvent.EventId }, newEvent);
        }
    }
}