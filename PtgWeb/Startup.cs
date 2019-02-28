using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Ptg.DataAccess;
using Ptg.HeightmapGenerator.HeightmapGenerators;
using Ptg.HeightmapGenerator.Interfaces;
using Ptg.Services.Interfaces;
using Ptg.Services.Services;
using Ptg.SplatmapGenerator.Interfaces;
using Ptg.SplatmapGenerator.SplatmapGenerators;
using PtgWeb.Hubs;
using System;

namespace PtgWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSignalR();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(5);//You can set Time   
            });
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            services.AddScoped<IRandomHeightmapGenerator, RandomHeightmapGenerator>();
            services.AddScoped<IFaultHeightmapGenerator, FaultHeightmapGenerator>();
            services.AddScoped<IDiamondSquareGenerator, DiamondSquareGenerator>();
            services.AddScoped<ITerrainService, TerrainService>();
            services.AddSingleton<IRepository, RepositoryInMemory>(); // TODO remove reference to DataAccess, make separate service init classes in the projects
            services.AddScoped<IRandomSplatmapGenerator, RandomSplatmapGenerator>();
            services.AddScoped<IHeightBasedSplatmapGenerator, HeightBasedSplatmapGenerator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
            //}

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors("AllowAll");
            app.UseSignalR(options =>
            {
                options.MapHub<HeightmapHub>("/heightmap");
            });

            app.UseSession();
            app.UseMvc();
        }
    }
}
