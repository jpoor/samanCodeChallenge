using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saman.Backend.Business.DataAccess;
using Saman.Backend.Business.Entity.Authentication;
using Saman.Backend.Share.shareMiddlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ErrorHandlingMiddleware>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCorsPolicy(builder.Configuration, MyAllowSpecificOrigins);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddHttpClient();

builder.Services.AddMemoryCache();

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });

builder.Services.AddDbContext<mainDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("mainDbContext")));

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<User_dBo, IdentityRole>(options =>
{
    // Set password options
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<mainDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCustomAuthentication(builder.Configuration);

builder.Services.AddScopeServices();

var app = builder.Build();

app.UseMiddleware<LanguageHandlingMiddleware>();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
