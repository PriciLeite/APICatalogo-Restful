using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProdutoRepository _produtoRepository;
        private CategoriaRepository _categoriaRepository; // Definindo instâncias de ProdutoRepository e 
        private AppDbContext _context;                      // CategoriaRepository e DbContext

        public UnitOfWork(AppDbContext contexto)
        {
            _context = contexto;
        }


        public IProdutoRepository ProdutoRepository
        {
            get
            {
                // Se a instância for null ------------------  passa uma instância do contexto:
                return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_context);
            }

        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                // Se a instância for null ------------------  passa uma instância do contexto:
                return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_context);
            }
        }

        public void commit()
        {
            _context.SaveChanges();
        }

        public void Dispose() // liberando os recursos usados na injeção;
        {
            _context.Dispose();
        }



    }
}
