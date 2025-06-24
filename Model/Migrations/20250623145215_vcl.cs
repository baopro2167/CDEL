using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class vcl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSampleMethod_SampleMethod_SampleMethodId",
                table: "ServiceSampleMethod");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSampleMethod_Service_ServiceId",
                table: "ServiceSampleMethod");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceSampleMethod",
                table: "ServiceSampleMethod");

            migrationBuilder.DropIndex(
                name: "IX_ServiceSampleMethod_ServiceId",
                table: "ServiceSampleMethod");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ServiceSampleMethod");

            migrationBuilder.RenameTable(
                name: "ServiceSampleMethod",
                newName: "ServiceSampleMethods");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceSampleMethod_SampleMethodId",
                table: "ServiceSampleMethods",
                newName: "IX_ServiceSampleMethods_SampleMethodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceSampleMethods",
                table: "ServiceSampleMethods",
                columns: new[] { "ServiceId", "SampleMethodId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSampleMethods_SampleMethod_SampleMethodId",
                table: "ServiceSampleMethods",
                column: "SampleMethodId",
                principalTable: "SampleMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSampleMethods_Service_ServiceId",
                table: "ServiceSampleMethods",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSampleMethods_SampleMethod_SampleMethodId",
                table: "ServiceSampleMethods");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSampleMethods_Service_ServiceId",
                table: "ServiceSampleMethods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceSampleMethods",
                table: "ServiceSampleMethods");

            migrationBuilder.RenameTable(
                name: "ServiceSampleMethods",
                newName: "ServiceSampleMethod");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceSampleMethods_SampleMethodId",
                table: "ServiceSampleMethod",
                newName: "IX_ServiceSampleMethod_SampleMethodId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ServiceSampleMethod",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceSampleMethod",
                table: "ServiceSampleMethod",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSampleMethod_ServiceId",
                table: "ServiceSampleMethod",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSampleMethod_SampleMethod_SampleMethodId",
                table: "ServiceSampleMethod",
                column: "SampleMethodId",
                principalTable: "SampleMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSampleMethod_Service_ServiceId",
                table: "ServiceSampleMethod",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
