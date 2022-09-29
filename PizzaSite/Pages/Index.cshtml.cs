using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PizzaShop.Data;
using System.Net;

namespace PizzaShop.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string IpAddress;

        public IndexModel(ILogger<IndexModel> logger, PizzaContext context)
        {
            _logger = logger;

            /*
             * Cheap way to warm-up EF Core for this demo
             * A better way is via a custom service or compiled models
             */
            _ = context.Customers.Count();
        }

        public async Task OnGet()
        {

            var httpClient = new HttpClient();
            var ipAddress = await httpClient.GetStringAsync("https://api.ipify.org");
            IpAddress = ipAddress;

            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
        }
    }
}