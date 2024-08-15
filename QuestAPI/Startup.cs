using System;
using Microsoft.EntityFrameworkCore;
using QuestAPI.Data;
using QuestAPI.Models;

namespace QuestAPI{
    public class Startup{
        public Startup(IConfiguration configuration){
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services){
            services.AddDbContext<QuestDbContext>(options => options.UseSqlite("Data Source=QuestDB.db"));
            services.AddControllers();
            services.AddSwaggerGen();
            services.Configure<QuestConfig>(Configuration.GetSection("QuestConfig"));
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env){
            if (env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
