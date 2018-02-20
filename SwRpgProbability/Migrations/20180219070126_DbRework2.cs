using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SwRpgProbability.Migrations
{
    public partial class DbRework2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "StalemateOutcomes",
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
                name: "TotalOutcomes",
                table: "Pool",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TriumphOutcomes",
                table: "Pool",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UniqueOutcomes",
                table: "Pool",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "StalemateOutcomes",
                table: "Pool");

            migrationBuilder.DropColumn(
                name: "SuccessOutcomes",
                table: "Pool");

            migrationBuilder.DropColumn(
                name: "ThreatOutcomes",
                table: "Pool");

            migrationBuilder.DropColumn(
                name: "TotalOutcomes",
                table: "Pool");

            migrationBuilder.DropColumn(
                name: "TriumphOutcomes",
                table: "Pool");

            migrationBuilder.DropColumn(
                name: "UniqueOutcomes",
                table: "Pool");
        }
    }
}
