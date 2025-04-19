using AutoMapper;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Helpers.Profiles;

public class TodosUpdateProfile : Profile
{
    public TodosUpdateProfile()
    {
        this.CreateMap<TodoListUpdateDTO, TodoListEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        this.CreateMap<TaskUpdateDTO, TaskEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        this.CreateMap<CommentUpdateDTO, CommentEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        this.CreateMap<TagUpdateDTO, TagEntity>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
