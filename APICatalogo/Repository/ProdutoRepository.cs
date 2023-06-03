using APICatalogo.Context;
using APICatalogo.Model;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {

        }

        public IEnumerable<Produto> GetProdutoPorPreco()
        {
            return Get().OrderBy(p => p.Preco).ToList();
        }
    
    }

}
