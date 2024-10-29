﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Auth.Roles.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.ToTable("roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "User",
                            Name = "User"
                        },
                        new
                        {
                            Id = "Administrator",
                            Name = "Administrator"
                        });
                });

            modelBuilder.Entity("Domain.Auth.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Image")
                        .HasColumnType("text")
                        .HasColumnName("image");

                    b.Property<string>("Name")
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)")
                        .HasColumnName("name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Domain.Categories.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.ToTable("categories", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("5aae21a1-ec96-42c1-808a-c3dcdc6c6fec"),
                            Name = "Processor"
                        },
                        new
                        {
                            Id = new Guid("e1d401da-d336-4a44-8de9-b5824c636bba"),
                            Name = "Computer case"
                        },
                        new
                        {
                            Id = new Guid("09b3b6f4-8107-44d4-9324-adf8b2fb301d"),
                            Name = "Graphics Card"
                        });
                });

            modelBuilder.Entity("Domain.Manufacturers.Manufacturer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_manufacturers");

                    b.ToTable("manufacturers", (string)null);
                });

            modelBuilder.Entity("Domain.Products.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.Property<Guid>("ManufacturerId")
                        .HasColumnType("uuid")
                        .HasColumnName("manufacturer_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("stock_quantity");

                    b.HasKey("Id")
                        .HasName("pk_products");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_products_category_id");

                    b.HasIndex("ManufacturerId")
                        .HasDatabaseName("ix_products_manufacturer_id");

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<string>("RolesId")
                        .HasColumnType("text")
                        .HasColumnName("roles_id");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid")
                        .HasColumnName("users_id");

                    b.HasKey("RolesId", "UsersId")
                        .HasName("pk_user_roles");

                    b.HasIndex("UsersId")
                        .HasDatabaseName("ix_user_roles_users_id");

                    b.ToTable("userRoles", (string)null);
                });

            modelBuilder.Entity("Domain.Products.Product", b =>
                {
                    b.HasOne("Domain.Categories.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_products_categories_category_id");

                    b.HasOne("Domain.Manufacturers.Manufacturer", "Manufacturer")
                        .WithMany()
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_products_manufacturers_manufacturer_id");

                    b.OwnsOne("Domain.Products.ComponentCharacteristic", "ComponentCharacteristic", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uuid");

                            b1.HasKey("ProductId");

                            b1.ToTable("products");

                            b1.ToJson("component_characteristic");

                            b1.WithOwner()
                                .HasForeignKey("ProductId")
                                .HasConstraintName("fk_products_products_id");

                            b1.OwnsOne("Domain.Products.CPU", "Cpu", b2 =>
                                {
                                    b2.Property<Guid>("ComponentCharacteristicProductId")
                                        .HasColumnType("uuid");

                                    b2.Property<double>("BaseClock")
                                        .HasColumnType("decimal(5, 2)")
                                        .HasAnnotation("Relational:JsonPropertyName", "base clock");

                                    b2.Property<double>("BoostClock")
                                        .HasColumnType("decimal(5, 2)")
                                        .HasAnnotation("Relational:JsonPropertyName", "boost clock");

                                    b2.Property<int>("Cores")
                                        .HasColumnType("integer")
                                        .HasAnnotation("Relational:JsonPropertyName", "cores");

                                    b2.Property<string>("Model")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasAnnotation("Relational:JsonPropertyName", "model");

                                    b2.Property<int>("Threads")
                                        .HasColumnType("integer")
                                        .HasAnnotation("Relational:JsonPropertyName", "threads");

                                    b2.HasKey("ComponentCharacteristicProductId");

                                    b2.ToTable("products");

                                    b2.ToJson("component_characteristic");

                                    b2.WithOwner()
                                        .HasForeignKey("ComponentCharacteristicProductId")
                                        .HasConstraintName("fk_products_products_component_characteristic_product_id");
                                });

                            b1.OwnsOne("Domain.Products.Case", "Case", b2 =>
                                {
                                    b2.Property<Guid>("ComponentCharacteristicProductId")
                                        .HasColumnType("uuid");

                                    b2.Property<string>("CoolingSystem")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasAnnotation("Relational:JsonPropertyName", "cooling system");

                                    b2.Property<string>("FormFactors")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasAnnotation("Relational:JsonPropertyName", "form factors");

                                    b2.Property<int>("NumberOfFans")
                                        .HasColumnType("integer")
                                        .HasAnnotation("Relational:JsonPropertyName", "number of fans");

                                    b2.HasKey("ComponentCharacteristicProductId");

                                    b2.ToTable("products");

                                    b2.ToJson("component_characteristic");

                                    b2.WithOwner()
                                        .HasForeignKey("ComponentCharacteristicProductId")
                                        .HasConstraintName("fk_products_products_component_characteristic_product_id");
                                });

                            b1.OwnsOne("Domain.Products.GPU", "Gpu", b2 =>
                                {
                                    b2.Property<Guid>("ComponentCharacteristicProductId")
                                        .HasColumnType("uuid");

                                    b2.Property<decimal>("BoostClock")
                                        .HasColumnType("decimal(6, 2)")
                                        .HasAnnotation("Relational:JsonPropertyName", "boost clock");

                                    b2.Property<decimal>("CoreClock")
                                        .HasColumnType("decimal(6, 2)")
                                        .HasAnnotation("Relational:JsonPropertyName", "core clock");

                                    b2.Property<int>("MemorySize")
                                        .HasColumnType("integer")
                                        .HasAnnotation("Relational:JsonPropertyName", "memory size");

                                    b2.Property<string>("MemoryType")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasAnnotation("Relational:JsonPropertyName", "memory type");

                                    b2.Property<string>("Model")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasAnnotation("Relational:JsonPropertyName", "model");

                                    b2.HasKey("ComponentCharacteristicProductId");

                                    b2.ToTable("products");

                                    b2.ToJson("component_characteristic");

                                    b2.WithOwner()
                                        .HasForeignKey("ComponentCharacteristicProductId")
                                        .HasConstraintName("fk_products_products_component_characteristic_product_id");
                                });

                            b1.Navigation("Case");

                            b1.Navigation("Cpu");

                            b1.Navigation("Gpu");
                        });

                    b.Navigation("Category");

                    b.Navigation("ComponentCharacteristic")
                        .IsRequired();

                    b.Navigation("Manufacturer");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("Domain.Auth.Roles.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_roles_roles_roles_id");

                    b.HasOne("Domain.Auth.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_roles_users_users_id");
                });
#pragma warning restore 612, 618
        }
    }
}
