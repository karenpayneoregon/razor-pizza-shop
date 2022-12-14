# About

This is a basic ASP.NET Core [^1] project using Razor Pages [^2] using Entity Framework Core 6 [^3] using Microsoft SQL-Server database.

# Heath Check

See Program.cs, code for health check is in two regions.

## Requires

NuGet package `Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore`


# Project structure

Functionality is broken up as follows

## Startup

Prior to .NET Core 6, the startup comprised of two files, `Startup.cs` and `Program.cs`. .NET 6 introduces a new hosting model for ASP.NET Core applications. This model is streamlined and reduces the amount of boilerplate code required to get a basic ASP.NET Core application up and running.

## wwwroot

CSS, JavaScript libraries
## Pages/Shared

Site wide configuration

## Pages/Views

The `Pages` folder contains pages for each model [^4]. Under each folder e.g. the `Customers` folder are several pages, for

- Viewing data (list all records)
- Details view of data (one record)
- Editing data
- Removal of data

For this project I left some pages disconected or left out to show (if time permits) how to create a new page.


# Libraries

Visual Studio has a component, `libman` which allows adding external libraries such as OED libraries and/or libraries such as BootStrap (ASP.NET Core comes with the current release of BootStrap).

**libman**

![Libman](assets/libman.png)

# Add code to a page

See the following [page](https://learn.microsoft.com/en-us/aspnet/web-pages/overview/getting-started/introducing-razor-syntax-c) for more.

The `@` character starts inline expressions, single statement blocks, and multi-statement blocks:

```csharp
<!-- Single statement blocks  -->
@{ var total = 7; }
@{ var myMessage = "Hello World"; }

<!-- Inline expressions -->
<p>The value of your account is: @total </p>
<p>The value of myMessage is: @myMessage</p>

<!-- Multi-statement block -->
@{
    var greeting = "Welcome to our site!";
    var weekDay = DateTime.Now.DayOfWeek;
    var greetingMessage = greeting + " Today is: " + weekDay;
}
<p>The greeting is: @greetingMessage</p>
```

Enclose code blocks in braces. A code block includes one or more code statements and is enclosed in braces.

```csharp
<!-- Single statement block.  -->
@{ var theMonth = DateTime.Now.Month; }
<p>The numeric value of the current month: @theMonth</p>

<!-- Multi-statement block. -->
@{
    var outsideTemp = 79;
    var weatherMessage = "Hello, it is " + outsideTemp + " degrees.";
}
<p>Today's weather: @weatherMessage</p>
```

# Tag Helpers

Tag Helpers enable server-side code to participate in creating and rendering HTML elements in Razor files. 

Most built-in Tag Helpers target standard HTML elements and provide server-side attributes for the element. For example, the `<input>` element used in many views in the Views/Account folder contains the asp-for attribute. This attribute extracts the name of the specified model property into the rendered HTML. Consider a Razor view with the following


```csharp
public class Movie
{
    public int ID { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Genre { get; set; }
    public decimal Price { get; set; }
}
```

The following Razor markup:

```html
<label asp-for="Movie.Title"></label>
```

Generates the following HTML:

```html
<label for="Movie_Title">Title</label>
```

## Tag Helpers in forms

See the [following](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-6.0)

## built in tag helpers

See the [following](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/built-in/anchor-tag-helper?view=aspnetcore-6.0)


# Entity Framework Core

- DbContext (under the `Data` folder)
    - Manage database connection
    - Configure model & relationship
    - Querying database
    - Saving data to the database
    - Configure change tracking
    - Caching
    - Transaction management

Samples were `context` is a `DbContext`

Get a `Customer` and their orders

```csharp
public static async Task<Customer> GetCustomer(int id)
{
    await using var context = new PizzaContext();
    return await context
        .Customers
        .Include(c => c.Orders)
        .FirstOrDefaultAsync(c => c.Id == id);
}
```

Update a `Customer` where the code is designed to update only first and last name. Caveat, the line with `IsModified = false` is ceremonial in that Entity Framework Core is smart enough on it's own to know what to update but in earlier versions we needed `IsModified = false`.

```csharp
public static async Task UpdateName(Customer currentCustomer)
{
    await using var context = new PizzaContext();
    var customer = context.Customers.FirstOrDefault(c => c.Id == currentCustomer.Id);

    if (customer is not null)
    {
        customer.FirstName = currentCustomer.FirstName;
        customer.LastName = currentCustomer.LastName;
        context.Entry(customer).State = EntityState.Modified;
        context.Entry(customer).Property(p => p.Email).IsModified = false;
        await context.SaveChangesAsync();
    }
}
```




- Models (under the `Models` folder)




# Database schema

![Database Model](assets/DatabaseModel.png)

# CSS Isolation

Razor Pages [CSS Isolation](https://github.com/karenpayneoregon/razor-pages-style-isolation)

# Resources

- [Host](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-6.0) ASP.NET Core on Windows with IIS
- [Globalization](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-6.0) and localization in ASP.NET Core
- Razor [syntax reference](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-6.0) for ASP.NET Core
- Security [topics](https://learn.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-6.0)
- [Globalization and Internationalization](https://app.pluralsight.com/library/courses/asp-dot-net-core-6-globalization-internationalization/table-of-contents) in ASP.NET Core 6 (Pluralsight)
- ASP.NET Razor - [C# Code Syntax](https://gist.github.com/jwill9999/655533b6652418bd3bc94d864a5e2b49) 
- ASP.NET - [Razor Syntax Cheat](https://www.snippset.com/asp-net-razor-syntax-cheat-sheet-s586) sheet
- Dependency Injection in ASP.NET Core 6 [Course](https://app.pluralsight.com/library/courses/dependency-injection-asp-dot-net-core-6/table-of-contents)
- [FluentEmail](https://github.com/lukencode/FluentEmail) - All in one email sender for .NET and .NET Core
- Concurrency conflicts
    - [Microsoft](https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/concurrency?view=aspnetcore-6.0&tabs=visual-studio)
    - Karen Payne working [example](https://github.com/karenpayneoregon/ef-core-6-tips/tree/master/TaxpayersConcurrencyCheck) done in a console project using [[ConcurrencyCheck](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.concurrencycheckattribute?view=net-6.0)]



[^1]: Overview to [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
[^2]:Introduction to [Razor Pages](https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-6.0&tabs=visual-studio) in ASP.NET Core
[^3]: Entity Framework Core 6
[^4]: A [model](https://learn.microsoft.com/en-us/ef/core/#the-model) is made up of entity classes and a context object that represents a session with the database. 
