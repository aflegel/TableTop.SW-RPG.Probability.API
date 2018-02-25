using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DataFramework.Migrations
{
    public partial class CacheCleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailureOutcomes",
                table: "PoolCombination");

            migrationBuilder.DropColumn(
                name: "NeutralOutcomes",
                table: "PoolCombination");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FailureOutcomes",
                table: "PoolCombination",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "NeutralOutcomes",
                table: "PoolCombination",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
