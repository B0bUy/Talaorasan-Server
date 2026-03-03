using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talaorasan.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedFacePoseForEnrollmentImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Pose",
                table: "EnrollmentImages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pose",
                table: "EnrollmentImages");
        }
    }
}
