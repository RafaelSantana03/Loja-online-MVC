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
                    Nome = "Camisa Real Madrid 06/07 Manga Longa",
                    Descricao = "Camisa retrô do Real Madrid, temporada 2006/2007, uniforme reserva com mangas longas. Estilo clássico dos Galácticos.",
                    Preco = 179.90m,
                    UrlImagem = "/imagens/camisetaREALMADRID.jpg"
                },
                new Produto
                {
                    Nome = "Camisa Milan 2002 Uniforme 2",
                    Descricao = "Camisa retrô do Milan, uniforme visitante da temporada 2002. Design icônico com detalhes em preto e vermelho.",
                    Preco = 189.90m,
                    UrlImagem = "/imagens/CAMISAMILAN.jpg"
                },
                new Produto
                {
                    Nome = "Camisa PSV Eindhoven 98/99",
                    Descricao = "Uniforme retrô do PSV Eindhoven, temporada 1998/1999. Modelo clássico que marcou uma era da equipe holandesa.",
                    Preco = 179.90m,
                    UrlImagem = "/imagens/camisaPSV.jpg"
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
