using eShopSolution.Database.Entity;
using eShopSolutions.Application.Catalog.Category;
using eShopSolutions.Application.Catalog.Product;
using eShopSolutions.Application.Common;
using eShopSolutions.Application.System.Language;
using eShopSolutions.Application.System.Roles;
using eShopSolutions.Application.System.Users;
using eShopSolutions.Application.Utilities;
using eShopSolutions.Database.EF;
using eShopSolutions.Utilities.Contains;
using eShopSolutions.ViewModels.System.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace eShopSolutions.BackendAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<EShopDbContext>(optionsAction =>
            {
                string connecString = Configuration.GetConnectionString("eShopSolutionDb");
                optionsAction.UseSqlServer(connecString);
            });
            services.AddTransient<IStorageService, FileStorageService>();
            services.AddTransient<IPublicProductServices, PublicProductServices>();
            services.AddTransient<IManagerProductServices, ManagerProductServices>();
            services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
            services.AddTransient<IUserServices, UserSevices>();

            services.AddTransient<IRolesServices, RolesServices>();
            services.AddTransient<ILanguageServices, LanguageServices>();
            services.AddTransient<ISlidesServices, SlidesServices>();

            services.AddTransient<IValidator<LoginRequest>, LoginRequestValidation>();
            services.AddControllers().AddFluentValidation(fv =>fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidation>());
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<EShopDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false; // KHONG bat buoc phai co so
                options.Password.RequireLowercase = false;  // khong bat buoc phai co chu thuong
                options.Password.RequiredLength = 4; // toi thieu 4 ky tu
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // khoa 5 phut
                options.Lockout.MaxFailedAccessAttempts = 5; // sau 5 lan thi khoa
            });



/*
            var secetKey = Configuration["AppSettings:SecretKey"];
            var secetKeyBytes = Encoding.UTF8.GetBytes(secetKey);

            services.AddAuthentication
                (JwtBearerDefaults.AuthenticationScheme)
                // xai jwt dung cho ca mobile, cookie chi danh cho web

                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters // ke thua tu object TokenValidationParameters
                                                                                     // chua tap hop cac tham so Security Token Handler  khi xac thuc 2 lop SecurityToken
                    {
                        // tu cap token
                        ValidateIssuer = false, // tra ve xem nguoi dung co duoc xac thuc thong qua ma xac thuc hay khong
                        ValidateAudience = false, // tra ve doi tuong co duoc xac thuc trong qua trinh xac thuc
                        // ky vao token
                        ValidateIssuerSigningKey = true, // viec xac thuc vao SecurityToken co dc goi hay khong
                        IssuerSigningKey = new SymmetricSecurityKey(secetKeyBytes), // de xac thuc signature 
                        ClockSkew = TimeSpan.Zero
                    };
                });*/
           

          

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger eShopSolution", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme //  dung de bao mat
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"

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
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                    });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
           
            app.UseAuthentication();    
            app.UseAuthorization();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
