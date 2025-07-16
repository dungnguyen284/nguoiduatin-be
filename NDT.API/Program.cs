using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using NDT.BusinessLogic.AutoMapper;
using NDT.BusinessLogic.Services.Implementations;
using NDT.BusinessLogic.Services.Interfaces;
using NDT.BusinessModels.Entities;
using NDT.DataAccess.Data;
using NDT.DataAccess.Repositories;
using NDT.DataAccess.UnitOfWork;
using System.Text;

namespace NDT.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure Identity
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure JWT
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });


            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Register Repositories
            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

            // Register UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Services
            builder.Services.AddScoped<INewsService, NewsService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAdvertisementService, AdvertisementService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IStockInforService, StockInfoService>(provider =>
            {
                var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
                var mapper = provider.GetRequiredService<AutoMapper.IMapper>();
                var grpcService = provider.GetRequiredService<IGrpcService>();
                return new StockInfoService(unitOfWork, mapper, grpcService);
            });

            // Đăng ký IGrpcService với endpoint từ cấu hình
            var grpcEndpoint = builder.Configuration.GetSection("GrpcSettings:Endpoint").Value;
            builder.Services.AddScoped<IGrpcService>(provider => new GrpcService(grpcEndpoint));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "ex: Bearer {token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<News>("news"); 

            builder.Services.AddControllers()
                .AddOData(opt => opt
                    .AddRouteComponents("odata", odataBuilder.GetEdmModel())
                    .Select().Filter().OrderBy().Expand().SetMaxTop(100).Count());

            var app = builder.Build();

            // Seed Data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await DataSeeder.SeedRolesAndUsersAsync(userManager, roleManager);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
