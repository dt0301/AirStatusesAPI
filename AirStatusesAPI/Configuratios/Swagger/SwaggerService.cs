using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace AirStatusesAPI.Configuratios.Swagger
{
    /// <summary>
    /// Класс для настройки Swagger.
    /// </summary>
    public class SwaggerService
    {
        private readonly string _xmlFile;
        private readonly string _xmlPath;

        /// <summary>
        /// Конструктор, инициализирующий пути к XML-файлу с комментариями для Swagger.
        /// </summary>
        public SwaggerService()
        {
            _xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            _xmlPath = Path.Combine(AppContext.BaseDirectory, _xmlFile);
        }

        /// <summary>
        /// Настраивает SwaggerGen с заданными параметрами.
        /// </summary>
        /// <param name="c">Объект SwaggerGenOptions для настройки.</param>
        public void ConfigureSwaggerGen(SwaggerGenOptions c)
        {
            // Создание документа Swagger
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "AIR Statuses Control", Version = "v1" });

            // Добавление определения безопасности для JWT-авторизации
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            // Добавление требования безопасности для JWT-авторизации
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

            // Включение XML-комментариев
            c.IncludeXmlComments(_xmlPath);
        }
    }
}

