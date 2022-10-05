using Infraestructura.Data.Repositorio.IRepositorio;
namespace Infraestructura.Data.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        ICompaniaRepositorio Compania {get;}
        IEmpleadoRepositorio Empleado {get; }
        Task Guardar();
        
    }
}