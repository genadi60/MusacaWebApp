using Microsoft.EntityFrameworkCore.Migrations;

namespace MusacaWebApp.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Receipts_ReceiptId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ReceiptId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceiptId",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "ReceiptOrders",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ReceiptId = table.Column<string>(nullable: true),
                    OrderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptOrders_Receipts_ReceiptId",
                        column: x => x.ReceiptId,
                        principalTable: "Receipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptOrders_OrderId",
                table: "ReceiptOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptOrders_ReceiptId",
                table: "ReceiptOrders",
                column: "ReceiptId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptOrders");

            migrationBuilder.AddColumn<string>(
                name: "ReceiptId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReceiptId",
                table: "Orders",
                column: "ReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Receipts_ReceiptId",
                table: "Orders",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
