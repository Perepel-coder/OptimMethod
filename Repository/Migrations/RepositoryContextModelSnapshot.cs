﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository;

#nullable disable

namespace Repository.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    partial class RepositoryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("Models.DescriptionTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Models.Method", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsRealized")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MethodsOptimization");
                });

            modelBuilder.Entity("Models.Parameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Notation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UnitOfMeasId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UnitOfMeasId");

                    b.ToTable("Parameters");
                });

            modelBuilder.Entity("Models.TaskParameterValue", b =>
                {
                    b.Property<int>("DescriptionTaskId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParameterId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Value")
                        .HasColumnType("REAL");

                    b.HasKey("DescriptionTaskId", "ParameterId");

                    b.HasIndex("ParameterId");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("Models.UnitOfMeas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UnitsOfMeas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "м"
                        },
                        new
                        {
                            Id = 2,
                            Name = "кг/м^3"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Дж/(кг·°С)"
                        },
                        new
                        {
                            Id = 4,
                            Name = "°С"
                        },
                        new
                        {
                            Id = 5,
                            Name = "м/с"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Па·с^n"
                        },
                        new
                        {
                            Id = 7,
                            Name = "1/°С"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Вт/(м2·°С)"
                        },
                        new
                        {
                            Id = 9,
                            Name = "-"
                        });
                });

            modelBuilder.Entity("Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Login = "admin",
                            Password = "admin",
                            Role = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Login = "user",
                            Password = "user",
                            Role = "user"
                        });
                });

            modelBuilder.Entity("Models.Parameter", b =>
                {
                    b.HasOne("Models.UnitOfMeas", "UnitOfMeas")
                        .WithMany("Parameters")
                        .HasForeignKey("UnitOfMeasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UnitOfMeas");
                });

            modelBuilder.Entity("Models.TaskParameterValue", b =>
                {
                    b.HasOne("Models.DescriptionTask", "DescriptionTask")
                        .WithMany("Values")
                        .HasForeignKey("DescriptionTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Parameter", "Parameter")
                        .WithMany("Values")
                        .HasForeignKey("ParameterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DescriptionTask");

                    b.Navigation("Parameter");
                });

            modelBuilder.Entity("Models.DescriptionTask", b =>
                {
                    b.Navigation("Values");
                });

            modelBuilder.Entity("Models.Parameter", b =>
                {
                    b.Navigation("Values");
                });

            modelBuilder.Entity("Models.UnitOfMeas", b =>
                {
                    b.Navigation("Parameters");
                });
#pragma warning restore 612, 618
        }
    }
}
