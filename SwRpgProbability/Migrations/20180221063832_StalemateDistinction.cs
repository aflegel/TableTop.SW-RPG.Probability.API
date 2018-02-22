using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SwRpgProbability.Migrations
{
    public partial class StalemateDistinction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StalemateOutcomes",
                table: "Pool",
                newName: "NeutralOutcomes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NeutralOutcomes",
                table: "Pool",
                newName: "StalemateOutcomes");
        }
    }
}
