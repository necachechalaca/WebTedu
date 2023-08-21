using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Database.Migrations
{
    public partial class ChangeLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "a2bb658f-966e-433b-af40-6c099e881c6b");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0c494e43-82cb-4cd3-9480-eaf1c6aa78c8", "AQAAAAEAACcQAAAAEPF0UE2CHT+V+Rh7Yp2Tx59NdCwHVhJAHO2uP/o0jzvrIQZJWr4AlkmHPpEvbF/5Hg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 8, 13, 17, 8, 36, 237, DateTimeKind.Local).AddTicks(5808));
        }
    }
}
