using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Restaurantproject.Areas.Admin.Models;
using Restaurantproject.Models;


namespace Restaurantproject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            // عرض الطلبات والنموذج في صفحة واحدة

            var viewModel = new OrderViewModel

            {
                Orders = await _context.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(m => m.MenuItem)
                    .ToListAsync(),

                MenuItems = await _context.MenuItems.ToListAsync()
            };

            return View("Orders", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Orders(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Orders));
            }

            if (model.NewOrder == null || model.SelectedMenuItemIds == null || !model.SelectedMenuItemIds.Any())
            {
                ModelState.AddModelError("", "You must enter the order details and select at least one item.");

                return RedirectToAction(nameof(Orders));

            }

            // إنشاء الطلب
            var order = new Order
            {
                CustomerName = model.NewOrder.CustomerName,
                PhoneNumber = model.NewOrder.PhoneNumber,
                Address = model.NewOrder.Address,
                OrderDate = DateTime.Now,
                Status = "Under preparation",
                TotalAmount = 0m
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // إضافة تفاصيل الأصناف
            foreach (var itemId in model.SelectedMenuItemIds)
            {

                var menuItem = await _context.MenuItems.FindAsync(itemId);
                if (menuItem != null)
                {
                    // نحاول نحصل على الكمية الخاصة بهذا الصنف
                    model.Quantities.TryGetValue(itemId, out int quantity);
                    if (quantity <= 0) quantity = 1; // في حال لم يدخل المستخدم كمية

                    _context.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = order.OrderId,
                        MenuItemId = menuItem.MenuItemId,
                        Quantity = quantity

                    });
                    order.TotalAmount += menuItem.Price * quantity;
                }
            }

            // حفظ تفاصيل الطلب
            var saveDetails = await _context.SaveChangesAsync();
            if (saveDetails <= 0)
            {
                ModelState.AddModelError("", "Failed to save the order details, please try again.");
                return RedirectToAction(nameof(Orders));
            }

            return RedirectToAction(nameof(Orders));



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            // البحث عن الطلب مع التفاصيل المرتبطة به
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            // حذف التفاصيل أولاً (حتى لا تحدث مشكلة في العلاقات)
            _context.OrderDetails.RemoveRange(order.OrderDetails);

            // ثم حذف الطلب نفسه
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();

            // بعد الحذف، العودة لقائمة الطلبات
            return RedirectToAction(nameof(Orders));
        }

    }
}