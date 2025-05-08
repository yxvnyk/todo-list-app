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
        _ = this.CreateMap<TodoListDTO, TodoListEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<TodoListEntity, TodoListDTO>();

        // Mapping between TaskDTO and TaskEntity
        _ = this.CreateMap<TaskDTO, TaskEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<TaskEntity, TaskDTO>();

        // Mapping between CommentDTO and CommentEntity
        _ = this.CreateMap<CommentDTO, CommentEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<CommentEntity, CommentDTO>();

        // Mapping between TagDTO and TagEntity
        _ = this.CreateMap<TagDTO, TagEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<TagEntity, TagDTO>();
    }
}
