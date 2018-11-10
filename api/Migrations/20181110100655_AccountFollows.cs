using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class AccountFollows : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountFollows",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false),
                    FollowingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountFollows", x => new { x.AccountId, x.FollowingId });
                    table.ForeignKey(
                        name: "FK_AccountFollows_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountFollows_Accounts_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountFollows_FollowingId",
                table: "AccountFollows",
                column: "FollowingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountFollows");
        }
    }
}
