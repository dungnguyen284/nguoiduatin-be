using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NDT.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class frontpage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FrontPageNews",
                columns: table => new
                {
                    SlotNumber = table.Column<int>(type: "int", nullable: false),
                    NewsId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrontPageNews", x => x.SlotNumber);
                    table.CheckConstraint("CK_FrontpageSlot_SlotNumber", "[SlotNumber] >= 1 AND [SlotNumber] <= 4");
                    table.ForeignKey(
                        name: "FK_FrontPageNews_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FrontPageNews_NewsId",
                table: "FrontPageNews",
                column: "NewsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FrontPageNews");
        }
    }
}
