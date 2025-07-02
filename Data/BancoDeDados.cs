namespace LojaOnline.Data;
using LojaOnline.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class BancoDeDados : IdentityDbContext<ApplicationUser>
{
    public BancoDeDados(DbContextOptions<BancoDeDados> options) : base(options) { }

    public DbSet<Produto> Produtos { get; set; }
}
