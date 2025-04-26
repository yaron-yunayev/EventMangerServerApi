using EventMangerServerApi.Core.Modles;
using Microsoft.EntityFrameworkCore;

namespace EventMangerServerApi.Infrastructure.Data
{
    public class EventProjectDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<UserFavoriteSupplier> UserFavoriteSuppliers { get; set; }

        public EventProjectDbContext(DbContextOptions<EventProjectDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // קשר many-to-many בין אירועים לספקים
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Suppliers)
                .WithMany(s => s.Events)
                .UsingEntity(j => j.ToTable("EventSupplier"));

            // אירוע קשור למנהלו (User) עם ForeignKey ומניעת מחיקה צלבית
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Manager)
                .WithMany(u => u.ManagedEvents)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // מיפוי טבלת UserFavoriteSupplier (הרשאות לבחירת ספקים)
            modelBuilder.Entity<UserFavoriteSupplier>()
                .HasKey(uf => new { uf.UserId, uf.SupplierId });

            modelBuilder.Entity<UserFavoriteSupplier>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.FavoriteSuppliers)
                .HasForeignKey(uf => uf.UserId);

            modelBuilder.Entity<UserFavoriteSupplier>()
                .HasOne(uf => uf.Supplier)
                .WithMany(s => s.FavoritedByUsers)
                .HasForeignKey(uf => uf.SupplierId);
        }
    }
}
