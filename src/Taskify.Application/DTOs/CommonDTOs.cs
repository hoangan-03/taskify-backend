using Taskify.Domain.Enums;

namespace Taskify.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateUserDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Deadline { get; set; }
    public TaskState State { get; set; }
    public string[] Type { get; set; } = Array.Empty<string>();
    public int Order { get; set; }
    public int? ProjectId { get; set; }
    public string? ProjectTitle { get; set; }
    public List<TagDto> Tags { get; set; } = new();
}

public class TagDto
{
    public int Id { get; set; }
    public string TagName { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}

public class ProjectDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<TaskDto> Tasks { get; set; } = new();
}