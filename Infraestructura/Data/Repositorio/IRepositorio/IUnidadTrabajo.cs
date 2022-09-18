using Infraestructura.Data.Repositorio.IRepositorio;
namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public class IUnidadTrabajo : IDisposable
    {
        ICompaniaRepositorio Compania {get;}
        IEmpleadoRepositorio Empleado {get; }
        Task Guardar();
        
    }
}