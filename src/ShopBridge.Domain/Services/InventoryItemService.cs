using ShopBridge.Domain.Interfaces;
using ShopBridge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Domain.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public InventoryItemService(IInventoryItemRepository inventoryItemRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
        }
        public async Task<IEnumerable<InventoryItem>> GetAll()
        {
            return await _inventoryItemRepository.GetAll();
        }
        public async Task<InventoryItem> GetById(int id)
        {
            return await _inventoryItemRepository.GetById(id);
        }
        public async Task<InventoryItem> Add(InventoryItem item)
        {
            if (_inventoryItemRepository.Search(b => b.Name == item.Name).Result.Any())
                return null;

            await _inventoryItemRepository.Add(item);
            return item;
        }
        public async Task<InventoryItem> Update(InventoryItem item)
        {
            if (_inventoryItemRepository.Search(b => b.Name == item.Name && b.Id != item.Id).Result.Any())
                return null;

            await _inventoryItemRepository.Update(item);
            return item;
        }
        public async Task<bool> Remove(InventoryItem item)
        {
            await _inventoryItemRepository.Remove(item);
            return true;
        }
        public async Task<IEnumerable<InventoryItem>> Search(string item)
        {
            return await _inventoryItemRepository.Search(b => b.Name.Contains(item));
        }
        public void Dispose()
        {
            _inventoryItemRepository?.Dispose();
        }
    }
}
