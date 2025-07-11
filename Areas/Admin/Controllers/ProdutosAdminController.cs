using LojaOnline.Data;
using LojaOnline.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojaOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProdutosAdminController : Controller
    {
        private readonly BancoDeDados _context;
        private readonly IWebHostEnvironment _webHost;


        public ProdutosAdminController(BancoDeDados context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _context.Produtos.ToListAsync();
            return View(produtos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produto produto, IFormFile? novaImagem)
        {
            if (id != produto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var produtoExistente = await _context.Produtos.FindAsync(id);
                    if (produtoExistente == null) return NotFound();

                    produtoExistente.Nome = produto.Nome;
                    produtoExistente.Descricao = produto.Descricao;
                    produtoExistente.Preco = produto.Preco;

                    if (novaImagem != null && novaImagem.Length > 0)
                    {
                        var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(novaImagem.FileName);
                        var caminho = Path.Combine(_webHost.WebRootPath, "imagens", nomeArquivo);

                        using (var stream = new FileStream(caminho, FileMode.Create))
                        {
                            await novaImagem.CopyToAsync(stream);
                        }

                        produtoExistente.UrlImagem = "/imagens/" + nomeArquivo;
                    }

                    _context.Update(produtoExistente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Produtos.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(produto);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null) return NotFound();

            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
