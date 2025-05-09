using AutoMapper;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Helpers.Profiles;

/// <summary>
/// Defines mapping configurations between DTOs and Entity models
/// for to-do lists, tasks, comments, and tags.
/// </summary>
public class TodosProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodosProfile"/> class,
    /// setting up AutoMapper mappings between DTOs and entity classes.
    /// </summary>
    public TodosProfile()
    {
        // Mapping between TodoListDTO and TodoListEntity
        _ = this.CreateMap<TodoListDto, TodoListEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<TodoListEntity, TodoListDto>();

        // Mapping between TaskDTO and TaskEntity
        _ = this.CreateMap<TaskDto, TaskEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<TaskEntity, TaskDto>();

        // Mapping between CommentDTO and CommentEntity
        _ = this.CreateMap<CommentDto, CommentEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<CommentEntity, CommentDto>();

        // Mapping between TagDTO and TagEntity
        _ = this.CreateMap<TagDto, TagEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<TagEntity, TagDto>();
    }
}
