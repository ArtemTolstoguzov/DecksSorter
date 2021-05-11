using DecksSorter.DTO;
using DecksSorter.Models;
using DecksSorter.Repositories;
using DecksSorter.Shufflers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DecksSorter
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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "DecksSorter", Version = "v1"});
            });
            // services.AddDbContext<DecksContext>(options =>
            //     options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")));
            // services.AddDbContext<DecksContext>(options =>
            //     options.UseNpgsql("User ID=postgres;Password=postgres;Host=localhost;Database=Decks;Integrated Security=true;Pooling=true;"));
            services.AddSingleton<IShuffler, SimpleShuffler>();
            services.AddSingleton<IConverter<Deck, DeckDto>, DeckConverter>();
            services.AddSingleton<IConverter<Card, CardDto>, CardConverter>();
            services.AddScoped<IDeckRepository, DeckRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DecksSorter v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
