using Api.Handler;
using Api.ServiceConfiguration.AutomapperConfiguration;
using AppServices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Model.Business;
using Model.Entities;
using Model.Repository;
using Repository;
using Repository.Context;

namespace Api.ServiceConfiguration
{
	public static class ServiceBuilder
	{
        /// <summary>
        /// Extension Method to group the IServiceCollection Definition for our app.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configuration">IConfiguration</param>
		public static void Build(this IServiceCollection services, IConfiguration configuration)
		{
            SetDatabase(services, configuration);

            SetControllerAndFilters(services);

            SetupAutomapper(services);

            SetIOC(services);
        }

		private static void SetControllerAndFilters(this IServiceCollection services)
		{
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new ExceptionHandler());
            });
        }

		private static void SetDatabase(this IServiceCollection services, IConfiguration configuration)
		{
            // Read from Appsettings
            services.Configure<IDBContextConfig>(configuration.GetSection(nameof(DBContextConfig)));
            services.AddSingleton<IDBContextConfig>(sp => sp.GetRequiredService<IOptions<DBContextConfig>>().Value);

            // Set the Context
            services.AddDbContext<BlogContext>(options => options.UseSqlServer(configuration["BlogContext:ConnectionString"]));
		}

        private static IServiceCollection SetupAutomapper(IServiceCollection serviceCollection)
        {
            var mapper = new AutoMapperConfig();

            serviceCollection.AddSingleton(mapper.Mapper);

            return serviceCollection;
        }

        private static void SetIOC(this IServiceCollection services)
        {
            services.AddScoped<IPostAppService, PostAppService>();
            services.AddScoped<ICommentAppService, CommentAppService>();
            services.AddScoped<ICommentBusiness, CommentBusiness>();
            services.AddScoped<IPostBusiness, PostBusiness>();
            services.AddScoped<IBaseRepository<Comment>, BaseRepository<Comment>>();
            services.AddScoped<IBaseRepository<Post>, BaseRepository<Post>>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
        }

    }
}
