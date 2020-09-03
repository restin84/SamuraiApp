using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
  public partial class VideoQueryingWithRawSQL : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.Sql(
        @"create procedure dbo.SamuraisWhoSaidAWord
                @text varchar(20)
                as
                select Samurais.Id, Samurais.Name, Samurais.ClanId
                from   Samurais inner join
                       Quotes on Samurais.Id = Quotes.SamuraiId
                where  (Quotes.Text like '%'+@text+'%')");
      migrationBuilder.Sql(
        @"create procedure dbo.DeleteQuotesForSamurai
            @samuraiId int
            as
            delete from Quotes
            where Quotes.SamuraiId=@samuraiId");
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.Sql(@"drop procedure dbo.SamuraisWhoSaidAWord");
      migrationBuilder.Sql(@"drom procedure dbo.DeleteQuotesForSamurai");
    }
  }
}
