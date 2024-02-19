using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace AirStatusesAPI.Configuratios.Swagger
{
    public class SwaggerService
    {
        private readonly string _xmlFile;
        private readonly string _xmlPath;

        public SwaggerService()
        {
            _xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            _xmlPath = Path.Combine(AppContext.BaseDirectory, _xmlFile);
        }

        public void ConfigureSwaggerGen(SwaggerGenOptions c)
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "AIR Statuses Control", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
            c.IncludeXmlComments(_xmlPath);
        }
    }
}
