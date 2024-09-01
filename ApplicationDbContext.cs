using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Channels;
using System.Text.Json;
namespace TodoAppBackend
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configure the one-to-many relationship between User and Message
            modelBuilder.Entity<Message>()
                .HasOne(t => t.Sender)
                .WithMany(p => p.MessageSent)
                .HasForeignKey(t => t.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the one-to-many relationship between User and Message
            modelBuilder.Entity<Message>()
                .HasOne(t => t.Receiver)
                .WithMany(p => p.MessageReceived)
                .HasForeignKey(t => t.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the one-to-many relationship between Project and Task
            modelBuilder.Entity<Task>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure many-to-many relationship between Task and Tag
            modelBuilder.Entity<TaskTag>()
                .HasKey(tt => new { tt.TaskId, tt.TagId });

            modelBuilder.Entity<TaskTag>()
                .HasOne(tt => tt.Task)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TaskId);

            modelBuilder.Entity<TaskTag>()
                .HasOne(tt => tt.Tag)
                .WithMany(tg => tg.TaskTags)
                .HasForeignKey(tt => tt.TagId);

            // Configure many-to-many relationship between Event and User
            modelBuilder.Entity<EventUser>()
                .HasKey(tt => new { tt.EventId, tt.UserId });

            modelBuilder.Entity<EventUser>()
                .HasOne(tt => tt.Event)
                .WithMany(t => t.EventUsers)
                .HasForeignKey(tt => tt.EventId);

            modelBuilder.Entity<EventUser>()
                .HasOne(tt => tt.User)
                .WithMany(tg => tg.EventUsers)
                .HasForeignKey(tt => tt.UserId);


            // Configure one-to-many relationship between Task and Event
            modelBuilder.Entity<Event>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Events)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Task and Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Task and Attachment
            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.Task)
                .WithMany(t => t.Attachments)
                .HasForeignKey(a => a.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            var stringArrayConverter = new ValueConverter<string[], string>(
           v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
           v => JsonSerializer.Deserialize<string[]>(v, (JsonSerializerOptions)null));

            modelBuilder.Entity<Task>()
                .Property(e => e.Type)
                .HasConversion(stringArrayConverter);

            modelBuilder.Entity<Task>()
            .HasOne(t => t.Assigner)
            .WithMany(u => u.CreatedTasks)
            .HasForeignKey(t => t.AssignerId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
