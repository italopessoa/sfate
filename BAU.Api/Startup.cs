using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAU.Api.DAL.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BAU.Api.DAL.Repositories;
using BAU.Api.DAL.Repositories.Interface;
using AutoMapper;
using BAU.Api.Models;
using BAU.Api.Service.Interface;
using BAU.Api.Service;

namespace BAU.Api
{
    public partial class Startup
    {
        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration">Represents a set of key/value application configuration properties</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Application settings
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configure DI Services
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .Build()
                );
            });

            services.AddScoped<IShiftRepository,ShiftRepository>();
            services.AddScoped<IShiftService,ShiftService>();
            services.AddDbContext<BAUDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("SqlServer"))
            );

            ConfigureServicesJWT(services);
            ConfigureServicesSwagger(services);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Specify how the application will responde to HTTP requests
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile<BAUMappingProfile>();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<BAUDbContext>();
                dbContext.Database.EnsureCreated();
            };

            app.UseCors("CorsPolicy");
            ConfigureSwagger(app);
            ConfigureJWT(app);
            app.UseMvc();
        }
    }
}
