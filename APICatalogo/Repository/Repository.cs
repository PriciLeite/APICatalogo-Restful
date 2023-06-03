using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatalogo.Repository
{
    public class Repository<T> : IRepository<T> where T : class // só pode implementar classe
    {
        protected AppDbContext _context;

        public Repository(AppDbContext contexto)
        {
            _context = contexto;
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>().AsNoTracking(); // Desabilita o rastreamento de entidade do E.F. Core
        }                                              // Set<T> -> retorna uma instância DbSet<T> para o acesso a entidades de
                                                       // determinado tipo no Contexto;

        public T? GetById(Expression<Func<T, bool>> predicate) // obtendo por id usando o delegate Func -> comparar o id do Produto ou Categoria como critério;
        {
            return _context.Set<T>().SingleOrDefault(predicate); // bool -> validará o critério se é False/True. 
        }

        public void Add(T entity)
        {
             _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified; // Indicando que o State foi Modified;
            _context.Set<T>().Update(entity); // Em seguida usa Update para atualiza-lá;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    
    }

}
