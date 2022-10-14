using ConfigurationLibrary.Classes;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Classes;
using PizzaShop.Data;

namespace PizzaShop;

class Program
{
    public static async Task Main(string[] args)
    {
        // simple
        //var builder = WebApplication.CreateBuilder(args);

        // verbose
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ApplicationName = typeof(Program).Assembly.FullName,
            ContentRootPath = Directory.GetCurrentDirectory()
        });

        // Add services to the container.
        builder.Services.AddRazorPages();

       
        builder.Services.AddDbContext<PizzaContext>(options => 
            options.UseSqlServer(ConfigurationHelper.ConnectionString()));
        
        WebApplication app = builder.Build();
        
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                               ForwardedHeaders.XForwardedProto
        });
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.MapRazorPages();

        if (Environment.GetEnvironmentVariable("CREATE_DB") == "Yes")
        {
            InitializeDatabase.Clean();
            InitializeDatabase.Populate();
        }
        
        await app.RunAsync();
    }
}