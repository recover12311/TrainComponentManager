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

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IRepository<TrainComponent>, TrainComponentRepository>();

            builder.Services.AddHttpClient("LocalApi", client =>
            {
                // BaseAddress ��������������� ���������� �� ����, ������������ HTTPS ��� ���
                // ��� ��� � Docker Compose �� ������� ��� HTTP � HTTPS URL,
                // ���������� ���� �����, �� ����� ����� ������� �� ���������,
                // �� ��� HttpClient �� ���� ���������, � ������ ����� API ����������.
                // ��� ��������� ������� � ����������, ��� �������, ���������� ������������ HTTP,
                // ���� ��� ������� ������������� � HTTPS ������ ���������� ��� dev.
                // �� ���� �� ������ ���������� ������������ HTTPS, �� ����� ������������ ����������.

                // ���� API ���������� �� HTTPS (����� 8081), �� ����������� ����:
                client.BaseAddress = new Uri("https://localhost:8081");

                // ���� �� ������ ������������ HTTP (����� 8080), ��� ����� ����� ��� Docker dev:
                // client.BaseAddress = new Uri("http://localhost:8080");

                // �������� HTTPS ��� ���������, ��� ��� �� ��� � ASPNETCORE_URLS
                // ��� ������� BaseAddress https://localhost:8081
                // ���� �� ������ ������ ������������ HTTP ��� ���������� �������,
                // ������ ��������� client.BaseAddress = new Uri("http://localhost:8080");
            })
            // *** ��������� ���������� ��� ������������� SSL ������ � ������ ���������� ***
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                // ���� ��� ����� �����������, ������ ���� ����� - Development
                // � ���� � ASPNETCORE_URLS �������� HTTPS.
                // ��� ��������� ������������ ������ SSL-����������� � Docker ��� dev-�����.
                if (builder.Environment.IsDevelopment() &&
                    builder.Configuration["ASPNETCORE_URLS"]?.Contains("https", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                }
                // ��� ������ ������� (��������, Production ��� ���� ��� HTTPS) ���������� ����������� ����������.
                return new HttpClientHandler();
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // *** �������� ���� ���� ��� ��������������� ���������� �������� ***
            // ��� ������ ��� ���������� � ������������ � Docker.
            // ��� ���������� ������ ���������� ��������� ������� ��� ��������.
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    // ���������, ��� ���� ������ SQL Server ��������� ������.
                    // ������ SQL Server ����������� ���������, ��� .NET ����������.
                    // ����� �������� ��������� �������� ��� ������ ��������� ������� �����,
                    // �� ��� ������� ��������� ���� ����� ����������.
                    context.Database.Migrate(); // ��������� ��� ��������� ��������
                    Console.WriteLine("Database migrations applied successfully.");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                    // �� ������ ���� �������� ����������, ����� ��������� ����
                    // � �� �����, ��� �������� �� �������.
                    // throw;
                }
            }
            // *** ����� ����� ��������������� ���������� �������� ***

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
