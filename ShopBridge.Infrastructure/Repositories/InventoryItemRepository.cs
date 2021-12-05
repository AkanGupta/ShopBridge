using Microsoft.EntityFrameworkCore;
using ShopBridge.Domain.Interfaces;
using ShopBridge.Domain.Models;
using ShopBridge.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Infrastructure.Repositories
{
    public class InventoryItemRepository :Repository<InventoryItem>, IInventoryItemRepository
    {
        public InventoryItemRepository(ShopBridgeDbContext context) : base(context) { }
    }
}
