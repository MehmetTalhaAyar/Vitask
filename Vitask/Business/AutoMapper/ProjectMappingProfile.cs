using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Models;
using Entities.Concrete;
using Vitask.Models;

namespace Business.AutoMapper
{
	public class ProjectMappingProfile : Profile
	{
		public ProjectMappingProfile()
		{

			CreateMap<Project, ProjectViewModel>()
				.ForMember(dest => dest.Leader,opt=> opt.MapFrom(src=> src.Commander));

			CreateMap<AddProjectViewModel, ProjectViewModel>()
				.ForSourceMember(dest => dest.UserIds, opt => opt.DoNotValidate());

			CreateMap<Project, UpdateProjectViewModel>()
				.ForMember(dest => dest.UserIds, opt => opt.MapFrom(src => src.Users.Select(x => x.UserId).ToList())).ReverseMap();
		}
	}
}
