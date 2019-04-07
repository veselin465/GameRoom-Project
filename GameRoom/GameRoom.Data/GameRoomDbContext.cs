using Microsoft.EntityFrameworkCore;
using GameRoom.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoom.Data
{
    public class GameRoomDbContext : DbContext
    {

        public GameRoomDbContext(DbContextOptions<GameRoomDbContext> options)
            :base(options)
        {

        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Scoreboard> Scoreboards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationData.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Scoreboard>().HasKey(s => new { s.PlayerId, s.GameId });
            modelBuilder.Entity<Scoreboard>()
                .HasOne(s => s.Player)
                .WithMany(p => p.Scoreboard);
            modelBuilder.Entity<Scoreboard>()
                .HasOne(s => s.Game)
                .WithMany(g => g.Scoreboard);
            base.OnModelCreating(modelBuilder);
        }
    }
}
