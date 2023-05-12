using APICatalogo.Context;
using APICatalogo.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        // Injetar uma instância da classe AppDbContext:

        private readonly AppDbContext _context; //readonly somente leitura

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetActionResult()
        {
            var produtos = _context.Produtos;

            if (produtos is null)
            {
                return NotFound("Produtos não encontrado ou lista vazia.");
            }

            return Ok(produtos);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Produto> GetProduto(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if(produto == null)
            {
                return NotFound("Produto não encontrado.");
            }
            
            return Ok(produto);
        }

        [HttpGet("{nome}")]
        public ActionResult<Produto> GetProduto(string nome)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Nome == nome);
            if (produto == null)
            {
                return NotFound("Produto não encontrado.");
            }

            return Ok(produto);
        }


    }
}
