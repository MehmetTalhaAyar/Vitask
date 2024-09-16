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
	public class UserMappingProfile : Profile
	{

		public UserMappingProfile()
		{
			CreateMap<AppUser, UserViewModel>()
				.ForMember(dest => dest.LastName,opt => opt.MapFrom(src=> src.Surname))
				.ReverseMap();

			CreateMap<SignUpViewModel, AppUser>()
				.ForMember(dest => dest.UserName,opt=> opt.MapFrom(src=> src.Name+src.Surname))
				.ForSourceMember(src => src.Password, opt => opt.DoNotValidate());

			CreateMap<AppUser, ProfileViewModel>()
				.ForMember(dest => dest.PictureUrl , opt=> opt.MapFrom(src=> src.Image))
				.ForMember(dest => dest.About, opt => opt.Condition(src=> src.UserInfo != null && src.UserInfo.About != null))
				.ForMember(dest => dest.About , opt => opt.MapFrom(src=> src.UserInfo.About))
				.ForMember(dest => dest.Location, opt => opt.Condition(src => src.UserInfo != null && src.UserInfo.Location != null))
				.ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.UserInfo.Location))
				.ForMember(dest => dest.Title, opt => opt.Condition(src => src.UserInfo != null && src.UserInfo.Title != null))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.UserInfo.Title));


			CreateMap<UpdateUserInfoViewModel, UserInfo>();
		}
	}
}
