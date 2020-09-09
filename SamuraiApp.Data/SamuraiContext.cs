using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
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

    public DbSet<Battle> Battles { get; set; }
    public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

    public SamuraiContext() {

    }

    public SamuraiContext(DbContextOptions options) : base(options) {

    }

    public static readonly ILoggerFactory ConsoleLoggerFactory
      = LoggerFactory.Create(builder => {
        builder
          .AddFilter((category, level) =>
              category == DbLoggerCategory.Database.Command.Name
              && level == LogLevel.Information)
          .AddConsole(); //in the Microsoft.Extensions.Logging.Console package
          
      });
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      if (!optionsBuilder.IsConfigured) {
        optionsBuilder
           .UseSqlServer(
             "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = SamuraiTestData"); 
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      //here we are specifying that a key for a SamuraiBattle ks the (SamuraiId, BattleId) pair
      modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });
      //here we are telling the Model Builder that the Horse entity should map to a table called
      //horses
      modelBuilder.Entity<Horse>().ToTable("Horses");
      //here we are letting the context know that the SamuraiBattleStat entity has no key
      //Use the ToView() method so that EF Core does not try to create a DB Table for
      //the SamuraiBattleStat
      //Since we used HasNoKey EF Core will never track the SamuraiBattleStat entity
      modelBuilder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuraiBattleStats");
    }
  }
}
