using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnifieldTech;

namespace UnifieldTech.Data
{
    public class UnifieldTechContext : DbContext
    {
        public UnifieldTechContext (DbContextOptions<UnifieldTechContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().HasData(GetSeedDataClientes());
            modelBuilder.Entity<Celular>().HasData(GetSeedDataCelulares());
            modelBuilder.Entity<Fazenda>().HasData(GetSeedDataFazendas());
        }

        public DbSet<UnifieldTech.Cliente> Cliente { get; set; } = default!;

        public DbSet<UnifieldTech.Celular> Celular { get; set; } = default!;

        public DbSet<UnifieldTech.Fazenda> Fazenda { get; set; } = default!;

        private Cliente[] GetSeedDataClientes() => new Cliente[] {
        new Cliente {
            ClienteID = 1,
            NomeCliente = "Robert",
            CPF = "132.318.266.93",
            E_Mail = "robert@gmail.com",
            DataNacs = Convert.ToDateTime("1997/11/11"),
            Password = "123"
        }
        };
        private Celular[] GetSeedDataCelulares() => new Celular[] {
        new Celular {
            CelularID = 1,
            CelularN = "35991529241",
            ClienteID = 1,
        }
        };
        private Fazenda[] GetSeedDataFazendas() => new Fazenda[] {
        new Fazenda {
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
            ClienteID = 1,

        }
        };
    }
}
