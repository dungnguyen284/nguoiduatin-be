using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NDT.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixfknews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "News");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "News",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "News");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "News",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
