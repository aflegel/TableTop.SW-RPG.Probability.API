using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DataFramework.Migrations
{
    public partial class PoolCombinationCleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalOutcomes",
                table: "PoolCombination");

            migrationBuilder.DropColumn(
                name: "UniqueOutcomes",
                table: "PoolCombination");

            migrationBuilder.AddColumn<long>(
                name: "Quantity",
                table: "PoolResult",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PoolResult");

            migrationBuilder.AddColumn<long>(
                name: "TotalOutcomes",
                table: "PoolCombination",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UniqueOutcomes",
                table: "PoolCombination",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
