using AppServices.Models;
using AppServices.Models.Creation;
using AutoMapper;
using Model.Entities;

namespace AppServices.Mapper
{
	public class PostMapper : Profile
	{
		public PostMapper()
		{
			CreateMap<Post, PostDTO>().ReverseMap();
			CreateMap<Post, PostCreateDTO>().ReverseMap();
		}
	}
}

