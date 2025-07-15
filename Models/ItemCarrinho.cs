namespace LojaOnline.Models
{
    public class ItemCarrinho
    {
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public string? Nome { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public string? UrlImagem { get; set; }
    }
}
