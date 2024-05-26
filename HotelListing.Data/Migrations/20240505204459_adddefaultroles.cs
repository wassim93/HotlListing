using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelListing.Data.Migrations
{
    public partial class adddefaultroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "227ea39e-52aa-48fb-aea7-2a76ec9e7274", "0d1af601-c28c-48ad-a3e8-2983439f5219", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6530defd-169a-46f2-ad7b-0d22696f3492", "fa8e9bed-80b9-4be5-a8cd-7fc2f3917306", "admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "227ea39e-52aa-48fb-aea7-2a76ec9e7274");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6530defd-169a-46f2-ad7b-0d22696f3492");
        }
    }
}
