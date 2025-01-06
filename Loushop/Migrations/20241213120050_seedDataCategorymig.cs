using Microsoft.EntityFrameworkCore.Migrations;

namespace Loushop.Migrations
{
    public partial class seedDataCategorymig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "گروه لباس ورزشی", "لباس ورزشی" },
                    { 2, "گروه ساعت مچی", "ساعت مچی" },
                    { 3, "گروه کالای دیجیتال", "کالای دیجیتال" },
                    { 4, "گروه لوازم منزل", "لوازم منزل" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
