using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Threading.Channels;
using TwitterStreamApi.Clients;
using TwitterStreamApi.Models.TweetModels;
using TwitterStreamApi.Services.TweetProcessing;

namespace TwitterStreamApi
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TwitterStreamApi", Version = "v1" });
            });

            StartupClients.ConfigureServices(services, Configuration);
            services.AddHostedService<TweetConsumer>();
            var channel = Channel.CreateUnbounded<Tweet>();
            services.AddSingleton(channel.Reader);
            services.AddSingleton(channel.Writer);
            services.AddSingleton<TweetStreamProducer>();
            services.AddTransient<ITweetParser, TweetParser>();
            services.AddTransient<ITweetProcessor, TweetProcessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TwitterStreamApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
