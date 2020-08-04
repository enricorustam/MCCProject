using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP.NetCoreProject.Migrations
{
    public partial class addmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_M_Department",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Supervisor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Supervisor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Form",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    supervisorId = table.Column<int>(nullable: true),
                    departmentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Form", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_M_Form_TB_M_Department_departmentId",
                        column: x => x.departmentId,
                        principalTable: "TB_M_Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_M_Form_TB_M_Supervisor_supervisorId",
                        column: x => x.supervisorId,
                        principalTable: "TB_M_Supervisor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Validation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Action = table.Column<string>(nullable: true),
                    supervisorId = table.Column<int>(nullable: true),
                    formId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Validation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_M_Validation_TB_M_Form_formId",
                        column: x => x.formId,
                        principalTable: "TB_M_Form",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_M_Validation_TB_M_Supervisor_supervisorId",
                        column: x => x.supervisorId,
                        principalTable: "TB_M_Supervisor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Report",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    validationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_M_Report_TB_M_Validation_validationId",
                        column: x => x.validationId,
                        principalTable: "TB_M_Validation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Form_departmentId",
                table: "TB_M_Form",
                column: "departmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Form_supervisorId",
                table: "TB_M_Form",
                column: "supervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Report_validationId",
                table: "TB_M_Report",
                column: "validationId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Validation_formId",
                table: "TB_M_Validation",
                column: "formId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Validation_supervisorId",
                table: "TB_M_Validation",
                column: "supervisorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_M_Report");

            migrationBuilder.DropTable(
                name: "TB_M_Validation");

            migrationBuilder.DropTable(
                name: "TB_M_Form");

            migrationBuilder.DropTable(
                name: "TB_M_Department");

            migrationBuilder.DropTable(
                name: "TB_M_Supervisor");
        }
    }
}
