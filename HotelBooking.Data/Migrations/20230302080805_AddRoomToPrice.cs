using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomToPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FHB_Room_PriceId",
                table: "FHB_Room");

            migrationBuilder.CreateIndex(
                name: "IX_FHB_Room_PriceId",
                table: "FHB_Room",
                column: "PriceId",
                unique: true,
                filter: "[PriceId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FHB_Room_PriceId",
                table: "FHB_Room");

            migrationBuilder.CreateIndex(
                name: "IX_FHB_Room_PriceId",
                table: "FHB_Room",
                column: "PriceId");
        }
    }
}
