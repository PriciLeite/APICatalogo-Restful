using APICatalogo.Models;
using APICatalogo.Repository;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
       
        private readonly IUnitOfWork _ouf;
        private readonly ILogger _logger;

        public CategoriasController(IUnitOfWork contexto, ILogger<CategoriasController> logger)
        {
            _ouf = contexto;
            _logger = logger;
        }


        // /categorias/saudacao/nome
        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }




        // /Categorias/produtos
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                _logger.LogInformation("================ Get categorias/produtos =============== "); // Marcação do Logger.

                return _ouf.CategoriaRepository.GetCategoriasProdutos().ToList(); // Detalhamento da consulta transferido pra CategoriaRepository;                
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tratar a sua solicitação.");
            }
        }




        // /categorias
        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {   
                _logger.LogInformation($"================ Get /categorias =============== "); // Marcação do Logger.

                var categoria = _ouf.CategoriaRepository.Get().ToList(); // Detalhamento da consulta transferido pra Repository; 
                if (categoria == null)
                {
                    return NotFound("Lista de categorias não encontrada ou vázia.");
                }

                return categoria;
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tratar a sua solicitação.");
            }

        }

        // /categorias/id
        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id, [BindRequired] string nome)
        {
            try
            { 
                _logger.LogInformation($"============= Get categorias/id = {id} ============= "); // Marcação do Logger.

                var nomeCategoria = nome;               
               
                var categoria = _ouf.CategoriaRepository.GetById(p => p.CategoriaId == id); // Detalhamento da consulta transferido pra Repository; 
                if (categoria == null)
                {
                    _logger.LogInformation($"========= Get categorias/id = {id} NOT FOUND ========== "); // Marcação do Logger.
                    return NotFound($"Categoria id={id} não encontrada ou não existe.");
                }

                return categoria;
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro id={id} não encontrado ou não existe.");
            }

        }




        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            try
            {
                _logger.LogInformation($"================ POST categorias = {categoria} =============== "); // Marcação do Logger.

                if (categoria is null)
                {
                    return BadRequest("Preenchimento vázio!");
                }

                _ouf.CategoriaRepository.Add(categoria); // Detalhamento da consulta transferido pra Repository;
                _ouf.commit();

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoria);
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tratar a sua solicitação.");
            }


        }



        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)  // id + objeto tipo do tipo Categoria.
        {
            try
            {   
                _logger.LogInformation($"================ Get categorias/id = {id} PUT =============== "); // Marcação do Logger.

                if (id != categoria.CategoriaId)
                {
                    return BadRequest("Id fornecido diferente para Idcategoria.");
                }

                _ouf.CategoriaRepository.Update(categoria); // Detalhamento da consulta transferido pra Repository;
                _ouf.commit();

                return Ok(categoria);

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro, verifique se id={id} ou categoria={categoria} realamente existem para" +
                    $"ser atualizados.");
            }

        }


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _logger.LogInformation($"================ Get categorias/id = {id} DELETE =============== "); // Marcação do Logger.

                var categoria = _ouf.CategoriaRepository.GetById(p => p.CategoriaId == id); // Detalhamento da consulta transferido pra Repository;

                if (categoria is null)
                {
                    return NotFound("Id não encontrado ou não existe.");
                }

                _ouf.CategoriaRepository.Delete(categoria);
                _ouf.commit();

                return Ok(categoria);

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                   $"Ocorreu um erro id={id} não encontrado ou não existe.");
            }

        }

    }
}
