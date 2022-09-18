using Core.Models;

namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface ICompaniaRepositorio : IRepositorio<Compania>
    {
        void Actualizar(Compania compania);
    }
}