using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;

namespace Infraestructura.Data.Repositorio
{
    public class EmpleadoRepositorio : Repositorio<Empleado>, IEmpleadoRepositorio
    {
        private readonly ApplicationDbContext _db;
        public EmpleadoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Empleado empleado)
        {
           var empleadoDB = _db.TbEmpleado.FirstOrDefault(c=> c.Id == empleado.Id);
            if (empleadoDB != null)
            {
                empleadoDB.Apellidos = empleado.Apellidos;
                empleadoDB.Nombres = empleado.Nombres;
                empleadoDB.Cargo = empleado.Cargo;
                empleadoDB.CompaniaId = empleado.CompaniaId;
                _db.SaveChanges();
            }
        }
    }
}