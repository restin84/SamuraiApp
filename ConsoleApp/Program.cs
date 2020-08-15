using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
      //QueryFilters();
      //RetrieveAndUpdateSamurai();
      //RetrieveAndUpdateMultipleSamurais();
      //MultipleDatabaseOperations();
      //RetrieveAndDeleteASamurai();
      InsertBattle();
      Console.Write("Press any key...");
      Console.ReadKey();
    }

    private static void InsertMultipleSamurais() {
      //Bulk insert happens at 4 inserts
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
      //var samurais = context.Samurais.Where(
      //  s => s.Name == "Doug").ToList();
      //var samurais2 = (from s in context.Samurais
      //                 where s.Name == "Doug"
      //                 select s).ToList();
      //var name = "Doug";
      //var samurais3 = (from s in context.Samurais
      //                 where s.Name == name
      //                 select s).ToList();
      //var samurais4 = context.Samurais.Where(s =>
      //  EF.Functions.Like(s.Name, "D%")).ToList();
      var name = "Doug";
      var samurais = context.Samurais
        .FirstOrDefault(s => s.Name == name);
      //Find is a DBSet method that will execute right away
      //If the object with the key as param is already in 
      //memory and being tracked by the context then no query
      //is needed
      var samuraiWithId = context.Samurais.Find(2);
      //Last() or LastOrDefault() methods will only work 
      //if you first sort the query using the OrderBy method
      var lastSamurai = context.Samurais.OrderBy(s => s.Id)
        .LastOrDefault(s => s.Name == name);
    }

    private static void RetrieveAndUpdateSamurai()
    {
      var samurai = context.Samurais.FirstOrDefault();
      samurai.Name += "San";
      context.SaveChanges();
    }

    private static void RetrieveAndUpdateMultipleSamurais()
    {
      //Skip() and Take() methods are good for paging
      var samurais = context.Samurais.Skip(4).Take(4).ToList();
      samurais.ForEach(s => s.Name += "San");
      context.SaveChanges();
    }

    private static void MultipleDatabaseOperations()
    {
      var samurai = context.Samurais.FirstOrDefault();
      samurai.Name += "San";
      context.Samurais.Add(new Samurai() { Name = "Kikuchiyo" });
      context.SaveChanges();
    }

    private static void RetrieveAndDeleteASamurai() {
      var samurai = context.Samurais.Find(18);
      context.Samurais.Remove(samurai);
      context.SaveChanges();
    }

    private static void InsertBattle() {
      context.Battles.Add(new Battle() {
        Name = "Battle of Okehazam",
        StartDate = new DateTime(1560, 05, 01),
        EndDate = new DateTime(1560, 06, 15)
      });
      context.SaveChanges();
    }

    //In a disconnected scenario each method to retrieve or update data
    //will instantiate its own DBContext instance and then dispose it when
    //its finished
    /*
     * Julie Lerman has a course: Entity Framework in the Enterprise
     * that has a module about patterns for handling disconnected 
     * scenarios
     */
    private static void QueryAndUpdateBattle_Disconnected() {
      var battle = context.Battles.AsNoTracking().FirstOrDefault();
      battle.EndDate = new DateTime(1560, 06, 30);
      using (var newContextInstance = new SamuraiContext()) {
        //Use Update() to mark the object as modified
        //Now the new context is tracking this Battle object
        newContextInstance.Battles.Update(battle);
        newContextInstance.SaveChanges();
      }
    }
  }
}
