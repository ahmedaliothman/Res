using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ResidencyApplication.Services.Models.DomainModels;
using ResidencyApplication.Services.Models.EntityModels;
using ResidencyApplication.Services.Models.jwt;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using ResidencyApplication.Services.Mapping;
using AutoMapper;
using ResidencyApplication.Services.Models.Settings;
using ResidencyApplication.Services.Models.Services;
using Newtonsoft.Json;
using Hangfire;
using Hangfire.SqlServer;
using ResidencyApplication.Services.Extensions;
using ResidencyApplication.Services.Exceptions;

namespace ResidencyApplication.Services
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Email Configuration 
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            // sms Configuration 
            services.Configure<SMSSettings>(Configuration.GetSection("SMS"));
            services.AddTransient<ISMSHelper, SMSHelper>();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            string connStr = Configuration.GetConnectionString("DevConnectionString");
            services.AddDbContext<ResidencyApplicationContext>(options => options.UseSqlServer(connStr));
            services.AddCors(action => action.AddPolicy("all", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            #region  SwaggerConfigure
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                // c.SwaggerDoc("v1.0", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Main API v1.0", Version = "v1.0" });

                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
            });

            #endregion
            #region jwtConfigure
            //services.AddOptions();
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        //set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddScoped<IJwtServices, JwtServices>();
            #endregion
            //enable json converter from to json tyoes and c# types 
            //services.AddControllers().AddNewtonsoftJson();
            services.AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });


            #region  HangFire
            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(Configuration.GetConnectionString("DevConnectionString"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true,

            }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            #endregion

            // add Configration to access HttpContext in services
            StaticHttpContextExtensions.AddHttpContextAccessor(services);
            // 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticHttpContext();
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;

            });
            app.UseSwagger(option => option.RouteTemplate = "swagger/{documentName}/swagger.json");
            app.UseSwaggerUI(option => option.SwaggerEndpoint("v1/swagger.json", "Residency Application API"));
            // Enable middleware to serve generated Swagger as a JSON endpoint.

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Uploads")),
                RequestPath = "/Uploads",
                ServeUnknownFileTypes = true,
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                }
            });
            app.UseSpaStaticFiles();
            app.UseRouting();
            //to enable cors
            app.UseCors(builder => builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireServer();
              var options = new DashboardOptions
            {
                Authorization = new[] { new CustomAuthorizationFilter() }
            };
            app.UseHangfireDashboard("/hangfire", options);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

        }
    }
}
