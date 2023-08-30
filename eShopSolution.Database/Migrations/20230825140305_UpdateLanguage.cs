using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Database.Migrations
{
    public partial class UpdateLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTranslation_Languages_LanguageId",
                table: "ProductTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTranslation_Products_ProductId",
                table: "ProductTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTranslation",
                table: "ProductTranslation");

            migrationBuilder.RenameTable(
                name: "ProductTranslation",
                newName: "ProductTranslations");

            migrationBuilder.RenameIndex(
                name: "IX_ProductTranslation_ProductId",
                table: "ProductTranslations",
                newName: "IX_ProductTranslations_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductTranslation_LanguageId",
                table: "ProductTranslations",
                newName: "IX_ProductTranslations_LanguageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTranslations",
                table: "ProductTranslations",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "b6bd0bf0-9242-4de4-80b7-733b8554b53b");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d11d65a2-c356-4cf0-8f95-540520a60dd7", "AQAAAAEAACcQAAAAECr4otkC78wJmUN/ZKtWsI9aa2LZo3VLD+Hk3GsRFgSNOwdu6jWXV8jnCvh4ULwDQQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 8, 25, 21, 3, 5, 367, DateTimeKind.Local).AddTicks(9441));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTranslations_Languages_LanguageId",
                table: "ProductTranslations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTranslations_Products_ProductId",
                table: "ProductTranslations",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTranslations_Languages_LanguageId",
                table: "ProductTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTranslations_Products_ProductId",
                table: "ProductTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTranslations",
                table: "ProductTranslations");

            migrationBuilder.RenameTable(
                name: "ProductTranslations",
                newName: "ProductTranslation");

            migrationBuilder.RenameIndex(
                name: "IX_ProductTranslations_ProductId",
                table: "ProductTranslation",
                newName: "IX_ProductTranslation_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductTranslations_LanguageId",
                table: "ProductTranslation",
                newName: "IX_ProductTranslation_LanguageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTranslation",
                table: "ProductTranslation",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "a176452f-8c29-40c5-a29f-b08ef0ac7881");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5270da06-9a62-429d-a238-34d946919700", "AQAAAAEAACcQAAAAEJsPeHKMinm4a2R5CVDpQ6Rw0K/HvwXFIr3ckG8L3FJ/KU9n7E1595tGH7erP5gkZw==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 8, 20, 22, 14, 56, 847, DateTimeKind.Local).AddTicks(5966));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTranslation_Languages_LanguageId",
                table: "ProductTranslation",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTranslation_Products_ProductId",
                table: "ProductTranslation",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
