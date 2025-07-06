namespace LojaOnline.Data;
using LojaOnline.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class BancoDeDados : IdentityDbContext<ApplicationUser>
{
    public BancoDeDados(DbContextOptions<BancoDeDados> options) : base(options) { }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<LojaOnline.Models.Order> Orders { get; set; }
    public DbSet<LojaOnline.Models.OrderItem> OrderItems { get; set; }
}
