using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Database.Migrations
{
    public partial class AddProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "c6f1558f-839e-483a-a310-c07598538a69");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "81c1f183-e90e-43f5-aa98-7cbbe43453ef", "AQAAAAEAACcQAAAAEPITD3Tji2GtUe3fbTbOcL1nqRNKHU57nqiNdaoFgFKyuHaqDJoMcSrewo0J9JCfBg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 8, 27, 11, 44, 2, 690, DateTimeKind.Local).AddTicks(1024));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "1f023d78-fbfa-4d8d-a35f-4fc518d14429");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c565386d-c24c-4873-92df-cc1f5b129541", "AQAAAAEAACcQAAAAED7GmqAygZkgPXgxdUHru+kmCblp0aPmpDttvn5xBZPRo56Obccf4KuCCtBlO0Oa3Q==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 8, 25, 21, 49, 14, 341, DateTimeKind.Local).AddTicks(7095));
        }
    }
}
