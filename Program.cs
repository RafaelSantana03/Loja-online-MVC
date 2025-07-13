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

// Identity com ApplicationUser e RoleManager
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<BancoDeDados>()
.AddDefaultTokenProviders()
.AddErrorDescriber<LojaOnline.Utils.IdentityErrorDescriberPtBr>();

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

// Rota para áreas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


// Rota padrão para Controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Razor Pages (para páginas de login/registro)
app.MapRazorPages();

// Seed de dados
SeedData.Inicializar(app);

// Criação do papel "Admin" e usuário admin padrão
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    await SeedRolesAndAdminUser(roleManager, userManager);
}

async Task SeedRolesAndAdminUser(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
{
    var roleExists = await roleManager.RoleExistsAsync("Admin");
    if (!roleExists)
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    var adminEmail = "admin@retroshop.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var user = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}

app.Run();
