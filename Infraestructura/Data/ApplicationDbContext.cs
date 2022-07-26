using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {          
        }

        public DbSet<Compania> TbCompania { get; set; }
        public DbSet<Empleado> TbEmpleado { get; set; }
    }
}