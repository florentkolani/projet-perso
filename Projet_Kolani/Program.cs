////using Projet_Kolani.Data;

////var builder = WebApplication.CreateBuilder(args);


////// Add services to the container.
////builder.Services.AddControllersWithViews();
////builder.Services.AddSingleton<Projet_KolaniDbContext>();

////var app = builder.Build();

////// Configure the HTTP request pipeline.
////if (!app.Environment.IsDevelopment())
////{
////    app.UseExceptionHandler("/Home/Error");
////    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
////    app.UseHsts();
////}

////app.UseHttpsRedirection();
////app.UseStaticFiles();

////app.UseRouting();

////app.UseAuthorization();

////app.MapControllerRoute(
////    name: "default",
////    pattern: "{controller=Home}/{action=Index}/{id?}");

////app.Run();

//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Projet_Kolani.Data;


//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddSingleton<Projet_KolaniDbContext>();

//// Add ASP.NET Core Identity services
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<Projet_KolaniDbContext>()
//    .AddDefaultTokenProviders();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//// Add authentication and authorization middleware
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Account}/{action=Login}/{id?}");



//app.Run();

//// Seed default user
//await CreateDefaultUserAsync(app.Services);
//// Seed default user

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "login",
//        pattern: "login",
//        defaults: new { controller = "Account", action = "Login" });

//    endpoints.MapControllerRoute(
//        name: "logout",
//        pattern: "logout",
//        defaults: new { controller = "Account", action = "Logout" });

//    // ... Other routes ...

//    // This route should catch any remaining requests and redirect to the default route.
//    endpoints.MapControllerRoute(
//        name: "home",
//        pattern: "{controller=Home}/{action=Index}/{id?}");
//});

//async Task CreateDefaultUserAsync(IServiceProvider serviceProvider)
//{
//    using (var scope = serviceProvider.CreateScope())
//    {
//        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
//        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//        var dbContext = scope.ServiceProvider.GetRequiredService<Projet_KolaniDbContext>();

//        // Ensure that the database is created
//        dbContext.Database.EnsureCreated();

//        // Check if the default user already exists
//       // if (!dbContext.Users.Any())
//            if (!await userManager.Users.AnyAsync())
//            {
//            string nomUtilisateur = "florent";
//            string email = "flotte@example.com";
//            string motDePasse = "Florent@1234";

//            // Check if the "Admin" role exists, create if not
//            if (await roleManager.FindByNameAsync("Admin") == null)
//            {
//                await roleManager.CreateAsync(new IdentityRole("Admin"));
//            }

//            // Create and add the default user
//            IdentityUser utilisateur = new IdentityUser
//            {
//                UserName = nomUtilisateur,
//                Email = email,
//            };

//            IdentityResult resultat = await userManager.CreateAsync(utilisateur, motDePasse);

//            if (resultat.Succeeded)
//            {
//                // Add the default user to the "Admin" role
//                await userManager.AddToRoleAsync(utilisateur, "Admin");

//                // Save changes to the database
//                await dbContext.SaveChangesAsync();

//                Console.WriteLine("Default User Saved Successfully...");
//            }
//            else
//            {
//                Console.WriteLine("User creation failed. Errors:");
//                foreach (var error in resultat.Errors)
//                {
//                    Console.WriteLine($"- {error.Description}");
//                }
//            }
//        }       }
//}




using Projet_Kolani.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<Projet_KolaniDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<Projet_KolaniDbContext>()
        .AddDefaultTokenProviders();
builder.Services.AddScoped<SignInManager<IdentityUser>>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Add this line for authentication
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Seed default user
await CreerUtilisateurParDefaut(app.Services);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "login",
        pattern: "login",
        defaults: new { controller = "Account", action = "Login" });

    endpoints.MapControllerRoute(
        name: "logout",
        pattern: "logout",
        defaults: new { controller = "Account", action = "Logout" });

    // ... Other routes ...

    // This route should catch any remaining requests and redirect to the default route.
    endpoints.MapControllerRoute(
        name: "home",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();

async Task CreerUtilisateurParDefaut(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var scopedServices = scope.ServiceProvider;
        try
        {
            var userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = scopedServices.GetRequiredService<Projet_KolaniDbContext>();

            // Ensure that the database is created
            dbContext.Database.EnsureCreated();

            // Check if the default user already exists
            //if (!dbContext.Users.AnyAsync())
                if (!await userManager.Users.AnyAsync())
                {
                string nomUtilisateur = "florent";
                string email = "flotte@example.com";
                string motDePasse = "Florent446.com";

                // Check if the "Admin" role exists, create if not
                if (await roleManager.FindByNameAsync("Admin") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // Create and add the default user
                IdentityUser utilisateur = new IdentityUser
                {
                    UserName = nomUtilisateur,
                    Email = email,
                };

                IdentityResult resultat = await userManager.CreateAsync(utilisateur, motDePasse);

                if (resultat.Succeeded)
                {
                    // Add the default user to the "Admin" role
                    await userManager.AddToRoleAsync(utilisateur, "Admin");

                    // Save changes to the database
                    await dbContext.SaveChangesAsync();

                    Console.WriteLine("Default User Saved Successfully...");
                }
                else
                {
                    Console.WriteLine("User creation failed. Errors:");
                    foreach (var error in resultat.Errors)
                    {
                        Console.WriteLine($"- {error.Description}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var logger = scopedServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while creating the default user.");
        }
    }
}

