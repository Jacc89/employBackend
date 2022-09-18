using System.Linq.Expressions;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Data.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        public DbSet<T> dbSet;
        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
            
        }
        public async Task Agregar(T entidad)
        {
           await dbSet.AddRangeAsync(entidad);
        }

        public Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet;
            if(filtro !=null){
                query = query.Where(filtro);
            }
            if (incluirPropiedades != null) // compANI, CARGO, DEPARTM
            {
                foreach (var ip in incluirPropiedades.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                     query = query.Include(ip);
                    
                }
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        public void Remove(T entidad)
        {
            dbSet.Remove(entidad);
        }
    }}