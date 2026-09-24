using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuaProject.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class AddTryggerType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TriggerType",
                table: "Rules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggerType",
                table: "Rules");
        }
    }
}
