using Arena.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Arena.Infrastructure;

public class ArenaDbContext : DbContext
{
    public DbSet<CreatureDbModel> Creatures => Set<CreatureDbModel>();
    public DbSet<BattleDbModel> Battles => Set<BattleDbModel>();
    public DbSet<BattleLogDbModel> BattleLogs => Set<BattleLogDbModel>();

    public ArenaDbContext(DbContextOptions<ArenaDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BattleDbModel>(entity =>
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
