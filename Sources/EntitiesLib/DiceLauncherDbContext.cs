using Microsoft.EntityFrameworkCore;

namespace EntitiesLib
{
    /// <summary>
    /// Contexte de l'application DiceLauncher utilisant une connection à une base de données SQLite
    /// </summary>
    public class DiceLauncherDbContext : DbContext
    {
        public DbSet<GameEntity> Games { get; set; }
        public DbSet<DiceEntity> Dices { get; set; }
        public DbSet<DiceSideEntity> Sides { get; set; }

        public DiceLauncherDbContext() { }
        public DiceLauncherDbContext(DbContextOptions<DiceLauncherDbContext> options)
            :base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite("Data Source=DiceLauncher.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiceTypeEntity>()
                .HasKey(d => new { d.Dice_FK, d.Game_FK });
            modelBuilder.Entity<DiceSideTypeEntity>()
                .HasKey(d => new { d.Side_FK, d.Dice_FK });
        }

    }
}
