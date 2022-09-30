using Spectre.Console;

namespace DemoReverseEngineer;

internal class Program
{
    static async Task Main(string[] args)
    {
        await using var context = new PizzaContext();
        List<Customers> list = await context.Customers.ToListAsync();
        Table table = CreateTable();
        foreach (var customer in list)
        {
            table.AddRow(customer.FirstName, customer.LastName);
        }

        AnsiConsole.Write(table);
        Console.ReadLine();
    }

    [ModuleInitializer]
    public static void Init()
    {
        Console.Title = "Code sample: Getting to know Visual Studio and C#";
        SetConsoleWindowPosition(AnchorWindow.Center);
    }
    private static Table CreateTable()
    {
        return new Table()
            .RoundedBorder().BorderColor(Color.LightSlateGrey)
            .AddColumn("[b]First[/]")
            .AddColumn("[b]Last[/]")
            .Alignment(Justify.Center)
            .Title("[white on blue]Customers[/]");
    }
}