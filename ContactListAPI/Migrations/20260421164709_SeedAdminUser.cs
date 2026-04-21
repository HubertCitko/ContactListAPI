using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactListAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "BirthDate", "CategoryID", "Email", "Name", "OwnSubcategory", "Password", "Phone", "SubcategoryID", "Surname" },
                values: new object[] { 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "admin@gmail.com", "Admin", null, "$2a$11$z9Zt8/Ic/s2ohv1aR.xiVuGYfBHlHytS7cKlEj8QVLIKTqwZREw.a", "777777777", 1, "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CategoryID",
                table: "Contacts",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_SubcategoryID",
                table: "Contacts",
                column: "SubcategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Categories_CategoryID",
                table: "Contacts",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Subcategories_SubcategoryID",
                table: "Contacts",
                column: "SubcategoryID",
                principalTable: "Subcategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Categories_CategoryID",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Subcategories_SubcategoryID",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_CategoryID",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_SubcategoryID",
                table: "Contacts");

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
