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
        public async Task<IActionResult> Create(Produto produto, IFormFile? imagem)
        {
            if (ModelState.IsValid)
            {
                if (imagem != null && imagem.Length > 0)
                {
                    var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                    var extensao = Path.GetExtension(imagem.FileName).ToLowerInvariant();

                    if (!extensoesPermitidas.Contains(extensao))
                    {
                        ModelState.AddModelError("UrlImagem", "Formato de imagem inválido. Use JPG, JPEG, PNG ou WEBP.");
                        return View(produto);
                    }

                    var nomeArquivo = Guid.NewGuid().ToString() + extensao;
                    var caminho = Path.Combine(_webHost.WebRootPath, "imagens", nomeArquivo);

                    using (var stream = new FileStream(caminho, FileMode.Create))
                    {
                        await imagem.CopyToAsync(stream);
                    }

                    produto.UrlImagem = "/imagens/" + nomeArquivo;
                }

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
                        var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                        var extensao = Path.GetExtension(novaImagem.FileName).ToLowerInvariant();

                        if (!extensoesPermitidas.Contains(extensao))
                        {
                            ModelState.AddModelError("UrlImagem", "Formato de imagem inválido. Use JPG, JPEG, PNG ou WEBP.");
                            return View(produto);
                        }

                        var nomeArquivo = Guid.NewGuid().ToString() + extensao;
                        var caminho = Path.Combine(_webHost.WebRootPath, "imagens", nomeArquivo);

                        using (var stream = new FileStream(caminho, FileMode.Create))
                        {
                            await novaImagem.CopyToAsync(stream);
                        }

                        // ❗ opcional: excluir imagem antiga aqui se quiser
                        // if (!string.IsNullOrEmpty(produtoExistente.UrlImagem) && produtoExistente.UrlImagem.StartsWith("/imagens/"))
                        // {
                        //     var caminhoAntigo = Path.Combine(_webHost.WebRootPath, produtoExistente.UrlImagem.TrimStart('/'));
                        //     if (System.IO.File.Exists(caminhoAntigo)) System.IO.File.Delete(caminhoAntigo);
                        // }

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

            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
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
                // ❗ Remover imagem física associada
                if (!string.IsNullOrEmpty(produto.UrlImagem) && produto.UrlImagem.StartsWith("/imagens/"))
                {
                    var caminhoImagem = Path.Combine(_webHost.WebRootPath, produto.UrlImagem.TrimStart('/'));
                    if (System.IO.File.Exists(caminhoImagem))
                    {
                        System.IO.File.Delete(caminhoImagem);
                    }
                }

                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
