using Application;
using Application.Factory;
using Application.IFactory;
using Application.IServices;
using Application.Services;
using AutoMapper;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ApiKeyPOC
{
    public class Startup
    {
        readonly string AllowAll = "_allowAll";
        private readonly ILogger _Logger;
        public Startup(IConfiguration configuration, ILogger<Startup> logger, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _Logger = logger;

            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddConfiguration(configuration)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IServiceAuthentication, ServiceAuthentication>();
            services.AddScoped<IServiceApplication, ServiceApplication>();
            services.AddScoped<IServiceGeneral, ServiceGeneral>();
            services.AddScoped<IServiceClient, ServiceClient>();
            services.AddScoped<IServiceKey, ServiceKey>();
            services.AddScoped<IServiceLogApikeyDB, ServiceLog>();


            services.AddScoped<IAbstractServiceFactory, ConcreteServiceFactory>();

            services.AddDbContext<ApiKeyDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SQLServer")));

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            var serviceProvider = services.BuildServiceProvider();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

            var logger = serviceProvider.GetService<ILogger<Startup>>();
            services.AddSingleton(typeof(ILogger), logger);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddMvc().AddNewtonsoftJson(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            MapperConfiguration mapperConfiguration = new AutoMapper.MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfileConfiguration("default"));
            });

            IMapper mapper = mapperConfiguration.CreateMapper();


            string ConnectionString = Configuration.GetConnectionString("SQLServer");
            string[] arr = ConnectionString.Split(';');

            string Server = arr[0].Split('=')[1];
            string Database = arr[1].Split('=')[1];

            services.AddSingleton(mapper);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = Configuration.GetSection("SwaggerOptions:Description").Value,
                        Description = $"**Server:** { Server }<br>" +
                         $"**Database:** { Database }<br>" +
                        $"**Runtime:** { System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription }<br>" +
                        $"**netCore Version :** { System.Environment.Version }<br>" +
                        $"**Documentación :**  { Configuration.GetSection("SwaggerOptions:Doc").Value } [link]({ Configuration.GetSection("SwaggerOptions:Doc").Value })"
                    });
            });

            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowAll,
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        ;
                    });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var Active = Configuration.GetSection("Logging:LogFile:Active").Value;
            if (Active.Equals("true"))
            {
                loggerFactory.AddFile(Configuration.GetSection("Logging:LogFile:LogPath").Value);
            }

#if DEBUG || PERSONAL
            app.UseDeveloperExceptionPage();
            _Logger.LogInformation($"In { env.EnvironmentName } environment");
#endif

            var swaggerUrl = Configuration.GetSection("SwaggerOptions:UIEndpoint");
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(Configuration.GetSection("SwaggerOptions:UIEndpoint").Value, "Api");
            });

            var rewrite = new RewriteOptions().AddRedirect("^$", "swagger");
            app.UseRewriter(rewrite);

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireCors(AllowAll);
            });
        }
    }
}
