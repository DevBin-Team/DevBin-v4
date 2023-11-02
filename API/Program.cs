using API.Utils;
using Common.Data;
using Common.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var sqlServerVersion = ServerVersion.AutoDetect(connectionString);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, sqlServerVersion));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var openApiInfo = new OpenApiInfo()
    {
        Title = "DevBin",
        Version = "v4",
        Description = "This API provides access to the most common features of the service.<br/>" +
    "A developer API token is required and must be put in the request header as \"Authorization\"." +
    "<h4>API Rate limit</h4>" +
    "<p>POST API requests are limited to max 10 requests every 60 seconds.<br/>" +
    "All other methods are limited to max 10 requests every 10 seconds.</p>",
        License = new()
        {
            Name = "GNU AGPLv3",
            Url = new("https://github.com/DevBin-Team/DevBin/blob/main/LICENSE.txt"),
        },
        Contact = new()
        {
            Name = "DevBin Support",
            Email = "support@devbin.dev",
        },
        TermsOfService = new("https://devbin.dev/tos"),
    };

    options.SwaggerDoc("v4", openApiInfo);

    options.DocumentFilter<ApiNameNormalizeFilter>();
    options.SchemaFilter<EnumSerializeFilter>();

    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "API.xml"));
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Common.xml"));


    var securityScheme = new OpenApiSecurityScheme()
    {
        Description = "Specify the API authorization token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Authorization",
    };

    options.AddSecurityDefinition("api_key", securityScheme);

    var requirements = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "api_key"
                }
            }, Array.Empty<string>()
        }
    };
    options.AddSecurityRequirement(requirements);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "DevBin API v4";
        options.SwaggerEndpoint("/swagger/v4/swagger.json", "DevBin API v4");

    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
