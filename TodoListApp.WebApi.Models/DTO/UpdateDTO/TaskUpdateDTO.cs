using System;
using System.ComponentModel.DataAnnotations;
using TodoListApp.WebApi.Models.CustomValidations;
using TodoListApp.WebApi.Models.Enums;

namespace TodoListApp.WebApi.Models.DTO.UpdateDTO;

public class TaskUpdateDTO
{
    [MaxLength(50)]
    public string? Title { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }

    public DateTime? DateCreated { get; set; }

    [MinDate]
    public DateTime? DueDate { get; set; }

    [EnumDataType(typeof(Status), ErrorMessage = "Invalid status value.")]
    public Status? Status { get; set; }

    [MaxLength(450)]
    public string? AssigneeId { get; set; }
}
