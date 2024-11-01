using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
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
                name: "roles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    stock_quantity = table.Column<int>(type: "integer", nullable: false),
                    manufacturer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    component_characteristics = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_products_manufacturers_manufacturer_id",
                        column: x => x.manufacturer_id,
                        principalTable: "manufacturers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "userRoles",
                columns: table => new
                {
                    roles_id = table.Column<string>(type: "text", nullable: false),
                    users_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.roles_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_user_roles_roles_roles_id",
                        column: x => x.roles_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_users_id",
                        column: x => x.users_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("33a0e59a-97f1-406e-b379-22e54cbe14d5"), "Hdd" },
                    { new Guid("34ca2b06-3f33-40a6-9f22-3c966508661e"), "Sdd" },
                    { new Guid("35427fb9-c82e-4f8b-8ce7-bf5f6bb3476d"), "Cooler" },
                    { new Guid("443b51a2-cd7e-4930-8772-a52d5756a786"), "Processor" },
                    { new Guid("51d2f9df-1904-4bb3-8c31-e5732a3e5da7"), "Ram" },
                    { new Guid("5260ee46-19ea-4084-a9ea-c49146077d54"), "Psu" },
                    { new Guid("60d465fc-72cc-49e5-bf36-48bdd839e0fd"), "Computer case" },
                    { new Guid("719d528a-571d-43ea-aabb-2fad0bd83929"), "Graphics Card" },
                    { new Guid("854c71f0-2c55-4277-8453-98ef121511e2"), "Motherboard" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { "Administrator", "Administrator" },
                    { "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "image", "name", "password_hash" },
                values: new object[,]
                {
                    { new Guid("299f3c4f-8537-428a-b1e8-eb0794bc6e49"), "user@example.com", null, "user", "PvpQ0/xa69pXsyublcJ1lw==:F4Uw34hFvVb820linGngxtWLqdTqzNFwUvnz+O3knjw=" },
                    { new Guid("84da1939-bc9a-4270-9ed4-253e28cf0f81"), "admin@example.com", null, "admin", "wA6CToNJF8tbW9U6wX2e1w==:AckPrtzfcQ/BeUB/+X/rIWiQcxmTpf7EaT9jab/BThs=" }
                });

            migrationBuilder.InsertData(
                table: "userRoles",
                columns: new[] { "roles_id", "users_id" },
                values: new object[,]
                {
                    { "Administrator", new Guid("84da1939-bc9a-4270-9ed4-253e28cf0f81") },
                    { "User", new Guid("299f3c4f-8537-428a-b1e8-eb0794bc6e49") }
                });

            migrationBuilder.CreateIndex(
                name: "ix_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_manufacturer_id",
                table: "products",
                column: "manufacturer_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_users_id",
                table: "userRoles",
                column: "users_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "userRoles");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "manufacturers");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
