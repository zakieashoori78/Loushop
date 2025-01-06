using Microsoft.EntityFrameworkCore.Migrations;

namespace Loushop.Migrations
{
    public partial class addmosttablesid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Price", "QuantityInStoke" },
                values: new object[] { 1, 850.0m, 5 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Price", "QuantityInStoke" },
                values: new object[] { 2, 321.0m, 7 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Price", "QuantityInStoke" },
                values: new object[] { 3, 2500m, 2 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ItemId", "Name" },
                values: new object[] { 1, "یکی از  محصولات شرکت گلرنگ", 1, "شامپو گلرنگ" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ItemId", "Name" },
                values: new object[] { 2, "یکی از محصولات شرکت سامسونگ", 2, "گوشی j5" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ItemId", "Name" },
                values: new object[] { 3, "یکی از محصولات شرکت نیکتا", 3, "دمپایی ابری" });

            migrationBuilder.InsertData(
                table: "CategoryToProducts",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 3, 3 },
                    { 4, 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryToProducts",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "CategoryToProducts",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "CategoryToProducts",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "CategoryToProducts",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "CategoryToProducts",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "CategoryToProducts",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "CategoryToProducts",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "CategoryToProducts",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "CategoryToProducts",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "CategoryToProducts",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "CategoryToProducts",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "CategoryToProducts",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
