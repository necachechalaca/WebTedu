using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Database.Migrations
{
    public partial class AddProduct3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "b439d4b6-7ee4-400b-8e40-3a9cc3fbab56");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "598f9634-05d2-4227-a20d-257800fa8562", "AQAAAAEAACcQAAAAEKOLc26I42tRixEAAtDh7eknZkJiDAJ+4djiUKdIO6gZaYjgLA7IeNeus4Egfog3zA==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 8, 27, 12, 7, 32, 509, DateTimeKind.Local).AddTicks(2315));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 8, 27, 12, 3, 10, 628, DateTimeKind.Local).AddTicks(8495));
        }
    }
}
