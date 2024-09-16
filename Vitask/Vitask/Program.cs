using Business.Container;
using Entities.Concrete;
using DataAccessLayer.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Vitask.Statics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.ContainerDependencies();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<VitaskContext>();

builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<VitaskContext>();
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<VitaskContext>()
	 .AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();



builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(action =>
{
    action.LoginPath = "/Login/Index/";
   
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using(var scope = app.Services.CreateScope())
{
    var memoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
    CacheManager.Initialize(memoryCache);


    Extensions.SeedRoles(scope.ServiceProvider).Wait();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
