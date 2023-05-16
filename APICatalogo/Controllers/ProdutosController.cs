using APICatalogo.Context;
using APICatalogo.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> GetProduto(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto == null)
            {
                return NotFound("Produto não encontrado.");
            }

            return Ok(produto);
        }
               

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            // Verifica se existem informações no corpo da requisição
            if (HttpContext.Request.Body.CanRead)
            {
                return BadRequest();
            }

            if (produto is null)
            {
                return BadRequest();
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest("Id não compatível com IdProduto");  //400
            }

            //Entry acessa as informações rastreadas pelo _context.
            //O objeto retornado por Entry( ) é do tipo EntiteState que fornece
            //informações sobre o estado atual da entidade e permite que você altere o estado da entidade.  
            _context.Produtos.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();      
            
            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if ( produto is null)
            {
                return NotFound("Produto não encontrado.");
            }
        
            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        
        }

        
    
    
    }
}
