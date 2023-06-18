using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UnifieldTech.Models;

namespace UnifieldTech.Data
{
    public class UnifieldTechContext : IdentityDbContext<IdentityUser>
    {
        public UnifieldTechContext(DbContextOptions<UnifieldTechContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Cliente { get; set; } = default!;

        public DbSet<Celular> Celular { get; set; } = default!;

        public DbSet<Fazenda> Fazenda { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Celular>().HasData(
                new Celular
                {
                    CelularID = 1,
                    CelularN = "35991529241",
                    ClienteID = 1
                }
            );

            modelBuilder.Entity<Cliente>().HasData(
                new Cliente
                {
                    ClienteID = 1,
                    NomeCliente = "Robert",
                    CPF = "132.318.266.93",
                    E_Mail = "robert@gmail.com",
                    DataNacs = new DateTime(1997, 11, 11),
                    Password = "123"
                }
            );

            modelBuilder.Entity<Fazenda>().HasData(
                new Fazenda
                {
                    FazendaID = 1,
                    NomeFazenda = "Caipira",
                    Hectar = "18 He",
                    Cultivar = "Café",
                    Rua = "Primeira segunda",
                    Num = "1458",
                    Cidade = "Alfenas",
                    Estado = "Minas Gerais",
                    Latitude = "48",
                    Longitude = "45",
                    TipoPlantio = true,
                    AreaMecanizada = true,
                    ClienteID = 1
                }
            );
        }
    }
}
