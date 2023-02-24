using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAddressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FHB_Hotel_AddressId",
                table: "FHB_Hotel");

            migrationBuilder.CreateIndex(
                name: "IX_FHB_Hotel_AddressId",
                table: "FHB_Hotel",
                column: "AddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FHB_Hotel_AddressId",
                table: "FHB_Hotel");

            migrationBuilder.CreateIndex(
                name: "IX_FHB_Hotel_AddressId",
                table: "FHB_Hotel",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");
        }
    }
}
