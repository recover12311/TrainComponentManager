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
                // BaseAddress устанавливается независимо от того, используется HTTPS или нет
                // Так как в Docker Compose мы указали оба HTTP и HTTPS URL,
                // приложение само решит, на каком порту слушать по умолчанию,
                // но для HttpClient мы явно указываем, к какому порту API обращаться.
                // Для локальных вызовов в контейнере, как правило, безопаснее использовать HTTP,
                // если нет строгой необходимости в HTTPS внутри контейнера для dev.
                // Но если вы хотите продолжать использовать HTTPS, то нужно игнорировать сертификат.

                // Если API вызывается по HTTPS (через 8081), то используйте этот:
                client.BaseAddress = new Uri("https://localhost:8081");

                // Если вы хотите использовать HTTP (через 8080), что часто проще для Docker dev:
                // client.BaseAddress = new Uri("http://localhost:8080");

                // Выбираем HTTPS как приоритет, так как он был в ASPNETCORE_URLS
                // Это сделает BaseAddress https://localhost:8081
                // Если вы хотите всегда использовать HTTP для внутренних вызовов,
                // просто поставьте client.BaseAddress = new Uri("http://localhost:8080");
            })
            // *** ДОБАВЛЯЕМ ОБРАБОТЧИК ДЛЯ ИГНОРИРОВАНИЯ SSL ОШИБОК В РЕЖИМЕ РАЗРАБОТКИ ***
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                // Этот код будет выполняться, только если среда - Development
                // и если в ASPNETCORE_URLS настроен HTTPS.
                // Это позволяет игнорировать ошибки SSL-сертификата в Docker для dev-целей.
                if (builder.Environment.IsDevelopment() &&
                    builder.Configuration["ASPNETCORE_URLS"]?.Contains("https", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                }
                // Для других случаев (например, Production или если нет HTTPS) используем стандартный обработчик.
                return new HttpClientHandler();
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // *** ДОБАВИТЬ ЭТОТ БЛОК ДЛЯ АВТОМАТИЧЕСКОГО ПРИМЕНЕНИЯ МИГРАЦИЙ ***
            // Это удобно для разработки и тестирования в Docker.
            // Для продакшена обычно используют отдельный процесс для миграций.
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    // Убедитесь, что база данных SQL Server полностью готова.
                    // Иногда SQL Server запускается медленнее, чем .NET приложение.
                    // Можно добавить небольшую задержку или логику повторных попыток здесь,
                    // но для простых сценариев чаще всего достаточно.
                    context.Database.Migrate(); // Применить все ожидающие миграции
                    Console.WriteLine("Database migrations applied successfully.");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                    // Вы можете даже выкинуть исключение, чтобы контейнер упал
                    // и вы знали, что миграция не удалась.
                    // throw;
                }
            }
            // *** КОНЕЦ БЛОКА АВТОМАТИЧЕСКОГО ПРИМЕНЕНИЯ МИГРАЦИЙ ***

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
