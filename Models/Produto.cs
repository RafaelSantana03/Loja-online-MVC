namespace LojaOnline.Models;

public class Produto
{
    public int Id { get; set; }  // <- ESSA LINHA PRECISA EXISTIR
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public string? UrlImagem { get; set; }
}



