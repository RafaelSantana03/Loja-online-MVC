namespace LojaOnline.Controllers;
using Microsoft.AspNetCore.Mvc;
using LojaOnline.Models;
using LojaOnline.Extensions;
using LojaOnline.Data;

public class CarrinhoController : Controller
{
    private readonly BancoDeDados _contexto;
    private const string CarrinhoSessao = "Carrinho";

    public CarrinhoController(BancoDeDados contexto)
    {
        _contexto = contexto;
    }

    public IActionResult Index()
    {
        var carrinho = ObterCarrinho();
        return View(carrinho);
    }

    [HttpPost]
    public IActionResult Adicionar(int produtoId)
    {
        var produto = _contexto.Produtos.FirstOrDefault(p => p.Id == produtoId);
        if (produto == null) return NotFound();

        var carrinho = ObterCarrinho();

        var itemExistente = carrinho.FirstOrDefault(p => p.ProdutoId == produtoId);
        if (itemExistente != null)
        {
            itemExistente.Quantidade++;
        }
        else
        {
            carrinho.Add(new ItemCarrinho
            {
                ProdutoId = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                Quantidade = 1,
                UrlImagem = produto.UrlImagem
            });
        }

        SalvarCarrinho(carrinho);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Remover(int produtoId)
    {
        var carrinho = ObterCarrinho();
        var item = carrinho.FirstOrDefault(x => x.ProdutoId == produtoId);
        if (item != null)
        {
            carrinho.Remove(item);
            SalvarCarrinho(carrinho);
        }
        return RedirectToAction("Index");
    }

    private List<ItemCarrinho> ObterCarrinho()
    {
        return HttpContext.Session.GetObjetoComoJson<List<ItemCarrinho>>(CarrinhoSessao) ?? new List<ItemCarrinho>();
    }

    private void SalvarCarrinho(List<ItemCarrinho> carrinho)
    {
        HttpContext.Session.SetObjetoComoJson(CarrinhoSessao, carrinho);
    }
}
