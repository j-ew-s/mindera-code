using AppServices.Mapper;
using AutoMapper;

namespace Api.ServiceConfiguration.AutomapperConfiguration
{
	public class AutoMapperConfig
	{
        public IMapper Mapper { get; set; }

        public AutoMapperConfig()
		{
            var mappingConfig = new AutoMapper.MapperConfiguration(mc =>
            {
                mc.AddProfile(new CommentMapper());
                mc.AddProfile(new PostMapper());
            });

            Mapper = mappingConfig.CreateMapper();
        }
	}
}

