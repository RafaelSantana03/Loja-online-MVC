using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LojaOnline.Data;
using LojaOnline.Models;
using LojaOnline.Extensions;
using System.Threading.Tasks;
using System.Linq;

namespace LojaOnline.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly BancoDeDados _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CheckoutController(BancoDeDados context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Checkout
        public IActionResult Index()
        {
            var carrinho = HttpContext.Session.GetObjetoComoJson<List<ItemCarrinho>>("Carrinho") ?? new List<ItemCarrinho>();
            return View(carrinho);
        }

        // POST: /Checkout/Confirmar
        [HttpPost]
        public async Task<IActionResult> Confirmar()
        {
            var carrinho = HttpContext.Session.GetObjetoComoJson<List<ItemCarrinho>>("Carrinho") ?? new List<ItemCarrinho>();
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

            _context.Orders.Add(pedido);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Carrinho");

            return RedirectToAction("Sucesso");
        }

        public IActionResult Sucesso()
        {
            return View();
        }
    }
}