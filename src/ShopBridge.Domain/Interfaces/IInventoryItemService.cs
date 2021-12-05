using ShopBridge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Domain.Interfaces
{
    public interface IInventoryItemService : IDisposable
    {
        Task<IEnumerable<InventoryItem>> GetAll();
        Task<InventoryItem> GetById(int id);
        Task<InventoryItem> Add(InventoryItem book);
        Task<InventoryItem> Update(InventoryItem book);
        Task<bool> Remove(InventoryItem book);
        Task<IEnumerable<InventoryItem>> Search(string itemName);
    }
}
