using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace ConsoleApp
{
  class Program
  {
    private static SamuraiContext context = new SamuraiContext();
    static void Main(string[] args) {
      //context.Database.EnsureCreated();
      //GetSamurais("Before Add:");
      //AddSamurai();
      //GetSamurais("After Add:");
      //InsertMultipleSamurais();
      //InsertVariousTypes();
      QueryFilters();
      Console.Write("Press any key...");
      Console.ReadKey();
    }

    private static void InsertMultipleSamurais() {
      var samurai = new Samurai { Name = "Doug" };
      var samurai2 = new Samurai { Name = "Everest" };
      var samurai3 = new Samurai { Name = "Number 3" };
      var samurai4 = new Samurai { Name = "Number 4" };
      context.Samurais.AddRange(samurai, samurai2, samurai3, samurai4);
      context.SaveChanges();
    }

    private static void InsertVariousTypes() {
      var samurai = new Samurai { Name = "Kikuchio" };
      var clan = new Clan { ClanName = "Imperial Clan" };
      context.AddRange(samurai, clan);
      context.SaveChanges();
    }

    private static void AddSamurai() {
      var samurai = new Samurai { Name = "Doug" };
      context.Samurais.Add(samurai);
      context.SaveChanges();
    }

    private static void GetSamurais(string text) {
      var samurais = context.Samurais.ToList();
      Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
      foreach(var samurai in samurais) {
        Console.WriteLine(samurai.Name);
      }
    }

    public static void QueryFilters() {
      var samurais = context.Samurais.Where(
        s => s.Name == "Doug").ToList();
      var samurais2 = (from s in context.Samurais
                       where s.Name == "Doug"
                       select s).ToList();
      var name = "Doug";
      var samurais3 = (from s in context.Samurais
                       where s.Name == name
                       select s).ToList();
      var samurais4 = context.Samurais.Where(s =>
        EF.Functions.Like(s.Name, "D%")).ToList();
    }
  }
}
