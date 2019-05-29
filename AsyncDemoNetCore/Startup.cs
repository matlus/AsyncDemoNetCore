using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AsyncDemoNetCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("MovieServiceHttpClient");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (httpContext) =>
            {
                var httpClientFactory = app.ApplicationServices.GetService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("MovieServiceHttpClient");

                if (httpContext.Request.Path == "/Home/ASync")
                {
                    var allMovies = await MovieServiceGateway.DownloadDataAsync(httpClient);

                    using (var streamWriter = new StreamWriter(httpContext.Response.Body))
                    using (var jsonTextWriter = new JsonTextWriter(streamWriter))
                    {
                        var jsonSerializer = new JsonSerializer();
                        jsonSerializer.Serialize(jsonTextWriter, allMovies);
                        await streamWriter.FlushAsync();
                    }
                }
                else
                {
                    await httpContext.Response.WriteAsync("Nothing to return");
                }
            });
        }
    }
}
