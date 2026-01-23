using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointly.Infrastructure.Migrations
{
    public partial class AddAdvertismentTableAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advertisments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),

                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    EngineCapacity = table.Column<int>(type: "integer", nullable: true),

                    FuelType = table.Column<int>(type: "integer", nullable: false),
                    TransmissionType = table.Column<int>(type: "integer", nullable: false),
                    VehicleCondition = table.Column<int>(type: "integer", nullable: false),

                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),

                    BrandId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModelId = table.Column<Guid>(type: "uuid", nullable: false),
                    CityId = table.Column<Guid>(type: "uuid", nullable: false),

                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewByAdminId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),

                    ContactName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContactPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ContactEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),

                    IsHidePhone = table.Column<bool>(type: "boolean", nullable: false),
                    IsWhatsapp = table.Column<bool>(type: "boolean", nullable: true),
                    IsBiddable = table.Column<bool>(type: "boolean", nullable: false),

                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RejectedReason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisments", x => x.Id);

                    table.ForeignKey(
                        name: "FK_Advertisments_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Advertisments_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Advertisments_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Advertisments_Users_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_Advertisments_Users_ReviewByAdminId",
                        column: x => x.ReviewByAdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Indexes
            migrationBuilder.CreateIndex("IX_Advertisments_BrandId", "Advertisments", "BrandId");
            migrationBuilder.CreateIndex("IX_Advertisments_ModelId", "Advertisments", "ModelId");
            migrationBuilder.CreateIndex("IX_Advertisments_CityId", "Advertisments", "CityId");
            migrationBuilder.CreateIndex("IX_Advertisments_SellerId", "Advertisments", "SellerId");
            migrationBuilder.CreateIndex("IX_Advertisments_ReviewByAdminId", "Advertisments", "ReviewByAdminId");
            migrationBuilder.CreateIndex("IX_Advertisments_Price", "Advertisments", "Price");
            migrationBuilder.CreateIndex("IX_Advertisments_Year", "Advertisments", "Year");
            migrationBuilder.CreateIndex("IX_Advertisments_Status", "Advertisments", "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertisments");
        }
    }
}
