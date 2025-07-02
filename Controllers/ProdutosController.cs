
using Microsoft.AspNetCore.Mvc;
using LojaOnline.Models;
using LojaOnline.Data;

namespace LojaOnline.Controllers;

public class ProdutosController : Controller
{
    private readonly BancoDeDados _contexto;

    public ProdutosController(BancoDeDados contexto)
    {
        _contexto = contexto;
    }

    public IActionResult Index()
    {
        var produtos = _contexto.Produtos.ToList();
        return View(produtos);
    }
}
