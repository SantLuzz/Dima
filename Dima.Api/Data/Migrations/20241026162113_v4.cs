using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dima.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "NVARCHAR(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: true),
                    Slug = table.Column<string>(type: "NVARCHAR(80)", maxLength: 80, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "MONEY", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vouncher",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "CHAR(8)", maxLength: 8, nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false),
                    Amount = table.Column<decimal>(type: "MONEY", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouncher", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: false),
                    ExternalReference = table.Column<string>(type: "VARCHAR(60)", maxLength: 60, nullable: true),
                    Gateway = table.Column<short>(type: "SMALLINT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    Status = table.Column<short>(type: "SMALLINT", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    VouncherId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Vouncher_VouncherId",
                        column: x => x.VouncherId,
                        principalTable: "Vouncher",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ProductId",
                table: "Order",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_VouncherId",
                table: "Order",
                column: "VouncherId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouncher_Number",
                table: "Vouncher",
                column: "Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Vouncher");
        }
    }
}
