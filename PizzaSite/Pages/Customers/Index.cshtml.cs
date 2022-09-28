#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Data;
using PizzaShop.Models;

namespace PizzaShop.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly PizzaContext _context;

        public IndexModel(PizzaContext context)
        {
            _context = context;
        }

        public IList<Customer> Customer { get;set; }

        public async Task OnGetAsync()
        {
            Customer = await _context.Customers
                                .Include(c => c.Orders)
                                .ToListAsync();
        }
    }
}
