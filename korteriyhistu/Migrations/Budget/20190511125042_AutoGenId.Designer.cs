﻿// <auto-generated />
using korteriyhistu.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace korteriyhistu.Migrations.Budget
{
    [DbContext(typeof(BudgetContext))]
    [Migration("20190511125042_AutoGenId")]
    partial class AutoGenId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("korteriyhistu.Models.BudgetEntry", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("sum");

                    b.HasKey("id");

                    b.ToTable("Budget");
                });
#pragma warning restore 612, 618
        }
    }
}
