using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
       
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger)
        {
            _context = context;
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
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        {
            try
            {
                _logger.LogInformation("================ Get categorias/produtos =============== ");

                //Marcando não monitoramento em cache    //aplicando filtro para obter objetos relacionados. 
                return await _context.Categorias.AsNoTracking().Include(c => c.Produtos).Where(c => c.CategoriaId <= 5).ToListAsync();
            }

            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tratar a sua solicitação.");
            }
        }




        // /categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
            try
            {   
                _logger.LogInformation($"================ Get /categorias =============== ");


                // Não monitoramento  // Limitando a obtenção dos registros para não sobrecarga.
                var categoria = await _context.Categorias.AsNoTracking().Take(10).ToListAsync();
                if (categoria == null)
                {
                    return NotFound("Categoria não encontrada.");
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
        public ActionResult<Categoria> Get(int id)
        {
            try
            { 
                _logger.LogInformation($"============= Get categorias/id = {id} ============= ");

                //Marcando não monitoramento em cache 
                var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    _logger.LogInformation($"========= Get categorias/id = {id} NOT FOUND ========== ");
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
        public ActionResult Post(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                {
                    return BadRequest();
                }

                _context.Categorias.Add(categoria);
                _context.SaveChanges();

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
        public ActionResult Put(int id, Categoria categoria)
        {
            try
            {   
                _logger.LogInformation($"================ Get categorias/id = {id} PUT =============== ");

                if (id != categoria.CategoriaId)
                {
                    return BadRequest("Id fornecido diferente para Idcategoria.");
                }

                _context.Categorias.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();

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
                _logger.LogInformation($"================ Get categorias/id = {id} DELETE =============== "); 

                var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound("Id não encontrado ou não existe.");
                }

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();

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
