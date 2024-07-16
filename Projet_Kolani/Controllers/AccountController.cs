using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet_Kolani.Data;
using Projet_Kolani.Models;

namespace Projet_Kolani.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();
        }


        // Seed default user
        //await CreateDefaultUserAsync(app.Services);

        //async Task CreateDefaultUserAsync(IServiceProvider serviceProvider)
        //{
        //    using (var scope = serviceProvider.CreateScope())
        //    {
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        //        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //        var dbContext = scope.ServiceProvider.GetRequiredService<Projet_KolaniDbContext>();

        //        // Ensure that the database is created
        //        await dbContext.Database.EnsureCreatedAsync();

        //        // Check if the default user already exists
        //        if (!await userManager.Users.AnyAsync())
        //        {
        //            // Check if the "Admin" role exists, create if not
        //            if (!await roleManager.RoleExistsAsync("Admin"))
        //            {
        //                await roleManager.CreateAsync(new IdentityRole("Admin"));
        //            }

        //            // Create and add the default user
        //            var defaultUser = new IdentityUser { UserName = "admin", Email = "admin@example.com" };
        //            var result = await userManager.CreateAsync(defaultUser, "Admin@123");

        //            if (result.Succeeded)
        //            {
        //                // Add the default user to the "Admin" role
        //                await userManager.AddToRoleAsync(defaultUser, "Admin");
        //            }
        //        }
        //    }
        //}



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            // Vérifier si le modèle est valide avant de procéder à l'authentification
            if (ModelState.IsValid)
            {
                try
                {
                    // Votre logique d'authentification ici en utilisant SignInManager
                    var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Rediriger l'utilisateur vers la page souhaitée après la connexion réussie
                        return RedirectToAction("Index", "Home");
                    }

                    // Gestion des erreurs de connexion
                    ModelState.AddModelError(string.Empty, "Échec de la connexion");
                }
                catch (Exception ex)
                {
                    // Loggez l'exception pour déterminer la cause de l'erreur
                    // Vous pouvez utiliser un système de logging comme Serilog, NLog, etc.
                    // Exemple avec la console
                    Console.WriteLine($"Une exception s'est produite : {ex}");
                }
            }

            // Si le modèle n'est pas valide ou en cas d'échec de connexion, rester sur la page de connexion
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
  
    } 
}
