﻿// <auto-generated />
using System;
using System.Collections.Generic;
using CookApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace cook_api.Migrations
{
    [DbContext(typeof(CookApiContext))]
    [Migration("20240104215225_migration2")]
    partial class migration2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:CollationDefinition:case_insensitive", "und-u-ks-level2,und-u-ks-level2,icu,False")
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CookApi.Models.Ingredient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .UseCollation("case_insensitive");

                    b.Property<List<string>>("Substitutions")
                        .IsRequired()
                        .HasColumnType("varchar(50)[]")
                        .UseCollation("case_insensitive");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("CookApi.Models.Recipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int?>("CookingTime")
                        .HasColumnType("integer");

                    b.Property<string>("Difficulty")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)");

                    b.Property<List<string>>("Ingredients")
                        .IsRequired()
                        .HasColumnType("varchar(200)[]")
                        .UseCollation("case_insensitive");

                    b.Property<string>("Instructions")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .UseCollation("case_insensitive");

                    b.Property<List<string>>("Pictures")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(200)[]")
                        .HasDefaultValue(new List<string>());

                    b.Property<int?>("PreparationTime")
                        .HasColumnType("integer");

                    b.Property<int?>("Servings")
                        .HasColumnType("integer");

                    b.Property<bool>("Vegetarian")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<List<string>>("Videos")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(200)[]")
                        .HasDefaultValue(new List<string>());

                    b.HasKey("Id");

                    b.HasIndex("Ingredients");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("Ingredients"), "gin");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Recipes", t =>
                        {
                            t.HasCheckConstraint("CK_Recipe_CookingTime", "\"CookingTime\" >= 0 OR \"PreparationTime\" IS NULL");

                            t.HasCheckConstraint("CK_Recipe_Difficulty", "\"Difficulty\" IN ('Easy', 'Medium', 'Hard') OR \"Difficulty\" IS NULL");

                            t.HasCheckConstraint("CK_Recipe_Ingredients", "cardinality(\"Ingredients\") > 0");

                            t.HasCheckConstraint("CK_Recipe_PreparationTime", "\"PreparationTime\" > 0 OR \"PreparationTime\" IS NULL");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}