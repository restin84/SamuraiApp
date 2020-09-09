using ConsoleApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiAppTests
{
  [TestClass]
  public class BizDataLogicTests
  {
    [TestMethod]
    public void AddMultipleSamuraisReturnsCorrectNumberOfInsertedRows() {
      var builder = new DbContextOptionsBuilder();
      builder.UseInMemoryDatabase("AddMultipleSamurais");
      using (var context = new SamuraiContext(builder.Options)) {
        var bizLogic = new BusinessDataLogic(context);
        var nameList = new string[] { "Kikuchiyo", "Kyuzo", "Rikchi" };
        var result = bizLogic.AddMultipleSamurais(nameList);
        Assert.AreEqual(nameList.Count(), result);
      }
    }

    [TestMethod]
    public void CanInsertSingleSamurai() {
      var builder = new DbContextOptionsBuilder();
      builder.UseInMemoryDatabase("InsertNewSamurai");

      using (var context = new SamuraiContext(builder.Options)) {
        var bizlogic = new BusinessDataLogic(context);
        bizlogic.InsertNewSamurai(new Samurai());
      }

      using (var context2 = new SamuraiContext(builder.Options)) {
        //Since we use the same DB for two distinct contexts the count 
        //of the Samurais DbSet (or the number of Samurais in the DB)
        //should be the same in each context after insert
        Assert.AreEqual(1, context2.Samurais.Count());
      }
    }

    //TODO: Review some of the other tests in the course materials (not 
    //covered in the videos
  }
}
