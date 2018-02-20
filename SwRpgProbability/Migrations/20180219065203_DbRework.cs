using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SwRpgProbability.Migrations
{
    public partial class DbRework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Die",
                columns: table => new
                {
                    DieId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Die", x => x.DieId);
                });

            migrationBuilder.CreateTable(
                name: "Pool",
                columns: table => new
                {
                    PoolId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pool", x => x.PoolId);
                });

            migrationBuilder.CreateTable(
                name: "PoolDie",
                columns: table => new
                {
                    PoolId = table.Column<long>(nullable: false),
                    DieId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolDie", x => new { x.PoolId, x.DieId });
                    table.ForeignKey(
                        name: "FK_PoolDie_Die_DieId",
                        column: x => x.DieId,
                        principalTable: "Die",
                        principalColumn: "DieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PoolDie_Pool_PoolId",
                        column: x => x.PoolId,
                        principalTable: "Pool",
                        principalColumn: "PoolId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoolResult",
                columns: table => new
                {
                    PoolResultId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PoolId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolResult", x => x.PoolResultId);
                    table.ForeignKey(
                        name: "FK_PoolResult_Pool_PoolId",
                        column: x => x.PoolId,
                        principalTable: "Pool",
                        principalColumn: "PoolId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoolResultSymbol",
                columns: table => new
                {
                    PoolResultId = table.Column<long>(nullable: false),
                    Symbol = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolResultSymbol", x => new { x.PoolResultId, x.Symbol });
                    table.ForeignKey(
                        name: "FK_PoolResultSymbol_PoolResult_PoolResultId",
                        column: x => x.PoolResultId,
                        principalTable: "PoolResult",
                        principalColumn: "PoolResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PoolDie_DieId",
                table: "PoolDie",
                column: "DieId");

            migrationBuilder.CreateIndex(
                name: "IX_PoolResult_PoolId",
                table: "PoolResult",
                column: "PoolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PoolDie");

            migrationBuilder.DropTable(
                name: "PoolResultSymbol");

            migrationBuilder.DropTable(
                name: "Die");

            migrationBuilder.DropTable(
                name: "PoolResult");

            migrationBuilder.DropTable(
                name: "Pool");
        }
    }
}
