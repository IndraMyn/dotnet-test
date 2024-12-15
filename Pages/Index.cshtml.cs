using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;

namespace test.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BaseDbContext _dbContext;

    [BindProperty]
    public List<SoOrderDto> Orders { get; set; } = [];
    public int TotalPage { get; set; } = 0;
    public int TotalItem { get; set; } = 0;
    public int CurrentPage { get; set; } = 1;

    public IndexModel(ILogger<IndexModel> logger, BaseDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task OnGetAsync([FromQuery] string? keyword, [FromQuery] DateTime? orderDate, [FromQuery] int pages = 1, [FromQuery] int perPage = 5)
    {

        CurrentPage = pages <= 1 ? 1 : pages * perPage;

        IQueryable<SoOrder> query = _dbContext.SoOrders.Include(x => x.ComCustomer).OrderByDescending(x => x.OrderDate);

        TotalItem = await query.CountAsync();

        if (keyword != null) query = query.Where(x => x.OrderNo.Contains(keyword));
        if (orderDate != null) query = query.Where(x => x.OrderDate.Date == orderDate);

        var list = await query.Skip((pages - 1) * perPage).Take(perPage).ToListAsync();

        var count = await query.CountAsync();

        TotalPage = count >= perPage ? count / perPage : 1;

        foreach (var item in list)
        {
            Orders.Add(new SoOrderDto()
            {
                ID = item.SoOrderId,
                OrderDate = DateOnly.FromDateTime(item.OrderDate),
                SalesOrder = item.OrderNo,
                Customer = item.ComCustomer.CustomerName,
                CustomerId = item.ComCustomer.ComCustomerId
            });
        }
    }

    public async Task<IActionResult> OnPostDelete(int id)
    {
        var order = await _dbContext.SoOrders.Where(x => x.SoOrderId == id).FirstOrDefaultAsync();
        _dbContext.SoOrders.Remove(order);
        await _dbContext.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnGetExport()
    {

        var orders = await _dbContext.SoOrders.Include(x => x.ComCustomer).OrderByDescending(x => x.OrderDate).ToListAsync();
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Items");
            var currentRow = 1;

            // Header
            worksheet.Cell(currentRow, 1).Value = "ID";
            worksheet.Cell(currentRow, 2).Value = "Sales Order";
            worksheet.Cell(currentRow, 3).Value = "Order Date";
            worksheet.Cell(currentRow, 4).Value = "Customer";
            worksheet.Cell(currentRow, 5).Value = "Address";

            // Isi data
            foreach (var order in orders)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = order.SoOrderId;
                worksheet.Cell(currentRow, 2).Value = order.OrderNo;
                worksheet.Cell(currentRow, 3).Value = order.OrderDate.ToString("dd-MM-yyyy");
                worksheet.Cell(currentRow, 4).Value = order.ComCustomer.CustomerName;
                worksheet.Cell(currentRow, 5).Value = order.Address;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Orders-{DateTime.Now.ToString("dd-MM-yyyy")}.xlsx");
            }
        }
    }
}
