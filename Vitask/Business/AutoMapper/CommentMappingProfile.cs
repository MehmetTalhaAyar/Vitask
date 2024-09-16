using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;

namespace Business.AutoMapper
{
	public class CommentMappingProfile : Profile
	{
		public CommentMappingProfile()
		{
			

			CreateMap<Comment, CommentViewModel>();
		}
	}
}
