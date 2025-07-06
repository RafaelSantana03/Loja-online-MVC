namespace LojaOnline.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Produto
{
    public int Id { get; set; }

    [Required]
    public string? Nome { get; set; }
    public string? Descricao { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Preco { get; set; }
    public string? UrlImagem { get; set; }
}



