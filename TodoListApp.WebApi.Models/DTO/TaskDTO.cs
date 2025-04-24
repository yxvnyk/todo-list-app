using System;
using System.ComponentModel.DataAnnotations;
using TodoListApp.WebApi.Models.CustomValidations;
using TodoListApp.WebApi.Models.Enums;

namespace TodoListApp.WebApi.Models;

public class TaskDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Title field is required.")]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Description { get; set; }

    public DateTime DateCreated { get; set; }

    [Required(ErrorMessage = "Due date is required.")]
    [MinDate]
    public DateTime? DueDate { get; set; }

    [EnumDataType(typeof(Status), ErrorMessage = "Invalid status value.")]
    public Status Status { get; set; }

    [Required(ErrorMessage = "The AssigneeId field is required.")]
    [MaxLength(450)]
    public string AssigneeId { get; set; } = string.Empty;

    [Required]
    public int TodoListId { get; set; }
}
