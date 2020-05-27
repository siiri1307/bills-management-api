﻿// <auto-generated />
using korteriyhistu.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace korteriyhistu.Migrations
{
    [DbContext(typeof(BillsContext))]
    partial class BillsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("korteriyhistu.Models.Bill", b =>
                {
                    b.Property<int>("BillId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Apartment");

                    b.Property<string>("Comment");

                    b.Property<int>("MonthToPayFor");

                    b.Property<int>("Number");

                    b.Property<DateTime>("PaymentDeadline");

                    b.Property<int>("Status");

                    b.Property<double>("SumToPay");

                    b.Property<double>("Total");

                    b.Property<int>("YearToPayFor");

                    b.HasKey("BillId");

                    b.ToTable("Bill");
                });

            modelBuilder.Entity("korteriyhistu.Models.LogEntry", b =>
                {
                    b.Property<string>("LogEntryId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BillId");

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Message");

                    b.HasKey("LogEntryId");

                    b.HasIndex("BillId");

                    b.ToTable("LogEntry");
                });

            modelBuilder.Entity("korteriyhistu.Models.LogEntry", b =>
                {
                    b.HasOne("korteriyhistu.Models.Bill")
                        .WithMany("Logs")
                        .HasForeignKey("BillId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
