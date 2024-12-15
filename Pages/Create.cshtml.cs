using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;

namespace test.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly BaseDbContext _dbContext;

        public CreateModel(ILogger<CreateModel> logger, BaseDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [BindProperty]
        public SoOrderDetailDto OrderDetail { get; set; }

        public List<ComCustomer> Customers { get; set; } = [];

        public async Task OnGet()
        {
            Customers = await _dbContext.ComCustomers.ToListAsync();
        }

        public async Task OnPost()
        {
            var soOrder = new SoOrder()
            {
                ComCustomerId = OrderDetail.CustomerId,
                OrderDate = DateTime.Parse(OrderDetail.OrderDate.ToString()),
                Address = OrderDetail.Address,
                OrderNo = OrderDetail.SalesOrder,
            };

            await _dbContext.SoOrders.AddAsync(soOrder);

            await _dbContext.SaveChangesAsync();

            foreach (var item in OrderDetail.Items)
            {
                await _dbContext.SoItems.AddAsync(new()
                {
                    SoOrderId = soOrder.SoOrderId,
                    ItemName = item.ItemName,
                    Price = item.Price,
                    Quantity = item.Qty,
                });

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
