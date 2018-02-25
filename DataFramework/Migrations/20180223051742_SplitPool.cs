using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DataFramework.Migrations
{
    public partial class SplitPool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvantageOutcomes",
                table: "Pool");

            migrationBuilder.DropColumn(
                name: "DespairOutcomes",
                table: "Pool");

            migrationBuilder.DropColumn(
                name: "FailureOutcomes",
                table: "Pool");

            migrationBuilder.DropColumn(
                name: "NeutralOutcomes",
                table: "Pool");

            migrationBuilder.DropColumn(
                name: "SuccessOutcomes",
                table: "Pool");

            migrationBuilder.DropColumn(
                name: "ThreatOutcomes",
                table: "Pool");

            migrationBuilder.DropColumn(
                name: "TriumphOutcomes",
                table: "Pool");

            migrationBuilder.CreateTable(
                name: "DieFace",
                columns: table => new
                {
                    DieFaceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DieId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieFace", x => x.DieFaceId);
                    table.ForeignKey(
                        name: "FK_DieFace_Die_DieId",
                        column: x => x.DieId,
                        principalTable: "Die",
                        principalColumn: "DieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoolCombination",
                columns: table => new
                {
                    PositivePoolId = table.Column<long>(nullable: false),
                    NegativePoolId = table.Column<long>(nullable: false),
                    AdvantageOutcomes = table.Column<long>(nullable: false),
                    DespairOutcomes = table.Column<long>(nullable: false),
                    FailureOutcomes = table.Column<long>(nullable: false),
                    NeutralOutcomes = table.Column<long>(nullable: false),
                    SuccessOutcomes = table.Column<long>(nullable: false),
                    ThreatOutcomes = table.Column<long>(nullable: false),
                    TotalOutcomes = table.Column<long>(nullable: false),
                    TriumphOutcomes = table.Column<long>(nullable: false),
                    UniqueOutcomes = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolCombination", x => new { x.PositivePoolId, x.NegativePoolId });
                    table.ForeignKey(
                        name: "FK_PoolCombination_Pool_NegativePoolId",
                        column: x => x.NegativePoolId,
                        principalTable: "Pool",
                        principalColumn: "PoolId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PoolCombination_Pool_PositivePoolId",
                        column: x => x.PositivePoolId,
                        principalTable: "Pool",
                        principalColumn: "PoolId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DieFaceSymbol",
                columns: table => new
                {
                    DieFaceId = table.Column<int>(nullable: false),
                    Symbol = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieFaceSymbol", x => new { x.DieFaceId, x.Symbol });
                    table.ForeignKey(
                        name: "FK_DieFaceSymbol_DieFace_DieFaceId",
                        column: x => x.DieFaceId,
                        principalTable: "DieFace",
                        principalColumn: "DieFaceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DieFace_DieId",
                table: "DieFace",
                column: "DieId");

            migrationBuilder.CreateIndex(
                name: "IX_PoolCombination_NegativePoolId",
                table: "PoolCombination",
                column: "NegativePoolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DieFaceSymbol");

            migrationBuilder.DropTable(
                name: "PoolCombination");

            migrationBuilder.DropTable(
                name: "DieFace");

            migrationBuilder.AddColumn<long>(
                name: "AdvantageOutcomes",
                table: "Pool",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DespairOutcomes",
                table: "Pool",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FailureOutcomes",
                table: "Pool",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "NeutralOutcomes",
                table: "Pool",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SuccessOutcomes",
                table: "Pool",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ThreatOutcomes",
                table: "Pool",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TriumphOutcomes",
                table: "Pool",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
