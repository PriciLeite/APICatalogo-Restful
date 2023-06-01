using APICatalogo.Context;
using APICatalogo.Filter;
using APICatalogo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        // /produtos
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            try
            {   // Não monitoramento  // Limitando a obtenção dos registros para não sobrecarga.
                var produtos = await _context.Produtos.AsNoTracking().Take(10).ToListAsync();

                if (produtos is null)
                {
                    return NotFound("Produtos não encontrado ou lista vazia.");
                }

                return produtos;
            }

            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro na obtenção dos dados...");
            }
            
        }

        // /produtos/id
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetProdutoAsync(int id, [BindRequired] string nome)
        {
            try
            {
                var nomeProduto = nome; 
                                                    // Não monitorado pelo EF
                var produto = await _context.Produtos.AsNoTracking().Where(c => c.Nome == nomeProduto).FirstOrDefaultAsync(p => p.ProdutoId == id);
                if (produto == null)
                {
                    return NotFound("Produto não encontrado.");
                }

                return produto;
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro, id={id} não correspode ao nome ou não existe.");
            }
            
        }
               

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            try
            {
                //// Verifica se existem informações no corpo da requisição
                //if (HttpContext.Request.Body.CanRead)
                //{
                //    return BadRequest();
                //}

                if (produto is null)
                {
                    return BadRequest();
                }

                _context.Produtos.Add(produto);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterProduto",
                    new { id = produto.ProdutoId }, produto);
            }

            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro na obtenção dos dados...");
            }
            
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            try
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
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro, id={id} não encontrado ou não existe.");
            }

            
        }


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

                if (produto is null)
                {
                    return NotFound("Produto não encontrado.");
                }

                _context.Produtos.Remove(produto);
                _context.SaveChanges();

                return Ok(produto);
            }

            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro, id={id} não encontrado ou não existe.");
            }
           
        
        }    
        
    }

}
