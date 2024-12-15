using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.X509;
using test.Data;
using test.Models;

namespace test.Pages
{
    public class EditModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly BaseDbContext _dbContext;

        public EditModel(ILogger<IndexModel> logger, BaseDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [BindProperty]
        public SoOrderDetailDto OrderDetail { get; set; }

        [BindProperty]
        public List<ComCustomer> Customers { get; set; }


        public async Task OnGet(long id)
        {

            Customers = await _dbContext.ComCustomers.ToListAsync();

            var order = await _dbContext.SoOrders.Include(i => i.SoItems).Include(i => i.ComCustomer).Where(x => x.SoOrderId == id).FirstOrDefaultAsync();

            var items = new List<SoItemDto>();

            foreach (var item in order.SoItems)
            {
                items.Add(new()
                {
                    ItemName = item.ItemName,
                    Price = item.Price,
                    Qty = item.Quantity,
                    Total = item.Price * item.Quantity
                });
            }


            OrderDetail = new()
            {
                ID = id,
                OrderDate = DateOnly.FromDateTime(order.OrderDate),
                Customer = order.ComCustomer.CustomerName,
                CustomerId = order.ComCustomerId,
                SalesOrder = order.OrderNo,
                Address = order.Address,
                Items = items
            };

        }

        public async Task<IActionResult> OnPost()
        {
            var order = await _dbContext.SoOrders
                .Include(i => i.SoItems)
                .Include(i => i.ComCustomer)
                .Where(x => x.SoOrderId == OrderDetail.ID)
                .FirstOrDefaultAsync();

            order.OrderNo = OrderDetail.SalesOrder;
            order.Address = OrderDetail.Address;
            order.ComCustomerId = OrderDetail.CustomerId;
            order.OrderDate = DateTime.Parse(OrderDetail.OrderDate.ToString());

            _dbContext.Update(order); 

            _dbContext.SoItems.RemoveRange(order.SoItems);

            foreach (var item in OrderDetail.Items)
            {
                _dbContext.SoItems.Add(new SoItem
                {
                    ItemName = item.ItemName,
                    Price = item.Price,
                    Quantity = item.Qty,
                    SoOrderId = order.SoOrderId
                });
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToPage();
        }

    }
}
