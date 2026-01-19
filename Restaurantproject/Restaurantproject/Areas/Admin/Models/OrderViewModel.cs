namespace Restaurantproject.Areas.Admin.Models
{
    public class OrderViewModel
    {

        public List<Order>? Orders { get; set; } = new();
        public List<MenuItem>? MenuItems { get; set; } = new();

        // الطلب الجديد الذي نريد إضافته
        public Order NewOrder { get; set; } = new();

        // الأصناف المختارة (IDs)
        public List<int>? SelectedMenuItemIds { get; set; } = new();

        // الكمية (في حال أردت كمية لكل صنف)
        public int Quantity { get; set; } = 1;
        public Dictionary<int, int> Quantities { get; set; } = new();

    }
}