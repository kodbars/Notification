using Microsoft.EntityFrameworkCore;

namespace NotificationsDb.Data
{

    public partial class NotificationsDbContext : DbContext
    {
        public NotificationsDbContext()
        {
        }

        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Notifica__3213E83F45CAE809");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Message).HasColumnName("message");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
