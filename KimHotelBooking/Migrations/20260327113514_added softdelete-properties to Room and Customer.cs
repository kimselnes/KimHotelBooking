using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KimHotelBooking.Migrations
{
    /// <inheritdoc />
    public partial class addedsoftdeletepropertiestoRoomandCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "isActive",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "isActive",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Customer");
        }
    }
}
