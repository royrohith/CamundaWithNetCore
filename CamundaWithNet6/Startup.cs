using CamundaInstance.Camunda.Camunda.Contracts;
using CamundaInstance.Camunda.Camunda.Core;
using CamundaInstance.Camunda.Camunda.ExternalTasks;
using CamundaInstance.Domain.Hasura;
using CamundaInstance.Domain.Hasura.Contracts;
using CamundaInstance.Domain.Hasura.GraphQL;

namespace CamundaInstance.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<CamundaSettings>(Configuration.GetSection("CamundaSettings"));
            services.Configure<HasuraSettings>(Configuration.GetSection("HasuraSettings"));
            services.AddScoped<IEngineClient, EngineClient>();
            services.AddScoped<ITaskPollingService, TaskPollingService>();
            services.AddScoped<IHasuraService, HasuraService>();
            services.AddScoped<IGraphQLRepository, GraphQLRepository>();
            services.AddScoped<IExternalTaskExecutor, InsertIntoDB>();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
