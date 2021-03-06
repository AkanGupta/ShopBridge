using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridge.Domain.Models
{
    public class InventoryItem : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
