namespace LojaOnline.Controllers;
using LojaOnline.Data;
using LojaOnline.Extensions;
using LojaOnline.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

public class CarrinhoController : Controller
{
    private readonly BancoDeDados _contexto;
    private readonly UserManager<ApplicationUser> _userManager;
    private const string CarrinhoSessao = "Carrinho";

    public CarrinhoController(BancoDeDados contexto, UserManager<ApplicationUser> userManager)
    {
        _contexto = contexto;
        _userManager = userManager;
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

    [HttpPost]
    public async Task<IActionResult> FinalizarPedido()
    {
        var carrinho = ObterCarrinho();
        if (!carrinho.Any())
            return RedirectToAction("Index");

        var usuario = await _userManager.GetUserAsync(User);

        var pedido = new Order
        {
            UserId = usuario.Id,
            OrderDate = DateTime.Now,
            Items = carrinho.Select(item => new OrderItem
            {
                ProductId = item.ProdutoId,
                Quantity = item.Quantidade
            }).ToList()
        };

        _contexto.Orders.Add(pedido);
        await _contexto.SaveChangesAsync();

        HttpContext.Session.Remove(CarrinhoSessao);

        return RedirectToAction("Sucesso");
    }

    public IActionResult Sucesso()
    {
        return View();
    }
    private List<ItemCarrinho> ObterCarrinho()
    {
        return HttpContext.Session.GetObjetoComoJson<List<ItemCarrinho>>(CarrinhoSessao) ?? new List<ItemCarrinho>();
    }

    private void SalvarCarrinho(List<ItemCarrinho> carrinho)
    {
        HttpContext.Session.SetObjetoComoJson(CarrinhoSessao, carrinho);
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        var carrinho = ObterCarrinho();
        return View("Checkout", carrinho);
    }

    [HttpPost]
    public IActionResult ConfirmarCheckout()
    {
        var carrinho = ObterCarrinho();

        // Gera QR Code Pix simulando uma chave aleatória
        var total = carrinho.Sum(i => i.Preco * i.Quantidade);
        string chavePix = "rafael@pix.com.br";
        string nome = "RetroStore";
        string cidade = "São Paulo";
        string valor = total.ToString("F2").Replace(",", ".");

        string payloadPix = GerarPayloadPix(chavePix, nome, cidade, valor);
        string qrCodeBase64 = GerarQrCode(payloadPix);

        ViewBag.QrCodeBase64 = qrCodeBase64;
        ViewBag.Valor = valor;

        return View("PixPagamento");
    }
    private string GerarPayloadPix(string chave, string nome, string cidade, string valor)
    {
        return $"00020126360014BR.GOV.BCB.PIX0114{chave}520400005303986540{valor.Length}{valor}5802BR5913{nome}6009{cidade}62070503***6304";
    }
    private string GerarQrCode(string payload)
    {
        using var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new Base64QRCode(qrCodeData);
        return qrCode.GetGraphic(10);
    }

    [HttpPost]
    public IActionResult ConfirmarPagamento()
    {
        HttpContext.Session.Remove(CarrinhoSessao);
        return RedirectToAction("Sucesso");
    }
    [HttpPost]
    public IActionResult ConfirmarEndereco(string nome, string endereco, string cidade, string estado, string cep)
    {
        var carrinho = ObterCarrinho();
        var total = carrinho.Sum(i => i.Preco * i.Quantidade);
        string chavePix = "rafael@pix.com.br";
        string valor = total.ToString("F2").Replace(",", ".");

        string payloadPix = GerarPayloadPix(chavePix, nome, cidade, valor);
        string qrCodeBase64 = GerarQrCode(payloadPix);

        ViewBag.QrCodeBase64 = qrCodeBase64;
        ViewBag.Valor = valor;

        ViewBag.Nome = nome;
        ViewBag.Endereco = endereco;
        ViewBag.Cidade = cidade;
        ViewBag.Estado = estado;
        ViewBag.Cep = cep;

        return View("PixPagamento");
    }




}
