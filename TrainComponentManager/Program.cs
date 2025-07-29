using Microsoft.EntityFrameworkCore;
using TrainComponentManager.Components;
using TrainComponentManager.Data.Models;
using TrainComponentManager.Data.Repositories;

namespace TrainComponentManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IRepository<TrainComponent>, TrainComponentRepository>();

            builder.Services.AddHttpClient("LocalApi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:8081");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                if (builder.Environment.IsDevelopment() &&
                    builder.Configuration["ASPNETCORE_URLS"]?.Contains("https", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                }
                return new HttpClientHandler();
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<AppDbContext>();
                        context.Database.Migrate();
                        Console.WriteLine("Database migrations applied successfully.");
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while migrating the database.");
                        throw;
                    }
                }
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.MapControllers();

            app.Run();
        }
    }
}
