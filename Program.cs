using Redgate.Text_Migrations_v2.Core;
using Redgate.Text_Migrations_v2.Respositories;

namespace Redgate.Text_Migrations_v2
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var allowLocalHost = "allowLocalHost";

            var builder = WebApplication.CreateBuilder(args);
            
            ConfigureServices(builder.Services);
            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: allowLocalHost, policy =>
                {
                    policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
                });
            });

            var app = builder.Build();
            
            app.MapGet("/", () => "This is Redgate.TextMigrations");
            app.UseRouting();
            app.UseCors(allowLocalHost);
            app.UseEndpoints(ep => ep.MapControllers());
            app.Run();
            
            return 0;
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDatabaseRepository, Repository>();
        }
    }
}