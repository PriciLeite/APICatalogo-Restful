using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>>GetActionResult()
        {
            var categoria = _context.Categorias.ToList();
            if(categoria == null)
            {
                return NotFound("Categoria não encontrada.");
            }
            
            return Ok(categoria);
        
        }
    

    
    
    }
}
