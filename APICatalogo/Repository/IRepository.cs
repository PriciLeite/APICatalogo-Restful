using System.Linq.Expressions;

namespace APICatalogo.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> Get(); // poderá customizar a consulta;
        T? GetById(Expression<Func<T, bool>> predicate); //consultar por id;
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);


    }
}
