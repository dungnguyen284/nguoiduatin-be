using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NDT.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class stockandwatchlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WatchLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatchLists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WatchListStocks",
                columns: table => new
                {
                    StocksId = table.Column<int>(type: "int", nullable: false),
                    WatchListsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchListStocks", x => new { x.StocksId, x.WatchListsId });
                    table.ForeignKey(
                        name: "FK_WatchListStocks_Stocks_StocksId",
                        column: x => x.StocksId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchListStocks_WatchLists_WatchListsId",
                        column: x => x.WatchListsId,
                        principalTable: "WatchLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchLists_UserId",
                table: "WatchLists",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WatchListStocks_WatchListsId",
                table: "WatchListStocks",
                column: "WatchListsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchListStocks");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "WatchLists");
        }
    }
}
