using MinApi.Classes;
using MinApi.Services;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<HelloService>(new HelloService());
        var app = builder.Build();
        app.MapGet("/", (HttpContext _, HelloService helloService) => 
            helloService
                .SayHello(
                    builder.Configuration.GetSection("Personal").Value)
            );

        app.Run();
    }
}
