using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;

namespace Infraestructura.Data.Repositorio
{
    public class CompaniaRepositorio : Repositorio<Compania>, ICompaniaRepositorio
    {
        private readonly ApplicationDbContext _db;
        public CompaniaRepositorio(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void Actualizar(Compania compania)
        {
            var companiaDB = _db.TbCompania.FirstOrDefault(c=> c.Id == compania.Id);
            if (companiaDB != null)
            {
                companiaDB.NombreCompania = compania.NombreCompania;
                companiaDB.Dirreccion = compania.Dirreccion;
                companiaDB.Telefono = compania.Telefono;
                companiaDB.Telefono2 = compania.Telefono2;
            }
        }
    }
}