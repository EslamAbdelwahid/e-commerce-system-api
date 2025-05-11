using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Es.Repository.Identity.Migrations
{
    /// <inheritdoc />
    public partial class updateColumnStreetName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Streed",
                table: "Address",
                newName: "Street");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Address",
                newName: "Streed");
        }
    }
}
