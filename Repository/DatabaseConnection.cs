using Microsoft.EntityFrameworkCore;
using RecUber.Model;

namespace RecUber.Repository
{
    public sealed class DatabaseConnection : DbContext
    {
        public DbSet<Entry>? Entries { get; set; }
        public DbSet<Egress>? Egresses { get; set; }
        
        public DatabaseConnection(DbContextOptions options) : base(options) { Database.EnsureCreated(); }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entry>(entity =>
            {
                entity.ToTable("Entry");

                entity.HasKey(pk => pk.EntryId);

                //entity.Property(prop => prop.Type).IsRequired();

                entity.Property(prop => prop.Details).IsRequired();

                entity.Property(prop => prop.Date).IsRequired();

                entity.Property(prop => prop.Duration).IsRequired();

                entity.Property(prop => prop.Distance).IsRequired();

                entity.Property(prop => prop.TotalValue).IsRequired();
            });

            modelBuilder.Entity<Egress>(entity =>
            {
                entity.ToTable("Egress");

                entity.HasKey(pk => pk.EgressId);

                //entity.Property(prop => prop.Type).IsRequired();

                entity.Property(prop => prop.Details).IsRequired();

                entity.Property(prop => prop.Date).IsRequired();

                entity.Property(prop => prop.TotalValue).IsRequired();
            });
        }
    }
}
