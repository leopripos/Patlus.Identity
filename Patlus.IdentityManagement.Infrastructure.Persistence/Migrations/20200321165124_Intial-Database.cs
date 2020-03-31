using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Patlus.IdentityManagement.Persistence.Migrations
{
    public partial class IntialDatabase : Migration
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
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedTime = table.Column<DateTimeOffset>(nullable: false),
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
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedTime = table.Column<DateTimeOffset>(nullable: false),
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
                    AuthKey = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedTime = table.Column<DateTimeOffset>(nullable: false),
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
                values: new object[] { new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"), false, new DateTimeOffset(new DateTime(2020, 3, 21, 16, 51, 23, 756, DateTimeKind.Unspecified).AddTicks(9782), new TimeSpan(0, 0, 0, 0, 0)), new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"), new DateTimeOffset(new DateTime(2020, 3, 21, 16, 51, 23, 757, DateTimeKind.Unspecified).AddTicks(258), new TimeSpan(0, 0, 0, 0, 0)), "root", "YRktoPpuxUe8JlLYP5QC1qOuC7/JoUcJ4bjZmw6cpXU=" });

            migrationBuilder.InsertData(
                table: "Pool",
                columns: new[] { "Id", "Active", "Archived", "CreatedTime", "CreatorId", "Description", "LastModifiedTime", "Name" },
                values: new object[] { new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf"), true, false, new DateTimeOffset(new DateTime(2020, 3, 21, 16, 51, 23, 746, DateTimeKind.Unspecified).AddTicks(1228), new TimeSpan(0, 0, 0, 0, 0)), new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"), "Default identity pool for system administrator.", new DateTimeOffset(new DateTime(2020, 3, 21, 16, 51, 23, 746, DateTimeKind.Unspecified).AddTicks(1694), new TimeSpan(0, 0, 0, 0, 0)), "Root Administrator" });

            migrationBuilder.InsertData(
                table: "Identity",
                columns: new[] { "Id", "Active", "Archived", "AuthKey", "CreatedTime", "CreatorId", "LastModifiedTime", "Name", "PoolId" },
                values: new object[] { new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"), true, false, new Guid("1774a09a-3821-4ff4-ba16-08770c9c797d"), new DateTimeOffset(new DateTime(2020, 3, 21, 16, 51, 23, 747, DateTimeKind.Unspecified).AddTicks(5389), new TimeSpan(0, 0, 0, 0, 0)), new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"), new DateTimeOffset(new DateTime(2020, 3, 21, 16, 51, 23, 747, DateTimeKind.Unspecified).AddTicks(5779), new TimeSpan(0, 0, 0, 0, 0)), "root", new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf") });

            migrationBuilder.CreateIndex(
                name: "IX_HostedAccount_Name",
                table: "HostedAccount",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_AuthKey",
                table: "Identity",
                column: "AuthKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Name",
                table: "Identity",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_PoolId",
                table: "Identity",
                column: "PoolId");
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
