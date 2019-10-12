using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Patlus.IdentityManagement.Persistence.Migrations
{
    public partial class IniilStrctre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HostedAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastModifiedTime = table.Column<DateTime>(nullable: false),
                    Archived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostedAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pool",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastModifiedTime = table.Column<DateTime>(nullable: false),
                    Archived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pool", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PoolId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    LastModifiedTime = table.Column<DateTime>(nullable: false),
                    Archived = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_HostedAccount_Id",
                        column: x => x.Id,
                        principalTable: "HostedAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Identity_Pool_PoolId",
                        column: x => x.PoolId,
                        principalTable: "Pool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "HostedAccount",
                columns: new[] { "Id", "Archived", "CreatedTime", "CreatorId", "LastModifiedTime", "Name", "Password" },
                values: new object[] { new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"), false, new DateTime(2019, 10, 12, 9, 1, 19, 928, DateTimeKind.Utc).AddTicks(2220), new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"), new DateTime(2019, 10, 12, 9, 1, 19, 928, DateTimeKind.Utc).AddTicks(2938), "root", "YRktoPpuxUe8JlLYP5QC1qOuC7/JoUcJ4bjZmw6cpXU=" });

            migrationBuilder.InsertData(
                table: "Pool",
                columns: new[] { "Id", "Active", "Archived", "CreatedTime", "CreatorId", "Description", "LastModifiedTime", "Name" },
                values: new object[] { new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf"), true, false, new DateTime(2019, 10, 12, 9, 1, 19, 915, DateTimeKind.Utc).AddTicks(5242), new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"), "Default identity pool for system administrator.", new DateTime(2019, 10, 12, 9, 1, 19, 915, DateTimeKind.Utc).AddTicks(6338), "System Administrator" });

            migrationBuilder.InsertData(
                table: "Identity",
                columns: new[] { "Id", "Active", "Archived", "CreatedTime", "CreatorId", "LastModifiedTime", "Name", "PoolId" },
                values: new object[] { new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"), true, false, new DateTime(2019, 10, 12, 9, 1, 19, 917, DateTimeKind.Utc).AddTicks(5068), new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"), new DateTime(2019, 10, 12, 9, 1, 19, 917, DateTimeKind.Utc).AddTicks(5714), "root", new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf") });

            migrationBuilder.CreateIndex(
                name: "IX_Identity_PoolId",
                table: "Identity",
                column: "PoolId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Identity");

            migrationBuilder.DropTable(
                name: "HostedAccount");

            migrationBuilder.DropTable(
                name: "Pool");
        }
    }
}
