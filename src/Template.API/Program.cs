using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Template.API.Middlewares;
using Template.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

#region DB
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TemplateDbContext>(options =>
{
    options.UseNpgsql(connection,
       assembly => assembly.MigrationsAssembly(typeof(TemplateDbContext).Assembly.FullName));
});
#endregion

#region JWt
var bytes = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:JwtSecret"]!);
var audience = builder.Configuration["Authentication:ValidAudience"];
var issuer = builder.Configuration["Authentication:ValidIssuer"];

builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(bytes),
        ValidAudience = "authenticated",
        ValidIssuer = "" // Supabase's endpoint
    };
});

#endregion

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.ResolveDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<AccessTokenMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
