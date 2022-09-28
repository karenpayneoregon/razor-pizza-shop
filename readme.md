# Sample ASP.NET Core Razor page with EF Core

This repository provides a glimpses into the basics for building an ASP.NET Core Razor page application using Microsoft SQL-Server for a database and Microsoft EF Core 6 (Entity Framework Core).



# Setting up the database

With this solution open, from Visual Studio menu, select `PizzaShop Debug Properties` (use the small down arrow button)

![Figure1](assets/figure1.png)

Change **CREATE_DB** from **No** to **Yes**

![Figure2](assets/figure2.png)

Build the project

With Solution Explorer open, right click on the project node and select rebuild.

![Figure3](assets/figure3.png)

Click on the green button to run the project

![Figure4](assets/figure4.png)

Web site appear, for now close the browser.

Next, go back to debug properties and revert from Yes to No so that next time the project runs the database is not recreated.

![Figure3](assets/figure2.png)

**Notes**

- The database will reside under C:\users\\**your_login** as `OED.Pizza.mdf` and `OED.Pizza_log.ldf`
- To connect to the database via SQL-Server Management Studio (SSMS) use (localdb)\MSSQLLocalDB when prompted for a server


# Database schema

![Database Model](assets/DatabaseModel.png)

Sample conventional SELECT

```sql
DECLARE @OrderId AS INT = 2;
SELECT
        O.Id,
        O.OrderPlaced,
        O.OrderFulfilled,
        O.CustomerId,
        C.FirstName,
        C.LastName,
        OD.Quantity,
        OD.ProductId,
        P.[Name]
FROM
        dbo.Orders AS O
    INNER JOIN
        dbo.Customers AS C
            ON O.Id = C.Id
    INNER JOIN
        dbo.OrderDetails AS OD
            ON O.Id = OD.Id
    INNER JOIN
        dbo.Products  AS P
            ON O.Id = P.Id
WHERE
        (O.Id = @OrderId);
```

How it's done with EF Core

- Include and ThenInclude are representative for JOIN

```csharp
public static async Task GetOrder(int orderId)
{
    var result = await _context.Orders
        .Include(o => o.Customer)
        .Include(o => o.OrderDetails)
        .ThenInclude(o => o.Product)
        .FirstOrDefaultAsync(x => x.Id == orderId);
}
```

When running the application, I have setup that EF Core statements are shown in Visual Studio Output window. Example shown below. This can be helpful when there are performance issues to diagnose what needs to be done to improve performance.

Tip: We can tag EF Core statements with [TagWith](https://learn.microsoft.com/en-us/ef/core/querying/tags) and [TagWithCallSite](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.entityframeworkqueryableextensions.tagwithcallsite?view=efcore-6.0).


```sql
SELECT [o].[Id], [o].[CustomerId], [o].[OrderFulfilled], [o].[OrderPlaced], [c].[Id], [c].[Address], [c].[Email], [c].[FirstName], [c].[LastName], [c].[Phone]
FROM [Orders] AS [o]
INNER JOIN [Customers] AS [c] ON [o].[CustomerId] = [c].[Id]
ORDER BY [o].[OrderPlaced]
```