using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "form_factors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_form_factors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "manufacturers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_manufacturers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_materials",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_materials", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    stock_quantity = table.Column<int>(type: "integer", nullable: false),
                    discriminator = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    number_of_fans = table.Column<int>(type: "integer", nullable: true),
                    cooling_system = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    manufacturer_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_manufacturers_manufacturer_id",
                        column: x => x.manufacturer_id,
                        principalTable: "manufacturers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseFormFactors",
                columns: table => new
                {
                    cases_id = table.Column<Guid>(type: "uuid", nullable: false),
                    form_factors_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_case_form_factors", x => new { x.cases_id, x.form_factors_id });
                    table.ForeignKey(
                        name: "fk_case_form_factors_cases_cases_id",
                        column: x => x.cases_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_case_form_factors_form_factors_form_factors_id",
                        column: x => x.form_factors_id,
                        principalTable: "form_factors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductProductMaterials",
                columns: table => new
                {
                    product_materials_id = table.Column<Guid>(type: "uuid", nullable: false),
                    products_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_product_materials", x => new { x.product_materials_id, x.products_id });
                    table.ForeignKey(
                        name: "fk_product_product_materials_product_materials_product_materials",
                        column: x => x.product_materials_id,
                        principalTable: "product_materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_product_materials_products_products_id",
                        column: x => x.products_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_case_form_factors_form_factors_id",
                table: "CaseFormFactors",
                column: "form_factors_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_product_materials_products_id",
                table: "ProductProductMaterials",
                column: "products_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_manufacturer_id",
                table: "products",
                column: "manufacturer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseFormFactors");

            migrationBuilder.DropTable(
                name: "ProductProductMaterials");

            migrationBuilder.DropTable(
                name: "form_factors");

            migrationBuilder.DropTable(
                name: "product_materials");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "manufacturers");
        }
    }
}
