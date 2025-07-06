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
                    Nome = "Camiseta Oversized Preta",
                    Descricao = "Camiseta unissex com modelagem oversized e malha premium.",
                    Preco = 159.90m,
                    UrlImagem = "/Imagens/5.jpg"

                },
                new Produto
                {
                    Nome = "Camiseta Branca Básica",
                    Descricao = "Camiseta básica 100% algodão, ideal para o dia a dia.",
                    Preco = 89.90m,
                    UrlImagem = "https://via.placeholder.com/300x200.png?text=Camiseta+Branca"
                },
                new Produto
                {
                    Nome = "Camiseta Logo Streetwear",
                    Descricao = "Modelo com estampa frontal em silk, referência ao street style.",
                    Preco = 179.90m,
                    UrlImagem = "/Imagens/5.jpg"

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
