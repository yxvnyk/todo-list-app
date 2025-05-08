using AutoMapper;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Helpers.Profiles;

/// <summary>
/// Defines AutoMapper profiles for mapping update DTOs to entity models,
/// applying conditional mapping to avoid overwriting values with nulls.
/// </summary>
public class TodosUpdateProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodosUpdateProfile"/> class.
    /// Configures conditional mappings from update DTOs to their corresponding entities.
    /// Only non-null properties from the source are mapped.
    /// </summary>
    public TodosUpdateProfile()
    {
        // Map TodoListUpdateDTO to TodoListEntity, skipping null values
        this.CreateMap<TodoListUpdateDTO, TodoListEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Map TaskUpdateDTO to TaskEntity, skipping null values
        this.CreateMap<TaskUpdateDTO, TaskEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Map CommentUpdateDTO to CommentEntity, skipping null values
        this.CreateMap<CommentUpdateDTO, CommentEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Map TagUpdateDTO to TagEntity, skipping null values
        this.CreateMap<TagUpdateDTO, TagEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
