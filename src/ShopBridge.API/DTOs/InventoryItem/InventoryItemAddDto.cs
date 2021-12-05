using System;
using System.ComponentModel.DataAnnotations;

namespace ShopBridge.API.DTOs.InventoryItem
{
    public class InventoryItemAddDto
    {
        [Required]
        [StringLength(150, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
