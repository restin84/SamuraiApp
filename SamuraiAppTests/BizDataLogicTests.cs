using ConsoleApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
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
  }
}
