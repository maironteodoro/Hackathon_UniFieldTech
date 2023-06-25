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

        public DbSet<Fazenda> Fazenda { get; set; } = default!;
    }
}
