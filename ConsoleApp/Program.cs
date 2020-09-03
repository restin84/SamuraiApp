using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ConsoleApp
{
  class Program {
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
      //InsertBattle();
      //QueryAndUpdateBattleDisconnected();
      //InsertNewSamuraiWithAQuote();
      //InsertNewSamuraiWithManyQuotes();
      //AddQuoteToExistingSamuraiWhileTracked();
      //AddQuoteToExistingSamuraiNotTracked(19);
      //AddQuoteToExistingSamuraiNotTrackedEasy(2);
      //EagerLoadSamuraiWithQuotes();
      //ProjectSomeProperties();
      //FilteringWithRelatedData();
      //ModifyingRelatedDataWhenTracked();
      //ModifyingRelatedDataWhenNotTracked();
      //JoinBattleAndSamurai();
      //EnlistSamuraiIntoBattle();
      //RemoveJoinBetweenSamuraiAndBattleSimple();
      //AddNewSamuraiWithHorse();
      //AddNewSamuraiToHorseUsingId();
      //AddNewHorseSamuraiToObject();
      //AddNewHorseToDisconnectedSamuraiObject();
      //ReplaceHorse();
      //GetSamuraiWithClan();
      //GetClanWithSamurais();
      //QuerySamuraiBattleStats();
      //QueryUsingRawSql();
      QueryUsingRawSqlWithInterpolation();
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
      foreach (var samurai in samurais) {
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

    private static void RetrieveAndUpdateSamurai() {
      var samurai = context.Samurais.FirstOrDefault();
      samurai.Name += "San";
      context.SaveChanges();
    }

    private static void RetrieveAndUpdateMultipleSamurais() {
      //Skip() and Take() methods are good for paging
      var samurais = context.Samurais.Skip(4).Take(4).ToList();
      samurais.ForEach(s => s.Name += "San");
      context.SaveChanges();
    }

    private static void MultipleDatabaseOperations() {
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
    private static void QueryAndUpdateBattleDisconnected() {
      //AsNoTracking() will specify that this query should not 
      //bother tracking its results. The DbContext will NOT 
      //create Entity Entry objects to track the results of the 
      //query
      var battle = context.Battles.AsNoTracking().FirstOrDefault();
      battle.EndDate = new DateTime(1560, 06, 30);
      using (var newContextInstance = new SamuraiContext()) {
        //Use Update() to mark the object as modified
        //Now the new context is tracking this Battle object
        newContextInstance.Battles.Update(battle);
        newContextInstance.SaveChanges();
      }

    }
    private static void InsertNewSamuraiWithAQuote() {
      var samurai = new Samurai {
        Name = "Kambei Shimada",
        Quotes = new List<Quote>() {
          new Quote() {Text = "I've come to save you" }
        }
      };
      context.Samurais.Add(samurai);
      context.SaveChanges();
    }

    private static void InsertNewSamuraiWithManyQuotes() {
      var samurai = new Samurai {
        Name = "Kyuzo",
        Quotes = new List<Quote> {
          new Quote {Text = "Watch out for my sharp sword!" },
          new Quote { Text = "I told you to watch out for my sharp sword! Oh well!"}
        }
      };
      context.Samurais.Add(samurai);
      context.SaveChanges();
    }

    private static void AddQuoteToExistingSamuraiWhileTracked() {
      var samurai = context.Samurais.FirstOrDefault();
      samurai.Quotes.Add(new Quote {
        Text = "I bet you're happy that I've saved you!"
      });
      context.SaveChanges();
    }

    private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId) {
      var samurai = context.Samurais.Find(samuraiId);
      //adding a quote with only the text property set but NO Id prop set
      samurai.Quotes.Add(new Quote {
        Text = "Now that I have saved you, will you feed me dinner?"
      });
      using (var newContext = new SamuraiContext()) {
        //EFCs default behavior assumes that the Quote's FK to a Samurai
        //must be the key of the parent (samurai var)
        //newContext.Samurais.Update(samurai);//This results in an update call
        newContext.Samurais.Attach(samurai); //This will NOT call update
        newContext.SaveChanges();
      }
    }

    private static void AddQuoteToExistingSamuraiNotTrackedEasy(int samuraiId) {
      var quote = new Quote {
        Text = "Now that I've saved you, will you feed me dinner again?",
        SamuraiId = samuraiId
      };
      using (var context = new SamuraiContext()) {
        context.Quotes.Add(quote);
        context.SaveChanges();
      }
    }

    private static void EagerLoadSamuraiWithQuotes() {
      //This will perform eager loading on the Samurai DbSet's Quotes nav prop
      var samuraiWithQuotes = context.Samurais.Include(s => s.Quotes).ToList();
    }

    private static void ProjectSomeProperties() {
      //var someProperties = context.Samurais
      //  .Select(s => new { s.Id, s.Name, s.Quotes.Count })
      //  .ToList();

      //Here we are filtering the related data that is returned in the 
      //projectd type
      //Important: the anonymous types do not result in tracking of entity objects
      //var somePropertiesWithoutQuotes = context.Samurais
      //  .Select(s => new { s.Id, s.Name, HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy")) })
      //  .ToList();

      //Projecting full entity objects while filtering the related objects that
      //are also returned
      var samuraisWithHappyQuotes = context.Samurais
        .Select(s => new {
          Samurai = s,
          HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
        })
        .ToList();
      var firstSamurai = samuraisWithHappyQuotes[0].Samurai.Name += " The Happiest";
    }

    private static void FilteringWithRelatedData() {
      //We don't care if Quotes are tracked but we can use them in 
      //the query
      var samurais = context.Samurais
        .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
        .ToList();
    }

    private static void ModifyingRelatedDataWhenTracked() {
      //Eager load the Quotes when getting the Samurai
      var samurai = context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Id == 2);
      //Since the quotes were loaded we can modify them here
      samurai.Quotes[0].Text = " Did you hear that?";
      context.Quotes.Remove(samurai.Quotes[2]);
      context.SaveChanges();
    }

    private static void ModifyingRelatedDataWhenNotTracked() {
      var samurai = context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Id == 2);
      var quote = samurai.Quotes[0];
      quote.Text += " Did you hear that again?";
      using (var newContext = new SamuraiContext()) {
        //newContext.Quotes.Update(quote); //This will update ALL the quotes of the Samurai
        //We can simply use the Entry() method to change the State of the modified
        //Quote so the EF Core knows to update ONLY that entry
        newContext.Entry(quote).State = EntityState.Modified;
        newContext.SaveChanges();
      }
    }

    /*
     * Look at "Entity Framework Core 2: Mappings" for "Mapping and Interacting 
     * with Many-to-many Relationships" module to see this how to do this stuff in disconnected
     * scenarios
     */
    private static void JoinBattleAndSamurai() {
      //Samurai and Battle already exist and have their Ids
      var sbJoin = new SamuraiBattle { SamuraiId = 1, BattleId = 2 };
      //Note that there is no SamuraiBattles DbSet but we can add this SamuraiBattle
      //directly to the DbContext
      context.Add(sbJoin);
      context.SaveChanges();
    }

    private static void EnlistSamuraiIntoBattle() {
      var battle = context.Battles.Find(1);
      //Here we are just adding a SamuraiBattle via the Battle that is 
      //in memory. EF Core will figure out the Id of the Battle (notice
      //we do not provide it below)
      battle.SamuraiBattles
        .Add(new SamuraiBattle { SamuraiId = 21 });
      context.SaveChanges();
    }

    private static void RemoveJoinBetweenSamuraiAndBattleSimple() {
      var join = new SamuraiBattle { BattleId = 1, SamuraiId = 2 };
      //here we are just instructing EF Core to delete the SamuraiBattle
      //where BattleId = 1 and SamuraiId = 2
      context.Remove(join);
      context.SaveChanges();
    }

    private static void AddNewSamuraiWithHorse() {
      var samurai = new Samurai { Name = "Juna Ujichika" };
      samurai.Horse = new Horse { Name = "Silver" };
      context.Samurais.Add(samurai);
      context.SaveChanges();
    }

    private static void AddNewSamuraiToHorseUsingId() {
      var horse = new Horse { Name = "Scout", SamuraiId = 2 };
      context.Add(horse);
      context.SaveChanges();
    }

    private static void AddNewHorseSamuraiToObject() {
      var samurai = context.Samurais.Find(22);
      samurai.Horse = new Horse { Name = "Black Beauty" };
      context.SaveChanges();
    }

    private static void AddNewHorseToDisconnectedSamuraiObject() {
      var samurai = context.Samurais.AsNoTracking().FirstOrDefault(s => s.Id == 23);
      samurai.Horse = new Horse { Name = "Mr. Ed" };
      using (var newContext = new SamuraiContext()) {
        newContext.Attach(samurai);
        newContext.SaveChanges();
      }
    }

    private static void ReplaceHorse() {
      //var samurai = context.Samurais.Include(s => s.Horse).FirstOrDefault(s => s.Id == 23);
      var samurai = context.Samurais.Find(23); //has a horse but the next line
      //will throw an error because it has not been loaded into memory
      samurai.Horse = new Horse { Name = "Trigger" };
      context.SaveChanges();
    }

    private static void GetSamuraiWithClan() {
      var samurai = context.Samurais.Include(s => s.Clan).FirstOrDefault();
    }

    private static void GetClanWithSamurais() {
      //there is no Samurais nav prop on Clans and there is no ClanId in 
      //Samurai that points back to a Clan. So you need to do stuff 
      //like this when querying the 1-many Clan-Samurai relationship
      var clan = context.Clans.Find(3);
      var samuraisForClan = context.Samurais.Where(s => s.Clan.Id == 3).ToList();
    }

    private static void QuerySamuraiBattleStats() {
      var stats = context.SamuraiBattleStats.ToList();

      //this throws an exception because a SamuraiBattleStat has no key
      var findone = context.SamuraiBattleStats.Find(2);
    }

    private static void QueryUsingRawSql() {
      //The query here must return data for all properties of the entity type or 
      //EF Core will throw an exception
      //Since the FromSqlRaw* methods are DbSet methods the SQL supplied can only
      //be used for known entities. In other words you can't use the raw sql methods
      //to return a type that is not known by the context
      //You CANNOT select Nav props in raw sql methods
      var samurais = context.Samurais.FromSqlRaw("select * from Samurais").ToList();
      //Get the quotes along with the Samurais
      samurais = context.Samurais.FromSqlRaw(
        "select Id, Name, ClanId from Samurais").Include(s => s.Quotes).ToList();
    }

    private static void QueryUsingRawSqlWithInterpolation() {
      /*
       * IF USING INTERPOLATED STRINGS FOR THE SQL MAKE SURE TO CALL THE
       * FromSqlInterpolated TO AVOID THE POSSIBILITY OF SQL INJECTION
       * ATTACK!!!
       */
      string name = "Doug";
      var samurais = context.Samurais
        .FromSqlInterpolated($"select * from Samurais where name = {name}")
        .ToList();

    }
  }
}
