using Microsoft.EntityFrameworkCore;
using Taskify.Domain.Entities;

namespace Taskify.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Project> Projects { get; set; }
    DbSet<TaskItem> Tasks { get; set; }
    DbSet<Tag> Tags { get; set; }
    DbSet<Comment> Comments { get; set; }
    DbSet<Attachment> Attachments { get; set; }
    DbSet<Event> Events { get; set; }
    DbSet<Message> Messages { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}