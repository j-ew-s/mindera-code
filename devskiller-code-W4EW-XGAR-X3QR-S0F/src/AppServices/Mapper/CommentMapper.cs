using AppServices.Models;
using AppServices.Models.Creation;
using AutoMapper;
using Model.Entities;

namespace AppServices.Mapper
{
	public class CommentMapper : Profile
	{
		public CommentMapper()
		{
			CreateMap<Comment, CommentDTO>().ReverseMap();
			CreateMap<Comment, CommentCreateDTO>().ReverseMap();
        }
	}
}

