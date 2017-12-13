using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace BAU.Api
{
    public partial class Startup
    {
        private void ConfigureServicesSwagger(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Support Wheel of Fate API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Italo Pessoa", Email = "", Url = "https://github.com/italopessoa/support-wheel-of-fate" },
                    License = new License { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
                });

                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "BAU.Api.xml");
                c.IncludeXmlComments(xmlPath);
            });

        }

        private void ConfigureSwagger(Microsoft.AspNetCore.Builder.IApplicationBuilder app)
        {
            //Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BAU API V1");
            });
        }
    }
}
