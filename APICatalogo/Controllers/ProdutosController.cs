using APICatalogo.DTOs;
using APICatalogo.Filter;
using APICatalogo.Model;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork contexto, ILogger<ProdutosController> logger, IMapper mapper )
        {
            _ouf = contexto;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("preco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutoPorPrecos()
        {
            _logger.LogInformation("================ Get produto/preço =============== "); // Marcação do Logger.

            var produtos = _ouf.ProdutoRepository.GetProdutoPorPreco().ToList(); // obtem todos os produtos;
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos); // Mapeia as informações de produtos para ProdutoDTO e exibe ProdutoDTO;

            return produtosDto;
        }
        
        // /produtos
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            try
            {
                _logger.LogInformation("================ Get produtos =============== "); // Marcação do Logger.

                var produtos = _ouf.ProdutoRepository.Get().ToList(); // Detalhamento da consulta transferido pra Repository;
                var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos); // Mapeia as informações de produtos para ProdutoDTO e exibe ProdutoDT

                return produtosDto;
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro na obtenção dos dados...");
            }
            
        }

        // /produtos/id
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> GetProduto(int id, [BindRequired] string nome)
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

                var produtoDto = _mapper.Map<ProdutoDTO>(produto); //Mapeia as informações de produtos para ProdutoDTO e exibe ProdutoDTO;

                return produtoDto;
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro, id={id} não correspode ao nome ou não existe.");
            }
            
        }
               

        [HttpPost]
        public ActionResult Post([FromBody]ProdutoDTO produtoDto)
        {
            try
            {
                _logger.LogInformation("================ POST produto =============== "); // Marcação do Logger.

                var produto = _mapper.Map<Produto>(produtoDto); // Neste caso, Mapeamento será produtoDto para Tabela Produto;                

                _ouf.ProdutoRepository.Add(produto); // Detalhamento da consulta transferido pra Repository;
                _ouf.commit();

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto); //Mapeia as informações de produto para ProdutoDTO e exibe produtoDTO;

                return new CreatedAtRouteResult("ObterProduto",
                    new { id = produto.ProdutoId }, produtoDTO);
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro na obtenção dos dados...");
            }
            
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, ProdutoDTO produtoDto)
        {
            try
            {
                _logger.LogInformation($"================  UPDATE produto/{id}  =============== "); // Marcação do Logger.                
                

                if (id != produtoDto.ProdutoId)
                {
                    return BadRequest("Id não compatível com IdProduto");  //400
                }

                var produto = _mapper.Map<Produto>(produtoDto); // Neste caso, Mapeamento será produtoDto para Tabela Produto;

                _ouf.ProdutoRepository.Update(produto); // Detalhamento da consulta transferido pra Repository;
                _ouf.commit();

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto); //Mapeia as informações de produto para ProdutoDTO e exibe produtoDTO;


                return Ok(produtoDTO);
               
            
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro, id={id} não encontrado ou não existe.");
            }

            
        }


        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
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

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto); //Mapeia as informações de produto para ProdutoDTO e exibe produtoDTO;


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
