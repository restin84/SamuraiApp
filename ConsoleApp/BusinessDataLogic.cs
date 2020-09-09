using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
  public class BusinessDataLogic
  {
    private SamuraiContext context;

    public BusinessDataLogic(SamuraiContext context) {
      this.context = context;
    }

    public BusinessDataLogic() {
      context = new SamuraiContext();
    }

    public int AddMultipleSamurais(string[] nameList) {
      var samuraiList = new List<Samurai>();
      foreach (var name in nameList) {
        samuraiList.Add(new Samurai { Name = name });
      }

      context.Samurais.AddRange(samuraiList);

      var dbResult = context.SaveChanges();
      return dbResult;
    }
    public int InsertNewSamurai(Samurai samurai) {
      context.Samurais.Add(samurai);
      var dbResult = context.SaveChanges();
      return dbResult;
    }
  }
}
