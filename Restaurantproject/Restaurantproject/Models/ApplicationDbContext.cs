using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurantproject.Areas.Admin.Models;

namespace Restaurantproject.Models
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        // الطلب الجديد الذي نريد إضافته
        public Order NewOrder { get; set; } = new();

        // الأصناف المختارة (IDs)
        public List<int>? SelectedMenuItemIds { get; set; }

        // الكمية (في حال أردت كمية لكل صنف)
        public int Quantity { get; set; } = 1;

    }
}
