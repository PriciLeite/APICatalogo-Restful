using APICatalogo.Filter;
using APICatalogo.Model;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        // Injetar uma instância da interface IUnitOfWork - Padrão Unit Of Work:
        private readonly IUnitOfWork _ouf; //readonly somente leitura
        private readonly ILogger _logger;

        public ProdutosController(IUnitOfWork contexto, ILogger<ProdutosController> logger )
        {
            _ouf = contexto;
            _logger = logger;
        }

        [HttpGet("preco")]
        public ActionResult<IEnumerable<Produto>> GetProdutoPorPrecos()
        {
            _logger.LogInformation("================ Get produto/preço =============== "); // Marcação do Logger.

            return _ouf.ProdutoRepository.GetProdutoPorPreco().ToList();
        }

        // /produtos
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                _logger.LogInformation("================ Get produtos =============== "); // Marcação do Logger.

                var produtos = _ouf.ProdutoRepository.Get().ToList(); // Detalhamento da consulta transferido pra Repository;                
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
        public ActionResult<Produto> GetProduto(int id, [BindRequired] string nome)
        {
            try
            {
                _logger.LogInformation($"================ Get produtos/{id}?nome={nome}&valor=true =============== "); // Marcação do Logger.

                var nomeProduto = nome; 
                var produto = _ouf.ProdutoRepository.GetById(p => p.ProdutoId == id); // Detalhamento da consulta transferido pra Repository;

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
        public ActionResult Post([FromBody]Produto produto)
        {
            try
            {
                _logger.LogInformation("================ POST produto =============== "); // Marcação do Logger.

                if (produto is null)
                {
                    return BadRequest("Preenchimento vázio!");
                } 

                _ouf.ProdutoRepository.Add(produto); // Detalhamento da consulta transferido pra Repository;
                _ouf.commit();

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
                _logger.LogInformation($"================  UPDATE produto/{id}  =============== "); // Marcação do Logger.

                if (id != produto.ProdutoId)
                {
                    return BadRequest("Id não compatível com IdProduto");  //400
                }
 
                _ouf.ProdutoRepository.Update(produto); // Detalhamento da consulta transferido pra Repository;
                _ouf.commit();

                return Ok(produto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro, id={id} não encontrado ou não existe.");
            }

            
        }


        [HttpDelete("{id:int}")]
        public ActionResult<Produto> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"================  DELETE produto/{id}  =============== "); // Marcação do Logger.

                var produto = _ouf.ProdutoRepository.GetById(p => p.ProdutoId == id);

                if (produto is null)
                {
                    return NotFound("Produto não encontrado.");
                }

                _ouf.ProdutoRepository.Delete(produto);
                _ouf.commit();

                return Ok($"Produto id = {id} deletado com sucesso!");
            }

            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro, id={id} não encontrado ou não existe.");
            }
           
        
        }    
        
    }

}
