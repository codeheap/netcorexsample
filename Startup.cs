using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherMicroservice;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WheaterMan
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.isDevelopment())
            //{
              //  app.UseDeveloperExceptionPage();
            //}
            //else
            //{
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            //}

            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseCookiePolicy();
            //app.UseMvc();

            app.Run(async context =>
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
                var latString = context.Request.Query["lat"].FirstOrDefault();
                var longString = context.Request.Query["long"].FirstOrDefault();

                var latitude = latString.TryParse();
                var longitude = longString.TryParse();

                if (latitude.HasValue && longitude.HasValue)
                {
                    var forecast = new List<WeatherReport>();
                    for (var days = 1; days <= 5; days++)
                    {
                        forecast.Add(new WeatherReport(latitude.Value, longitude.Value, days));
                    }

                    var json = JsonSerializer.Serialize(forecast);
                    context.Response.ContentType = "application/json; charset=utf-8";
                    await context.Response.WriteAsync(json);
                }
                else{
                    await context.Response.WriteAsync($"Retrieving Weather for lat: {latitude}, long: {longitude}");
                }

            });

        }
    }

    public static class Extensions
    {
        public static double? TryParse(this string input)
        {
            if (double.TryParse(input, out var result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
