﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnifieldTech.Data;

#nullable disable

namespace UnifieldTech.Migrations
{
    [DbContext(typeof(UnifieldTechContext))]
    partial class UnifieldTechContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UnifieldTech.Celular", b =>
                {
                    b.Property<int>("CelularID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CelularID"));

                    b.Property<string>("CelularN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClienteID")
                        .HasColumnType("int");

                    b.HasKey("CelularID");

                    b.HasIndex("ClienteID");

                    b.ToTable("Celular");

                    b.HasData(
                        new
                        {
                            CelularID = 1,
                            CelularN = "35991529241",
                            ClienteID = 1
                        });
                });

            modelBuilder.Entity("UnifieldTech.Cliente", b =>
                {
                    b.Property<int>("ClienteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClienteID"));

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataNacs")
                        .HasColumnType("datetime2");

                    b.Property<string>("E_Mail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeCliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClienteID");

                    b.ToTable("Cliente");

                    b.HasData(
                        new
                        {
                            ClienteID = 1,
                            CPF = "132.318.266.93",
                            DataNacs = new DateTime(1997, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            E_Mail = "robert@gmail.com",
                            NomeCliente = "Robert",
                            Password = "123"
                        });
                });

            modelBuilder.Entity("UnifieldTech.Fazenda", b =>
                {
                    b.Property<int>("FazendaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FazendaID"));

                    b.Property<bool>("AreaMecanizada")
                        .HasColumnType("bit");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClienteID")
                        .HasColumnType("int");

                    b.Property<string>("Cultivar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hectar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Latitude")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Longitude")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeFazenda")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Num")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rua")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TipoPlantio")
                        .HasColumnType("bit");

                    b.HasKey("FazendaID");

                    b.HasIndex("ClienteID");

                    b.ToTable("Fazenda");

                    b.HasData(
                        new
                        {
                            FazendaID = 1,
                            AreaMecanizada = true,
                            Cidade = "Alfenas",
                            ClienteID = 1,
                            Cultivar = "Café",
                            Estado = "Minas Gerais",
                            Hectar = "18 He",
                            Latitude = "48",
                            Longitude = "45",
                            NomeFazenda = "Caipira",
                            Num = "1458",
                            Rua = "Primeira segunda",
                            TipoPlantio = true
                        });
                });

            modelBuilder.Entity("UnifieldTech.Celular", b =>
                {
                    b.HasOne("UnifieldTech.Cliente", "cliente")
                        .WithMany("celular")
                        .HasForeignKey("ClienteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("cliente");
                });

            modelBuilder.Entity("UnifieldTech.Fazenda", b =>
                {
                    b.HasOne("UnifieldTech.Cliente", "cliente")
                        .WithMany("fazenda")
                        .HasForeignKey("ClienteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("cliente");
                });

            modelBuilder.Entity("UnifieldTech.Cliente", b =>
                {
                    b.Navigation("celular");

                    b.Navigation("fazenda");
                });
#pragma warning restore 612, 618
        }
    }
}
