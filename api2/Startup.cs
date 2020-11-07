using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api2.Data;
using api2.Models;
using api2.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace api2
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

            services.CustomeDbContext(Configuration)
                .CustomeSwagger(Configuration)
                .CustomeAutoMapper(Configuration);
          

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/Department/swagger.json", "Department");
                options.RoutePrefix = "";
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

       
    }
    static class ExtensionMehod
    {
        public static IServiceCollection CustomeAutoMapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(options =>
            {
                options.CreateMap<Department, CreateDepartmentDto>().ReverseMap();
            });

            return services;
        }

        public static IServiceCollection CustomeDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

        public static IServiceCollection CustomeSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("Department", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Department API",
                    Version = "V1",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Email = "ariqbinrazali@gmail.com",
                        Name = "Ariq"
                    }
                });

            });

            return services;
        }
    }
}
