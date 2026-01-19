using System.ComponentModel.DataAnnotations;

namespace Restaurantproject.Areas.Admin.Models
{
    public class MenuItem
    {
        [Key]
        public int MenuItemId { get; set; }

        [Required(ErrorMessage = "the name is Required"), StringLength(100, ErrorMessage = "please enter name less than 100 char")]
        public string Name { get; set; }

        [Required, StringLength(500, ErrorMessage = "please enter Description less than 500 char ")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 1000)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        // مثال: "وجبات رئيسية" - "مشروبات" - "حلويات"
        [StringLength(100, ErrorMessage = "please enter Category less than 100 char ")]
        public string Category { get; set; }

        // العلاقة: عنصر القائمة قد يكون ضمن تفاصيل طلبات متعددة
        public ICollection<OrderDetail>? OrderDetails { get; set; }

    }
}
