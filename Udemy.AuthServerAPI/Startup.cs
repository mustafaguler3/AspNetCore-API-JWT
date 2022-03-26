using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udemy.Shared.Configuration;
using Udemy.Shared.Extensions;
using UdemyAuthServer.Core.Configuration;
using UdemyAuthServer.Core.Entities;
using UdemyAuthServer.Core.Repositories;
using UdemyAuthServer.Core.Repositories.Service;
using UdemyAuthServer.Core.Service;
using UdemyAuthServer.Core.UnitOfWork;
using UdemyAuthServer.Data;
using UdemyAuthServer.Data.Context;
using UdemyAuthServer.Data.Repositories;
using UdemyAuthServer.Service.Service;

namespace Udemy.AuthServerAPI
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
            //DI Container'a Ekledik buralarý
            services.AddScoped<AuthenticationService,AuthenticationService>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IService<,>), typeof(GenericService<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("SqlServer"), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("UdemyAuthServer.Data");//tüm migration burada oluþacak
                });
            });

            services.AddIdentity<UserApp, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();//þifre sýfýrlama iþlemleri
                                            //için default token provider saðlýyoruz


            services.Configure<CustomTokenOption>(Configuration.GetSection("TokenOption"));//TokenOption appsetting içinde kullandýðýmýz isim

            var tokenOptions = Configuration.GetSection("TokenOption").Get<CustomTokenOption>();

            services.Configure<List<Client>>(Configuration.GetSection("Clients"));

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience[0],
                    IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

                    ValidateIssuerSigningKey = true, //imzayý doðrulucam
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero //5dk filan ekleme 0 a 0 tolere olan deðeri de 0 ladýk
                };

            });

            
            services.AddControllers().AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssemlyContaining<Startup>();
            });

            services.UseCustomValidationResponse();
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
                app.UseCustomException();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
