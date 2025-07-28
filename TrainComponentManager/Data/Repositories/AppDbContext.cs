using Microsoft.EntityFrameworkCore;
using TrainComponentManager.Data.Models;

namespace TrainComponentManager.Data.Repositories
{
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Gets the train components.
        /// </summary>
        /// <value>
        /// The train components.
        /// </value>
        public DbSet<TrainComponent> TrainComponents => Set<TrainComponent>();

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// <para>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run. However, it will still run when creating a compiled model.
        /// </para>
        /// <para>
        /// See <see href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</see> for more information and
        /// examples.
        /// </para>
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Убедимся, что UniqueNumber уникален и индексирован для быстрого поиска
            modelBuilder.Entity<TrainComponent>()
                .HasIndex(tc => tc.UniqueNumber)
                .IsUnique(); // Если UniqueNumber должен быть уникальным

            // Индекс для Name, если по нему часто ищут
            modelBuilder.Entity<TrainComponent>()
                .HasIndex(tc => tc.Name);

            // Если вы будете искать по CanAssignQuantity часто (например, чтобы выбрать все, что можно назначить количество)
            modelBuilder.Entity<TrainComponent>()
                .HasIndex(tc => tc.CanAssignQuantity);

            // Опционально: если Name и UniqueNumber являются обязательными
            modelBuilder.Entity<TrainComponent>()
                .Property(tc => tc.Name)
                .IsRequired();

            modelBuilder.Entity<TrainComponent>()
                .Property(tc => tc.UniqueNumber)
                .IsRequired();

            modelBuilder.Entity<TrainComponent>().HasData(
                new TrainComponent { Id = 1, Name = "Engine", UniqueNumber = "ENG123", CanAssignQuantity = false },
                new TrainComponent { Id = 2, Name = "Passenger Car", UniqueNumber = "PAS456", CanAssignQuantity = false },
                new TrainComponent { Id = 3, Name = "Freight Car", UniqueNumber = "FRT789", CanAssignQuantity = false },
                new TrainComponent { Id = 4, Name = "Wheel", UniqueNumber = "WHL101", CanAssignQuantity = true },
                new TrainComponent { Id = 5, Name = "Seat", UniqueNumber = "STS234", CanAssignQuantity = true },
                new TrainComponent { Id = 6, Name = "Window", UniqueNumber = "WIN567", CanAssignQuantity = true },
                new TrainComponent { Id = 7, Name = "Door", UniqueNumber = "DR123", CanAssignQuantity = true },
                new TrainComponent { Id = 8, Name = "Control Panel", UniqueNumber = "CTL987", CanAssignQuantity = true },
                new TrainComponent { Id = 9, Name = "Light", UniqueNumber = "LGT456", CanAssignQuantity = true },
                new TrainComponent { Id = 10, Name = "Brake", UniqueNumber = "BRK789", CanAssignQuantity = true },
                new TrainComponent { Id = 11, Name = "Bolt", UniqueNumber = "BLT321", CanAssignQuantity = true },
                new TrainComponent { Id = 12, Name = "Nut", UniqueNumber = "NUT654", CanAssignQuantity = true },
                new TrainComponent { Id = 13, Name = "Engine Hood", UniqueNumber = "EH789", CanAssignQuantity = false },
                new TrainComponent { Id = 14, Name = "Axle", UniqueNumber = "AX456", CanAssignQuantity = false },
                new TrainComponent { Id = 15, Name = "Piston", UniqueNumber = "PST789", CanAssignQuantity = false },
                new TrainComponent { Id = 16, Name = "Handrail", UniqueNumber = "HND234", CanAssignQuantity = true },
                new TrainComponent { Id = 17, Name = "Step", UniqueNumber = "STP567", CanAssignQuantity = true },
                new TrainComponent { Id = 18, Name = "Roof", UniqueNumber = "RF123", CanAssignQuantity = false },
                new TrainComponent { Id = 19, Name = "Air Conditioner", UniqueNumber = "AC789", CanAssignQuantity = false },
                new TrainComponent { Id = 20, Name = "Flooring", UniqueNumber = "FLR456", CanAssignQuantity = false },
                new TrainComponent { Id = 21, Name = "Mirror", UniqueNumber = "MRR789", CanAssignQuantity = true },
                new TrainComponent { Id = 22, Name = "Horn", UniqueNumber = "HRN321", CanAssignQuantity = false },
                new TrainComponent { Id = 23, Name = "Coupler", UniqueNumber = "CPL654", CanAssignQuantity = false },
                new TrainComponent { Id = 24, Name = "Hinge", UniqueNumber = "HNG987", CanAssignQuantity = true },
                new TrainComponent { Id = 25, Name = "Ladder", UniqueNumber = "LDR456", CanAssignQuantity = true },
                new TrainComponent { Id = 26, Name = "Paint", UniqueNumber = "PNT789", CanAssignQuantity = false },
                new TrainComponent { Id = 27, Name = "Decal", UniqueNumber = "DCL321", CanAssignQuantity = true },
                new TrainComponent { Id = 28, Name = "Gauge", UniqueNumber = "GGS654", CanAssignQuantity = true },
                new TrainComponent { Id = 29, Name = "Battery", UniqueNumber = "BTR987", CanAssignQuantity = false },
                new TrainComponent { Id = 30, Name = "Radiator", UniqueNumber = "RDR456", CanAssignQuantity = false }
            );
        }
    }
}
