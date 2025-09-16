using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using Taskify.Application.Interfaces;
using Taskify.Domain.Entities;
using Taskify.Domain.Enums;

namespace Taskify.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Attachment> Attachments { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<TaskItem> Tasks { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Password).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configure Project entity
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
        });

        // Configure TaskItem entity
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.State).HasConversion<string>();

            // Configure string array
            entity.Property(e => e.Type)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<string[]>(v, (JsonSerializerOptions?)null) ?? Array.Empty<string>());

            // Relationships
            entity.HasOne(e => e.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Assigner)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(e => e.AssignerId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Assignee)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(e => e.AssigneeId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Tag entity
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TagName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Color).IsRequired().HasMaxLength(20);
        });

        // Configure TaskTag many-to-many relationship
        modelBuilder.Entity<TaskTag>(entity =>
        {
            entity.HasKey(tt => new { tt.TaskId, tt.TagId });

            entity.HasOne(tt => tt.Task)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(tt => tt.Tag)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Comment entity
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CommentText).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.State).HasConversion<string>();

            entity.HasOne(e => e.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Attachment entity
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Url).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.FileType).HasConversion<string>();

            entity.HasOne(e => e.Task)
                .WithMany(t => t.Attachments)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Message entity
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MessageText).IsRequired().HasMaxLength(1000);

            entity.HasOne(e => e.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(e => e.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(e => e.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Event entity
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EventName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Color).HasConversion<string>();
            entity.Property(e => e.Location).HasMaxLength(200);

            entity.HasOne(e => e.Task)
                .WithMany(t => t.Events)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Creator)
                .WithMany(u => u.CreatedEvents)
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure EventUser many-to-many relationship
        modelBuilder.Entity<EventUser>(entity =>
        {
            entity.HasKey(eu => new { eu.EventId, eu.UserId });

            entity.HasOne(eu => eu.Event)
                .WithMany(e => e.EventUsers)
                .HasForeignKey(eu => eu.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(eu => eu.User)
                .WithMany(u => u.EventUsers)
                .HasForeignKey(eu => eu.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}