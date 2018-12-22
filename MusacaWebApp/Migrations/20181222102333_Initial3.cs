using Microsoft.EntityFrameworkCore.Migrations;

namespace MusacaWebApp.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptOrders_Orders_OrderId",
                table: "ReceiptOrders");

            migrationBuilder.DropTable(
                name: "UserOrders");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptOrders_OrderId",
                table: "ReceiptOrders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ReceiptOrders");

            migrationBuilder.AddColumn<string>(
                name: "ReceiptOrderId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReceiptOrderId",
                table: "Orders",
                column: "ReceiptOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ReceiptOrders_ReceiptOrderId",
                table: "Orders",
                column: "ReceiptOrderId",
                principalTable: "ReceiptOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ReceiptOrders_ReceiptOrderId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ReceiptOrderId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceiptOrderId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "ReceiptOrders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserOrders",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CashierId = table.Column<string>(nullable: true),
                    OrderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOrders_Users_CashierId",
                        column: x => x.CashierId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptOrders_OrderId",
                table: "ReceiptOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrders_CashierId",
                table: "UserOrders",
                column: "CashierId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrders_OrderId",
                table: "UserOrders",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptOrders_Orders_OrderId",
                table: "ReceiptOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
