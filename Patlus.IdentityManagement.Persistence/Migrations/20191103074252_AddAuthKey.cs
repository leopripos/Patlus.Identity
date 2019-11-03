using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Patlus.IdentityManagement.Persistence.Migrations
{
    public partial class AddAuthKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuthKey",
                table: "Identity",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "HostedAccount",
                keyColumn: "Id",
                keyValue: new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"),
                columns: new[] { "CreatedTime", "LastModifiedTime" },
                values: new object[] { new DateTime(2019, 11, 3, 14, 42, 51, 476, DateTimeKind.Local).AddTicks(8362), new DateTime(2019, 11, 3, 14, 42, 51, 476, DateTimeKind.Local).AddTicks(9296) });

            migrationBuilder.UpdateData(
                table: "Identity",
                keyColumn: "Id",
                keyValue: new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"),
                columns: new[] { "AuthKey", "CreatedTime", "LastModifiedTime" },
                values: new object[] { new Guid("00431702-9695-41a3-bfe7-c9f23b5bd4e3"), new DateTime(2019, 11, 3, 14, 42, 51, 459, DateTimeKind.Local).AddTicks(292), new DateTime(2019, 11, 3, 14, 42, 51, 459, DateTimeKind.Local).AddTicks(1111) });

            migrationBuilder.UpdateData(
                table: "Pool",
                keyColumn: "Id",
                keyValue: new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf"),
                columns: new[] { "CreatedTime", "LastModifiedTime" },
                values: new object[] { new DateTime(2019, 11, 3, 14, 42, 51, 455, DateTimeKind.Local).AddTicks(790), new DateTime(2019, 11, 3, 14, 42, 51, 456, DateTimeKind.Local).AddTicks(4874) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthKey",
                table: "Identity");

            migrationBuilder.UpdateData(
                table: "HostedAccount",
                keyColumn: "Id",
                keyValue: new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"),
                columns: new[] { "CreatedTime", "LastModifiedTime" },
                values: new object[] { new DateTime(2019, 10, 12, 9, 1, 19, 928, DateTimeKind.Utc).AddTicks(2220), new DateTime(2019, 10, 12, 9, 1, 19, 928, DateTimeKind.Utc).AddTicks(2938) });

            migrationBuilder.UpdateData(
                table: "Identity",
                keyColumn: "Id",
                keyValue: new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"),
                columns: new[] { "CreatedTime", "LastModifiedTime" },
                values: new object[] { new DateTime(2019, 10, 12, 9, 1, 19, 917, DateTimeKind.Utc).AddTicks(5068), new DateTime(2019, 10, 12, 9, 1, 19, 917, DateTimeKind.Utc).AddTicks(5714) });

            migrationBuilder.UpdateData(
                table: "Pool",
                keyColumn: "Id",
                keyValue: new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf"),
                columns: new[] { "CreatedTime", "LastModifiedTime" },
                values: new object[] { new DateTime(2019, 10, 12, 9, 1, 19, 915, DateTimeKind.Utc).AddTicks(5242), new DateTime(2019, 10, 12, 9, 1, 19, 915, DateTimeKind.Utc).AddTicks(6338) });
        }
    }
}
