using LojaOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace LojaOnline.Data;

public static class SeedData
{
    public static void Inicializar(IApplicationBuilder app)
    {
        using var escopo = app.ApplicationServices.CreateScope();
        var contexto = escopo.ServiceProvider.GetRequiredService<BancoDeDados>();

        contexto.Database.Migrate(); // garante que o banco esteja atualizado

        if (contexto.Produtos.Any()) return; // já tem dados? não faz nada

        contexto.Produtos.AddRange(
            new Produto
            {
                Nome = "Mouse Gamer RGB",
                Descricao = "Mouse com sensor de alta precisão e iluminação RGB.",
                Preco = 199.90m,
                UrlImagem = "https://via.placeholder.com/300x200.png?text=Mouse+RGB"
            },
            new Produto
            {
                Nome = "Teclado Mecânico",
                Descricao = "Teclado com switches mecânicos e LED.",
                Preco = 349.90m,
                UrlImagem = "https://via.placeholder.com/300x200.png?text=Teclado+Mecânico"
            },
            new Produto
            {
                Nome = "Monitor 24'' Full HD",
                Descricao = "Tela IPS com resolução Full HD e taxa de 75Hz.",
                Preco = 899.00m,
                UrlImagem = "https://via.placeholder.com/300x200.png?text=Monitor+24"
            },
            new Produto
            {
                Nome = "Camiseta Barra Crew",
                Descricao = "Camiseta de algodão com estampa exclusiva.",
                Preco = 189.90m,
                UrlImagem = "https://via.placeholder.com/300x200.png?text=Camiseta+Barra+Crew"
            }

        );

        contexto.SaveChanges();
    }
}
