using System.ComponentModel.DataAnnotations;

namespace Restaurantproject.Areas.Admin.Models
{
    public class Order
    {

        [Key]
        public int OrderId { get; set; }

        [Required, StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        // مجموع السعر الكلي للطلب
        public decimal TotalAmount { get; set; }

        // حالة الطلب (قيد التجهيز، جاهز، تم التسليم)
        [StringLength(50)]
        public string Status { get; set; } = "The order is being prepared";

        // العلاقة: طلب يحتوي على عدة تفاصيل أصناف
        public ICollection<OrderDetail>? OrderDetails { get; set; }

    }
}
