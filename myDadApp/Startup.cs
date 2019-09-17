using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using myDadApp.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.Azure.Cosmos;

namespace myDadApp
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


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<myDataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("myDataContext")));

            // MB: Add Cosmos
            var client = new CosmosClient(Configuration.GetConnectionString("myCosmos"));
            Database db = client.CreateDatabaseIfNotExistsAsync("myDadApp").Result;
            Container myCosmos = db.CreateContainerIfNotExistsAsync("Chores", "/Owner", 400).Result;
            services.AddSingleton<Container>(myCosmos);


            // MB: Add Storage for uploads
            CloudStorageAccount AzAcct = CloudStorageAccount.Parse(Configuration.GetConnectionString("myStorage"));

            var azTableClient = AzAcct.CreateCloudTableClient();
            CloudTable azTable = azTableClient.GetTableReference("Uploads");
            var rc = azTable.CreateIfNotExistsAsync().Result;
            services.AddSingleton<CloudTable>(azTable);

            var azBlobClient = AzAcct.CreateCloudBlobClient();
            var azContainer = azBlobClient.GetContainerReference("uploads");
            rc = azContainer.CreateIfNotExistsAsync().Result;
            services.AddSingleton<CloudBlobContainer>(azContainer);

            var azQClient = AzAcct.CreateCloudQueueClient();
            var azQueue = azQClient.GetQueueReference("new-uploads");
            rc = azQueue.CreateIfNotExistsAsync().Result;
            services.AddSingleton<CloudQueue>(azQueue);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Uploads}/{action=Index}/{id?}");
            });

            // MB: Add auto migrations
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<myDataContext>().Database.Migrate();
            }


        }
    }
}
