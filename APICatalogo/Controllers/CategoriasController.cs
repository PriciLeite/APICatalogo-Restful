using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using APICatalogo.Services;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork contexto, ILogger<CategoriasController> logger, IMapper mapper)
        {
            _ouf = contexto;
            _logger = logger;
            _mapper = mapper;
        }

        // /categorias/saudacao/nome
        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }


        // /Categorias/produtos
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
        {
            try
            {
                _logger.LogInformation("================ Get categorias/produtos =============== "); // Marcação do Logger.
                
                var categorias =  _ouf.CategoriaRepository.GetCategoriasProdutos().ToList(); // Detalhamento da consulta transferido pra CategoriaRepository;                
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias); // Mapeia as informações de categorias para CategoriaDTO e exibe categoriasDto;

                return categoriasDto;
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tratar a sua solicitação.");
            }
        }

        // /categorias
        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            try
            {   
                _logger.LogInformation($"================ Get /categorias =============== "); // Marcação do Logger.

                var categoria = _ouf.CategoriaRepository.Get().ToList(); // Detalhamento da consulta transferido pra Repository; 
                if (categoria == null)
                {
                    return NotFound("Lista de categorias não encontrada ou vázia.");
                }
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categoria);
                
                return categoriasDto;
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tratar a sua solicitação.");
            }

        }

        // /categorias/id
        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id, [BindRequired] string nome)
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

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);                
                return categoriaDto;
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro id={id} não encontrado ou não existe.");
            }

        }

        // /categorias
        [HttpPost]
        public ActionResult Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                _logger.LogInformation($"================ POST categorias = {categoriaDto} =============== "); // Marcação do Logger.

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _ouf.CategoriaRepository.Add(categoria); // Detalhamento da consulta transferido pra Repository;
                _ouf.commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoriaDTO);
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tratar a sua solicitação.");
            }


        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] CategoriaDTO categoriaDto)  // id + objeto tipo do tipo Categoria.
        {
            try
            {   
                _logger.LogInformation($"================ Get categorias/id = {id} PUT =============== "); // Marcação do Logger.

                if (id != categoriaDto.CategoriaId)
                {
                    _logger.LogInformation($"=============== Get categorias/id = {id} ERROR ===========");
                    return BadRequest("Id fornecido diferente para Idcategoria.");
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _ouf.CategoriaRepository.Update(categoria); // Detalhamento da consulta transferido pra Repository;
                _ouf.commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return Ok($"Categoria atualizado com sucesso!");


            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro, verifique se id={id} ou categoria={categoriaDto} realamente existem para" +
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

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
                _ouf.CategoriaRepository.Delete(categoria);
                _ouf.commit();

                return Ok($"Categoria deletado com sucesso!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   $"Ocorreu um erro id={id} não encontrado ou não existe.");
            }

        }

    }
}
