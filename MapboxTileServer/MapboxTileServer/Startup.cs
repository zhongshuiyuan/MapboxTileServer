// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MapboxTileServer.Clients;
using MapboxTileServer.Options;
using MapboxTileServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MapboxTileServer
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
            // Add CORS:
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policyBuilder => policyBuilder
                        .WithOrigins("http://localhost:4200", "http://localhost:8080", "http://localhost:9000")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddOptions();

            ConfigureOptions(services);
            RegisterApplicationServices(services);

            services.AddMvc();
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.Configure<ApplicationOptions>(Configuration.GetSection("Application"));
        }

        private void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddSingleton<PhotonSearchClient>();
            services.AddSingleton<IMapboxTileService, MapboxTileService>();
            services.AddSingleton<IKmlConverterService, KmlConverterService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICorsService corsService, ICorsPolicyProvider corsPolicyProvider)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            // To serve PBF Files, we need to allow unknown filetypes 
            // to be served by the Webserver:
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                OnPrepareResponse = (ctx) =>
                {
                    var policy = corsPolicyProvider.GetPolicyAsync(ctx.Context, "CorsPolicy")
                        .ConfigureAwait(false)
                        .GetAwaiter().GetResult();

                    var corsResult = corsService.EvaluatePolicy(ctx.Context, policy);

                    corsService.ApplyResult(corsResult, ctx.Context.Response);
                }
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireCors("CorsPolicy");
            });
        }
    }
}
