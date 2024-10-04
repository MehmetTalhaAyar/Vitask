using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Models;
using Vitask.Models;

using Task = Entities.Concrete.Task;

namespace Business.AutoMapper
{
	public class TaskMappingProfile : Profile
	{
		public TaskMappingProfile()
		{
			CreateMap<Task, AllMyTaskViewModel>()
				.ForMember(dest => dest.DueTime, opt => opt.MapFrom(src => src.DueDate.ToUniversalTime()))
				.ForMember(dest=> dest.ProjectName,opt => opt.MapFrom(src => src.Project.Name))
				.ForMember(dest => dest.Reporter,opt=> opt.MapFrom(src=> src.Reporter.UserName))
				.ForMember(dest => dest.Responsible, opt => opt.MapFrom(src => src.Responsible.UserName))
				.ForMember(dest => dest.Tag, opt=> opt.MapFrom(src=> src.Tag.Name));


			CreateMap<Task, TaskViewModel>()
				.ForMember(dest => dest.DueTime, opt => opt.MapFrom(src => src.DueDate.ToUniversalTime()))
				.ForMember(dest => dest.ReporterId, opt => opt.MapFrom(src => src.Reporter))
				.ForMember(dest => dest.ResponsibleId, opt => opt.MapFrom(src => src.Responsible))
				.ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.Tag))
				.ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Project))
				.ForSourceMember(dest => dest.Comments, opt => opt.DoNotValidate());


			CreateMap<Task, UpdateTaskViewModel>()
				.ForMember(dest => dest.DueTime , opt => opt.MapFrom(src=> src.DueDate.ToUniversalTime()))
				.ForMember(dest => dest.ReporterId, opt => opt.MapFrom(src => src.ReporterId))
				.ForMember(dest => dest.ResponsibleId, opt => opt.MapFrom(src => src.ResponsibleId))
				.ForMember(dest => dest.TagId, opt => opt.MapFrom(src => src.TagId))
				.ForSourceMember(src => src.CreatedOn, opt => opt.DoNotValidate()).ReverseMap();

			CreateMap<Task, AllTaskViewModel>()
				.ForMember(dest => dest.DueTime, opt => opt.MapFrom(src=> src.DueDate.ToUniversalTime()))
				.ForMember(dest => dest.Reporter, opt => opt.MapFrom(src => src.Reporter.UserName))
				.ForMember(dest => dest.Responsible, opt => opt.MapFrom(src => src.Responsible.UserName))
				.ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.Tag.Name));

			CreateMap<AddTaskViewModel, Task>()
				.ForMember(dest => dest.DueDate,opt => opt.MapFrom(src=> src.DueTime.ToUniversalTime()));
		}
	}
}
