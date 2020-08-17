using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP.NetCoreProject.Migrations
{
    public partial class updatemodelsupervisor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "TB_M_Supervisor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "TB_M_Supervisor");
        }
    }
}
