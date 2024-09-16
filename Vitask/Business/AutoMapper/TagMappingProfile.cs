using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Concrete;
using Vitask.Models;

namespace Business.AutoMapper
{
	public class TagMappingProfile : Profile
	{
		public TagMappingProfile()
		{
			CreateMap<Tag, AllTagsViewModel>();



		}
	}
}
