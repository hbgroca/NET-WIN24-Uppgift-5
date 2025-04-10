using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberEntityProjectEntity_Projects_ProjectsId",
                table: "MemberEntityProjectEntity");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberEntityProjectEntity_Projects_ProjectsId",
                table: "MemberEntityProjectEntity",
                column: "ProjectsId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberEntityProjectEntity_Projects_ProjectsId",
                table: "MemberEntityProjectEntity");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberEntityProjectEntity_Projects_ProjectsId",
                table: "MemberEntityProjectEntity",
                column: "ProjectsId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
