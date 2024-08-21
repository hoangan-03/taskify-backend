using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAppBackend
{
	public enum AttachmentType
	{
		PDF,
		DOCX,
		XLSX,
		PPTX,
		PNG,
		JPG,
		GIF,
		DOC,
		XLS,
		PPT,
		XML,
		MD,
	}

	public enum CommentState
	{
		checkedd,
		uncheckedd,
	}
    public enum TaskState
    {
        inprogress,
        completed,
		prioritized,
    }

    public class User
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? UserId { get; set; } 
		public string? FullName { get; set; }

		[Required]
		public string? Email { get; set; }

		[Required]
		public string? Password { get; set; }
		public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
        public ICollection<Task> CreatedTasks { get; set; } = new List<Task>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();
    }

	public class Tag
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }

		[Required]
		public string? TagName { get; set; }

		[Required]
		public string? Color { get; set; }

		// Navigation property
		public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
	}

	public class Comment
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

		[Required]
		public CommentState State { get; set; }

		[Required]
		public string? CommentText { get; set; }

		[Required]
		public DateTime Timeline { get; set; }

		// Foreign key for User
		[ForeignKey("User")]
		public int? UserId { get; set; }
		public User? User { get; set; }

		// Foreign key for Task
		[ForeignKey("Task")]
		public int? TaskId { get; set; }
		public Task? Task { get; set; }
	}

	public class Attachment
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentId { get; set; }

		[Required]
		public string? Url { get; set; }
		public string? Name { get; set; }
		public AttachmentType FileType { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Foreign key for Task
        [ForeignKey("Task")]
		public int? TaskId { get; set; }
		public Task? Task { get; set; }
	}

	public class Project
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ProjectId { get; set; }

		[Required]
		public string? Title { get; set; }
		public string? Description { get; set; }
		public DateTime CreateAt { get; set; } = DateTime.UtcNow;

		// Navigation property
		public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }

    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Deadline { get; set; }
		public TaskState State { get; set; }

        [Required]
        public string[] Type { get; set; } = Array.Empty<string>();

        // Foreign key for Project
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }

        // Foreign key for User
        [ForeignKey("User")]
        public int? AssignerId { get; set; }
        public User? Assigner { get; set; }
        public int? AssigneeId { get; set; }
        public User? Assignee { get; set; }

        // Navigation properties
        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public int Order { get; set; }
    }

    public class TaskTag
	{
		public int TaskId { get; set; }
		public Task? Task { get; set; }

		public int TagId { get; set; }
		public Tag? Tag { get; set; }
	}

    public enum Color
    {
        blue,
        green,
        red,
        purple,
        yellow,
        gray,
        pink,
    }
    public class Event
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }
		public string? EventName { get; set; }
		public string? Description { get; set; }
        public Color? Color { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Location { get; set; }

        [ForeignKey("Task")]
        public int? TaskId { get; set; }
        public Task? Task { get; set; }

        [ForeignKey("User")]
        public int? CreatorId { get; set; }
        public User? Creator { get; set; }
        public ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();

    }

    public class EventUser
    {
        public int EventId { get; set; }
        public Event? Event { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }


}
