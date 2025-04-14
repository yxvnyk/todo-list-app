using AutoMapper;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Helpers.Profiles;

public class TodosProfile : Profile
{
    public TodosProfile()
    {
        _ = this.CreateMap<TodoListModel, TodoListEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<TodoListEntity, TodoListModel>();

        _ = this.CreateMap<TaskModel, TaskEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<TaskEntity, TaskModel>();

        _ = this.CreateMap<CommentModel, CommentEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<CommentEntity, CommentModel>();

        _ = this.CreateMap<TagModel, TagEntity>()
            .ForMember(dest => dest.Id, model => model.Ignore());
        _ = this.CreateMap<TagEntity, TagModel>();
    }
}
