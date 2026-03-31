using Arena.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arena.DataAccess;

public class ArenaDbContext : DbContext
{
    public DbSet<CreatureEntity> Creatures => Set<CreatureEntity>();
    public DbSet<BattleEntity> Battles => Set<BattleEntity>();
    public DbSet<BattleLogEntity> BattleLogs => Set<BattleLogEntity>();

    public ArenaDbContext(DbContextOptions<ArenaDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BattleEntity>(entity =>
        {
            entity.HasOne(b => b.Creature1)
                .WithMany()
                .HasForeignKey(b => b.Creature1Id)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Creature2)
                .WithMany()
                .HasForeignKey(b => b.Creature2Id)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Winner)
                .WithMany()
                .HasForeignKey(b => b.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(b => b.Logs)
                .WithOne(l => l.Battle)
                .HasForeignKey(l => l.BattleId);
        });
    }
}
