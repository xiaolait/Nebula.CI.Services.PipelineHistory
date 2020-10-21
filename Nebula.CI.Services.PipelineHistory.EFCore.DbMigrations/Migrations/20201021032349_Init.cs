using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nebula.CI.Services.PipelineHistory.EFCore.DbMigrations.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PipelineHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    No = table.Column<int>(nullable: false),
                    Diagram = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    CompletionTime = table.Column<DateTime>(nullable: true),
                    Percent = table.Column<int>(nullable: false),
                    PipelineName = table.Column<string>(nullable: true),
                    PipelineId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PipelineHistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PipelineHistory");
        }
    }
}
