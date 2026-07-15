using HotelReservationSystem.Data;
using HotelReservationSystem.Repositories.Reservations;
using HotelReservationSystem.Repositories.Rooms;
using HotelReservationSystem.Repositories.RoomTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options => options
                            .UseSqlServer(builder.Configuration.GetConnectionString("My Connection")));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IRoomTypeRepo, RoomTypeRepo>();
            builder.Services.AddScoped<IRoomRepo,  RoomRepo>();
            builder.Services.AddScoped<IReservationRepo, ReservationRepo>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var roleManage = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roleNames = { "Admin", "User" };
                foreach (var roleName in roleNames)
                {
                    var roleExists = await roleManage.RoleExistsAsync(roleName);
                    if (!roleExists)
                        await roleManage.CreateAsync(new IdentityRole(roleName));
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                string adminEmail = configuration["AdminSettings:Email"];
                string adminPassword = configuration["AdminSettings:Password"];

                if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword))
                {
                    var adminUser = await userManager.FindByEmailAsync(adminEmail);

                    if (adminUser == null)
                    {
                        var newAdmin = new IdentityUser
                        {
                            UserName = adminEmail,
                            Email = adminEmail,
                            EmailConfirmed = true
                        };

                        var result = await userManager.CreateAsync(newAdmin, adminPassword);

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newAdmin, "Admin");
                        }
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Welcome}/{id?}")
                .WithStaticAssets();

            await app.RunAsync();
        }
    }
}
