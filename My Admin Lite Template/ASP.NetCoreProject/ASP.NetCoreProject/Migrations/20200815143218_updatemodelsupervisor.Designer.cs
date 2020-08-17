﻿// <auto-generated />
using System;
using ASP.NetCoreProject.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASP.NetCoreProject.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20200815143218_updatemodelsupervisor")]
    partial class updatemodelsupervisor
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ASP.NetCoreProject.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("TB_M_Admin");
                });

            modelBuilder.Entity("ASP.NetCoreProject.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("TB_M_Department");
                });

            modelBuilder.Entity("ASP.NetCoreProject.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("NIP");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("TB_M_Employee");
                });

            modelBuilder.Entity("ASP.NetCoreProject.Models.Form", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Duration");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDate");

                    b.Property<int?>("departmentId");

                    b.Property<int?>("employeeId");

                    b.Property<int?>("supervisorId");

                    b.HasKey("Id");

                    b.HasIndex("departmentId");

                    b.HasIndex("employeeId");

                    b.HasIndex("supervisorId");

                    b.ToTable("TB_M_Form");
                });

            modelBuilder.Entity("ASP.NetCoreProject.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("validationId");

                    b.HasKey("Id");

                    b.HasIndex("validationId");

                    b.ToTable("TB_M_Report");
                });

            modelBuilder.Entity("ASP.NetCoreProject.Models.Supervisor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("TB_M_Supervisor");
                });

            modelBuilder.Entity("ASP.NetCoreProject.Models.Validation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action");

                    b.Property<int?>("formId");

                    b.Property<int?>("supervisorId");

                    b.HasKey("Id");

                    b.HasIndex("formId");

                    b.HasIndex("supervisorId");

                    b.ToTable("TB_M_Validation");
                });

            modelBuilder.Entity("ASP.NetCoreProject.Models.Form", b =>
                {
                    b.HasOne("ASP.NetCoreProject.Models.Department", "department")
                        .WithMany()
                        .HasForeignKey("departmentId");

                    b.HasOne("ASP.NetCoreProject.Models.Employee", "employee")
                        .WithMany()
                        .HasForeignKey("employeeId");

                    b.HasOne("ASP.NetCoreProject.Models.Supervisor", "supervisor")
                        .WithMany()
                        .HasForeignKey("supervisorId");
                });

            modelBuilder.Entity("ASP.NetCoreProject.Models.Report", b =>
                {
                    b.HasOne("ASP.NetCoreProject.Models.Validation", "validation")
                        .WithMany()
                        .HasForeignKey("validationId");
                });

            modelBuilder.Entity("ASP.NetCoreProject.Models.Validation", b =>
                {
                    b.HasOne("ASP.NetCoreProject.Models.Form", "form")
                        .WithMany()
                        .HasForeignKey("formId");

                    b.HasOne("ASP.NetCoreProject.Models.Supervisor", "supervisor")
                        .WithMany()
                        .HasForeignKey("supervisorId");
                });
#pragma warning restore 612, 618
        }
    }
}
