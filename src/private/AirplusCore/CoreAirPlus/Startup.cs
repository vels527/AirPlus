using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using CoreAirPlus.Services;
using CoreAirPlus.Data;
using Microsoft.EntityFrameworkCore;


namespace CoreAirPlus
{
    public class Startup
    {

        public IConfiguration Configuration { get; set; }
        public Startup(IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            if (environment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var conn = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataDBContext>(options => options.UseSqlServer(conn));
            services.AddMvc();
            services.AddTransient<IDbReadService, DbReadService>();
            //services.AddTransient<Id>
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            //

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(Configuration["Message"]);
            });
        }
    }
}
