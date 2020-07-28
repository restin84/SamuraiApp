using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Data
{
  public class SamuraiContext : DbContext
  {
    public DbSet<Samurai> Samurais { get; set; }

    public DbSet<Quote> Quotes { get; set; }

    public DbSet<Clan> Clans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      optionsBuilder.UseSqlServer(
        "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = SamuraiAppData");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      //here we are specifying that a key for a SamuraiBattle ks the (SamuraiId, BattleId) pair
      modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });
    }
  }
}
