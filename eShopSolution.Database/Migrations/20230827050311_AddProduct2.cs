using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Database.Migrations
{
    public partial class AddProduct2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "227415a0-7778-485b-b27a-a37c41186381");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0f22b1a2-c292-4557-b05e-5a6b5a773be0", "AQAAAAEAACcQAAAAEHFFdfGQ3W9TH+HNCGLIgx4SXRz1mHSmX7Axozhlx0IEQykW/R4ffjEZz9CHH/V0eg==" });

            migrationBuilder.UpdateData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 2,
                column: "LanguageId",
                value: "vi");

            migrationBuilder.UpdateData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "LanguageId", "Name" },
                values: new object[] { "vi", "Áo nam" });

            migrationBuilder.InsertData(
                table: "CategoryTranslations",
                columns: new[] { "Id", "CategoryId", "LanguageId", "Name", "SeoAlias", "SeoDescription", "SeoTitle" },
                values: new object[,]
                {
                    { 5, 1, "vi", "Women Shirt", "women-shirt", "The shirt products for women", "The shirt products for women" },
                    { 6, 1, "vi", "Áo nam", "women-shirt", "The shirt products for women", "The shirt products for women" },
                    { 7, 2, "vi", "Women Shirt", "women-shirt", "The shirt products for women", "The shirt products for women" }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 8, 27, 12, 3, 10, 628, DateTimeKind.Local).AddTicks(8495));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 7);

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
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 2,
                column: "LanguageId",
                value: "en");

            migrationBuilder.UpdateData(
                table: "CategoryTranslations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "LanguageId", "Name" },
                values: new object[] { "en", "Women Shirt" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 8, 27, 11, 44, 2, 690, DateTimeKind.Local).AddTicks(1024));
        }
    }
}
