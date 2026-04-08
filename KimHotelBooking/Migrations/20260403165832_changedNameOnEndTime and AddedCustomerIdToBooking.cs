using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KimHotelBooking.Migrations
{
    /// <inheritdoc />
    public partial class changedNameOnEndTimeandAddedCustomerIdToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Booking",
                newName: "EndDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Booking",
                newName: "EndTime");
        }
    }
}
