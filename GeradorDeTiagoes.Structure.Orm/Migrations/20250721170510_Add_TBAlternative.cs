using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeradorDeTiagoes.Structure.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Add_TBAlternative : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alternative_Questions_QuestionId",
                table: "Alternative");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alternative",
                table: "Alternative");

            migrationBuilder.RenameTable(
                name: "Alternative",
                newName: "Alternatives");

            migrationBuilder.RenameIndex(
                name: "IX_Alternative_QuestionId",
                table: "Alternatives",
                newName: "IX_Alternatives_QuestionId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subjects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GradeLevel",
                table: "Subjects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Statement",
                table: "Questions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alternatives",
                table: "Alternatives",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alternatives_Questions_QuestionId",
                table: "Alternatives",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alternatives_Questions_QuestionId",
                table: "Alternatives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alternatives",
                table: "Alternatives");

            migrationBuilder.RenameTable(
                name: "Alternatives",
                newName: "Alternative");

            migrationBuilder.RenameIndex(
                name: "IX_Alternatives_QuestionId",
                table: "Alternative",
                newName: "IX_Alternative_QuestionId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "GradeLevel",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Statement",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alternative",
                table: "Alternative",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alternative_Questions_QuestionId",
                table: "Alternative",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
