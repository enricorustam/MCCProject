using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP.NetCoreProject.Migrations
{
    public partial class updatemodelemployee2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NIP",
                table: "TB_M_Employee",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NIP",
                table: "TB_M_Employee");
        }
    }
}
