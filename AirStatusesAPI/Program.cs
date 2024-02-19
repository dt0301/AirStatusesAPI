using AirStatusesAPI.Providers;
using AirStatusesApp.App.Flights.Queries;
using AirStatusesApp.App.Helpers;
using AirStatusesData;
using AirStatusesData.Services;
using AirStatusesInfrastructure.Interfaces;
using AirStatusesInfrastructure.RedisService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog.Events;
using Serilog;
using Serilog.Formatting.Compact;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Настройка Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("./logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // Использование Serilog в качестве провайдера логирования
builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("Postrgres")));

builder.Services.AddSingleton<IConnectionMultiplexer>(x =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
    try
    {
        return ConnectionMultiplexer.Connect(configuration);
    }
    catch (Exception ex)
    {
        throw new Exception($"Redis connection failed, {ex.Message}");
    }
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddUserDataService();
builder.Services.AddScoped<ICache, RedisCache>();
builder.Services.AddScoped<IDataFlightsService, DataFlightsService>();
builder.Services.AddFlightQueryService();

// Add seeders to the services
builder.Services.AddTransient<RoleSeeder>();
builder.Services.AddTransient<UserSeeder>();
builder.Services.AddTransient<FlightSeeder>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<AuthenticatedUserProvider>();
builder.Services.AddTransient<EncryptionProvider>();
builder.Services.AddTransient(x => x.GetService<AuthenticatedUserProvider>().GetAuthenticatedUser());
builder.Services.AddTransient(x => x.GetService<AuthenticatedUserProvider>().GetAuthenticatedJwtUser());

builder.Services.AddRolesDataSevice();

builder.Services.AddUserProps();

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

var secret = builder.Configuration.GetValue<string>("JwtSecret");
var key = Encoding.ASCII.GetBytes(secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//builder.Services.AddMvc()
//    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddSwaggerGen(c =>
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
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Call the seeders
using (var scope = app.Services.CreateScope())
{
    var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();
    await roleSeeder.SeedAsync();

    var userSeeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
    userSeeder.Seed();

    var flightSeeder = scope.ServiceProvider.GetRequiredService<FlightSeeder>();
    flightSeeder.Seed();
}

app.Run();
