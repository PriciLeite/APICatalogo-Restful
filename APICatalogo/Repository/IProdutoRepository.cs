using APICatalogo.Model;

namespace APICatalogo.Repository
{
    //Interface específica para produto herda a interfa genérica implementando o tipo Produto:
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutoPorPreco(); // obtendo os produtos pelo preço;
    }

}
