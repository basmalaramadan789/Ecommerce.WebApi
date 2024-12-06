using Ecommerce.Core.Identity;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Identity;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Infrastructure.Services;
using Ecommerce.Web.MiddleWare;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connect to database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<IdentityAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    // Configure Identity options here if needed
})
.AddRoles<ApplicationRole>() // Add role support
.AddEntityFrameworkStores<IdentityAppDbContext>() // Use Entity Framework for persistence
.AddSignInManager<SignInManager<ApplicationUser>>() // Add SignInManager for authentication
.AddRoleManager<RoleManager<ApplicationRole>>() // Add RoleManager for role management
.AddDefaultTokenProviders();


var jwt = builder.Configuration.GetSection("Token");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["key"])),
            ValidIssuer = jwt["Issuer"],
            ValidateIssuer = true,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();

// Dependency Injection (DI)
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IWishListItemService, WishLisrItemService>();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));



builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
builder.Services.AddHttpContextAccessor(); 
//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var options = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
    return ConnectionMultiplexer.Connect(options);
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Next Driven API", Version = "v1" });
    c.ResolveConflictingActions(x => x.First());
    // Swagger 2.+ support
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                //Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExeptionMiddleWare>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Apply migrations and seed the database
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetService<ApplicationDbContext>();
var identityContext = services.GetRequiredService<IdentityAppDbContext>();
var userManger = services.GetRequiredService<UserManager<ApplicationUser>>();
var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    await context.Database.MigrateAsync();
    await identityContext.Database.MigrateAsync();
    await ContextSeed.SeedAsync(context);

    await IdentityAppSeeding.SeedRolesAsync(roleManager); // Seed roles
    await IdentityAppSeeding.SeedUserAsync(userManger, roleManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during migration");
}

// Run the application
await app.RunAsync();
