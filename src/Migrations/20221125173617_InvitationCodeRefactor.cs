using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksAPI.Migrations
{
    public partial class InvitationCodeRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvitationCode",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitationCode",
                table: "Groups");
        }
    }
}
