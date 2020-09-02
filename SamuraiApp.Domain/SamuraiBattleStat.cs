using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
  //The SamuraiBattleStat entity is an entity that we can map to
  //the SamuraiBattleStats view
  public partial class SamuraiBattleStat
  {
    public string Name { get; set; }
    public int? NumberOfBattles { get; set; }
    public string EarliestBattle { get; set; }
  }
}
