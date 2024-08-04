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

	public class User
	{
		[Key]
		public string UserId { get; set; } = Guid.NewGuid().ToString();
		public string? FullName { get; set; }

		[Required]
		public string? Email { get; set; }

		[Required]
		public string? Password { get; set; }
		public DateTime CreateAt { get; set; } = DateTime.UtcNow;

		// Navigation property
		public ICollection<Task> Tasks { get; set; } = new List<Task>();
		public ICollection<Comment> Comments { get; set; } = new List<Comment>();
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
		public int CommentId { get; set; }

		[Required]
		public CommentState State { get; set; }

		[Required]
		public string? CommentText { get; set; }

		[Required]
		public DateTime Timeline { get; set; }

		// Foreign key for User
		[ForeignKey("User")]
		public string? UserId { get; set; }
		public User? User { get; set; }

		// Foreign key for Task
		[ForeignKey("Task")]
		public int? TaskId { get; set; }
		public Task? Task { get; set; }
	}

	public class Attachment
	{
		[Key]
		public int AttachmentId { get; set; }

		[Required]
		public string? Name { get; set; }
		public AttachmentType Type { get; set; }

		// Foreign key for Task
		[ForeignKey("Task")]
		public int? TaskId { get; set; }
		public Task? Task { get; set; }
	}

	public class Project
	{
		[Key]
		public string ProjectId { get; set; } = Guid.NewGuid().ToString();

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

        [Required]
        public string[] Type { get; set; } = Array.Empty<string>();

        // Foreign key for Project
        [ForeignKey("Project")]
        public string? ProjectId { get; set; }
        public Project? Project { get; set; }

        // Foreign key for User
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public User? User { get; set; }

        // Navigation properties
        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }

    public class TaskTag
	{
		public int TaskId { get; set; }
		public Task? Task { get; set; }

		public int TagId { get; set; }
		public Tag? Tag { get; set; }
	}

}
