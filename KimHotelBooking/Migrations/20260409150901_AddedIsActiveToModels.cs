using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KimHotelBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsActiveToModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isAvailable",
                table: "Room",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Room",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "isPaid",
                table: "Invoice",
                newName: "IsPaid");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Customer",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "isCheckedIn",
                table: "Booking",
                newName: "IsCheckedIn");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Room",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Invoice",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Customer",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Booking",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Room",
                newName: "isAvailable");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Room",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "IsPaid",
                table: "Invoice",
                newName: "isPaid");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Customer",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "IsCheckedIn",
                table: "Booking",
                newName: "isCheckedIn");

            migrationBuilder.AlterColumn<int>(
                name: "isActive",
                table: "Room",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "isActive",
                table: "Customer",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
