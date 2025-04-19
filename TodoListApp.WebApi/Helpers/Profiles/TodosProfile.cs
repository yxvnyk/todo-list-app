using AutoMapper;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Helpers.Profiles;

public class TodosProfile : Profile
{
    public TodosProfile()
    {
        _ = this.CreateMap<TodoListDTO, TodoListEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<TodoListEntity, TodoListDTO>();

        _ = this.CreateMap<TaskDTO, TaskEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<TaskEntity, TaskDTO>();

        _ = this.CreateMap<CommentDTO, CommentEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<CommentEntity, CommentDTO>();

        _ = this.CreateMap<TagDTO, TagEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<TagEntity, TagDTO>();
    }
}
