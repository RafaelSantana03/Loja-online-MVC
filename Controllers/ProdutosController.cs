using Microsoft.AspNetCore.Mvc;
using LojaOnline.Models;
using LojaOnline.Data;
using Microsoft.EntityFrameworkCore;

namespace LojaOnline.Controllers;

public class ProdutosController : Controller
{
    private readonly BancoDeDados _contexto;

    public ProdutosController(BancoDeDados contexto)
    {
        _contexto = contexto;
    }

    public async Task<IActionResult> Index(string ordenacao, int pagina = 1, int tamanhoPagina = 8)
    {
        var produtosQuery = _contexto.Produtos.AsQueryable();

        // Ordenação
        produtosQuery = ordenacao switch
        {
            "preco_asc" => produtosQuery.OrderBy(p => p.Preco),
            "preco_desc" => produtosQuery.OrderByDescending(p => p.Preco),
            "nome_asc" => produtosQuery.OrderBy(p => p.Nome),
            "nome_desc" => produtosQuery.OrderByDescending(p => p.Nome),
            _ => produtosQuery.OrderBy(p => p.Id)
        };

        var totalProdutos = await produtosQuery.CountAsync();
        var produtos = await produtosQuery
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalProdutos / tamanhoPagina);
        ViewBag.PaginaAtual = pagina;
        ViewBag.Ordenacao = ordenacao;

        return View(produtos);
    }
}
