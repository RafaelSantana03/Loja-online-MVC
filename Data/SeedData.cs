using LojaOnline.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LojaOnline.Data;

public static class SeedData
{
    public static void Inicializar(IApplicationBuilder app)
    {
        using var escopo = app.ApplicationServices.CreateScope();
        var contexto = escopo.ServiceProvider.GetRequiredService<BancoDeDados>();

        var loggerFactory = escopo.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("SeedData");

        try
        {
            contexto.Database.Migrate();

            if (contexto.Produtos.Any())
            {
                logger.LogInformation("O banco já contém produtos. Seed ignorado.");
                return;
            }

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
            logger.LogInformation("Produtos adicionados com sucesso!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao executar o SeedData.");
        }
    }
}
