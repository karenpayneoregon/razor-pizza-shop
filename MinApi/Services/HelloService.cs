using MinApi.Classes;

namespace MinApi.Services;

public class HelloService
{
    public string SayHello(string name)
    {
        return $"{Howdy.TimeOfDay()} {name}";
    }
}