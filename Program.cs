using LojaOnline.Models;
using LojaOnline.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVIÇOS --------------------

// MVC Controllers e Views
builder.Services.AddControllersWithViews();

// Conexão com banco de dados SQL Server
builder.Services.AddDbContext<BancoDeDados>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity com ApplicationUser
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<BancoDeDados>();

// Sessão
builder.Services.AddSession();

// Razor Pages (necessário para Identity funcionar)
builder.Services.AddRazorPages();


// -------------------- PIPELINE --------------------

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Sessão vem antes da autenticação
app.UseAuthentication(); // Autenticação
app.UseAuthorization();  // Autorização

// Rota padrão para Controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Razor Pages (para páginas de login/registro)
app.MapRazorPages();

// Seed de dados (opcional, se existir)
SeedData.Inicializar(app);

app.Run();
