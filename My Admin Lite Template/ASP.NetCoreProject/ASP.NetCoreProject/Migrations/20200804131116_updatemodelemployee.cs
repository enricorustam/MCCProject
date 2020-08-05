using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP.NetCoreProject.Migrations
{
    public partial class updatemodelemployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "employeeId",
                table: "TB_M_Form",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TB_M_Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Employee", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Form_employeeId",
                table: "TB_M_Form",
                column: "employeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_Form_TB_M_Employee_employeeId",
                table: "TB_M_Form",
                column: "employeeId",
                principalTable: "TB_M_Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_Form_TB_M_Employee_employeeId",
                table: "TB_M_Form");

            migrationBuilder.DropTable(
                name: "TB_M_Employee");

            migrationBuilder.DropIndex(
                name: "IX_TB_M_Form_employeeId",
                table: "TB_M_Form");

            migrationBuilder.DropColumn(
                name: "employeeId",
                table: "TB_M_Form");
        }
    }
}
