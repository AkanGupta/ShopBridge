using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopBridge.API.Controllers;
using ShopBridge.Domain.Interfaces;
using ShopBridge.Domain.Models;
using Moq;
using System.Threading.Tasks;
using ShopBridge.API.DTOs.InventoryItem;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ShopBridge.API.Tests
{
    [TestClass]
    public class InventoryItemControllerTest
    {
        private InventoryItemController _inventoryItemController;
        private Mock<IInventoryItemService> _inventoryItemServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<InventoryItemController>> _logger;

        [TestInitialize]
        public void TestInitialize()
        {
            _inventoryItemServiceMock = new Mock<IInventoryItemService>();
            _mapperMock = new Mock<IMapper>();
            _logger = new Mock<ILogger<InventoryItemController>>();
            _inventoryItemController = new InventoryItemController(_logger.Object, _mapperMock.Object, _inventoryItemServiceMock.Object);
        }

        private InventoryItem CreateItem()
        {
            return new InventoryItem()
            {
                Id = 1,
                Name = "Item Test",
                Description = "Description Test",
                Price = 150,
                Quantity = 2
            };
        }

        private InventoryItemResultDto MapModelToInventoryItemResultDto(InventoryItem item)
        {
            var itemDto = new InventoryItemResultDto()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Quantity = item.Quantity
            };
            return itemDto;
        }

        private List<InventoryItem> CreateItemList()
        {
            return new List<InventoryItem>()
    {
        new InventoryItem()
        {
            Id = 1,
            Name = "Item Test 1",
            Description = "Description Test 1",
            Price = 100,
            Quantity = 2
        },
        new InventoryItem()
        {
            Id = 2,
            Name = "Item Test 2",
            Description = "Description Test 2",
            Price = 200,
            Quantity = 4
        },
        new InventoryItem()
        {
            Id = 3,
            Name = "Item Test 3",
            Description = "Description Test 3",
            Price = 300,
            Quantity = 6
        }
    };
        }

        private List<InventoryItemResultDto> MapModelToInventoryItemResultListDto(List<InventoryItem> items)
        {
            var listItems = new List<InventoryItemResultDto>();

            foreach (var item in items)
            {
                var inventoryItem = new InventoryItemResultDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
                listItems.Add(inventoryItem);
            }
            return listItems;
        }


        [TestMethod]
        public async Task GetAll_ShouldReturnOk_WhenExistItems()
        {
            var items = CreateItemList();
            var dtoExpected = MapModelToInventoryItemResultListDto(items);

            _inventoryItemServiceMock.Setup(c => c.GetAll()).ReturnsAsync(items);
            _mapperMock.Setup(m => m.Map<IEnumerable<InventoryItemResultDto>>(It.IsAny<List<InventoryItem>>())).Returns(dtoExpected);

            var result = await _inventoryItemController.GetAll();
    
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _inventoryItemServiceMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnOk_WhenDoesNotExistAnyItem()
        {
            var items = new List<InventoryItem>();
            var dtoExpected = MapModelToInventoryItemResultListDto(items);

            _inventoryItemServiceMock.Setup(c => c.GetAll()).ReturnsAsync(items);
            _mapperMock.Setup(m => m.Map<IEnumerable<InventoryItemResultDto>>(It.IsAny<List<InventoryItem>>())).Returns(dtoExpected);

            var result = await _inventoryItemController.GetAll();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetById_ShouldReturnOk_WhenItemExist()
        {
            var item = CreateItem();
            var dtoExpected = MapModelToInventoryItemResultDto(item);

            _inventoryItemServiceMock.Setup(c => c.GetById(1)).ReturnsAsync(item);
            _mapperMock.Setup(m => m.Map<InventoryItemResultDto>(It.IsAny<InventoryItem>())).Returns(dtoExpected);

            var result = await _inventoryItemController.GetById(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _inventoryItemServiceMock.Verify(mock => mock.GetById(1), Times.Once);
        }

        [TestMethod]
        public async Task GetById_ShouldReturnOk_WhenItemDoesNotExist()
        {
            _inventoryItemServiceMock.Setup(c => c.GetById(1)).ReturnsAsync((InventoryItem)null);

            var result = await _inventoryItemController.GetById(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Add_ShouldReturnOk_WhenItemIsAdded()
        {
            var item = CreateItem();
            var itemAddDto = new InventoryItemAddDto() { Name = item.Name };
            var itemResultDto = MapModelToInventoryItemResultDto(item);

            _mapperMock.Setup(m => m.Map<InventoryItem>(It.IsAny<InventoryItemAddDto>())).Returns(item);
            _inventoryItemServiceMock.Setup(c => c.Add(item)).ReturnsAsync(item);
            _mapperMock.Setup(m => m.Map<InventoryItemResultDto>(It.IsAny<InventoryItem>())).Returns(itemResultDto);

            var result = await _inventoryItemController.Add(itemAddDto);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _inventoryItemServiceMock.Verify(mock => mock.Add(item), Times.Once);
        }

        [TestMethod]
        public async Task Add_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            var itemAddDto = new InventoryItemAddDto();
            _inventoryItemController.ModelState.AddModelError("Name", "The field name is required");

            var result = await _inventoryItemController.Add(itemAddDto);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Add_ShouldReturnBadRequest_WhenItemResultIsNull()
        {
            var item = CreateItem();
            var itemAddDto = new InventoryItemAddDto() { Name = item.Name };

            _mapperMock.Setup(m => m.Map<InventoryItem>(It.IsAny<InventoryItemAddDto>())).Returns(item);
            _inventoryItemServiceMock.Setup(c => c.Add(item)).ReturnsAsync((InventoryItem)null);

            var result = await _inventoryItemController.Add(itemAddDto);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnOk_WhenItemIsUpdatedCorrectly()
        {
            var item = CreateItem();
            var itemEditDto = new InventoryItemEditDto() { Id = item.Id, Name = "Test" };

            _mapperMock.Setup(m => m.Map<InventoryItem>(It.IsAny<InventoryItemEditDto>())).Returns(item);
            _inventoryItemServiceMock.Setup(c => c.GetById(item.Id)).ReturnsAsync(item);
            _inventoryItemServiceMock.Setup(c => c.Update(item)).ReturnsAsync(item);

            var result = await _inventoryItemController.Update(itemEditDto.Id, itemEditDto);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _inventoryItemServiceMock.Verify(mock => mock.Update(item), Times.Once);
        }

        [TestMethod]
        public async Task Update_ShouldReturnBadRequest_WhenItemIdIsDifferentThenParameterId()
        {
            var itemEditDto = new InventoryItemEditDto() { Id = 1, Name = "Test" };

            var result = await _inventoryItemController.Update(2, itemEditDto);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            var itemEditDto = new InventoryItemEditDto() { Id = 1 };
            _inventoryItemController.ModelState.AddModelError("Name", "The field name is required");

            var result = await _inventoryItemController.Update(1, itemEditDto);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


        [TestMethod]
        public async Task Remove_ShouldReturnOk_WhenItemIsRemoved()
        {
            var item = CreateItem();
            _inventoryItemServiceMock.Setup(c => c.GetById(item.Id)).ReturnsAsync(item);
            _inventoryItemServiceMock.Setup(c => c.Remove(item)).ReturnsAsync(true);

            var result = await _inventoryItemController.Remove(item.Id);

            Assert.IsInstanceOfType(result, typeof(OkResult));
            _inventoryItemServiceMock.Verify(mock => mock.Remove(item), Times.Once);
        }

        [TestMethod]
        public async Task Remove_ShouldReturnNotFound_WhenItemDoesNotExist()
        {
            var item = CreateItem();
            _inventoryItemServiceMock.Setup(c => c.GetById(item.Id)).ReturnsAsync((InventoryItem)null);

            var result = await _inventoryItemController.Remove(item.Id);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Search_ShouldReturnOk_WhenItemWithSearchedNameExist()
        {
            var itemList = CreateItemList();
            var item = CreateItem();

            _inventoryItemServiceMock.Setup(c => c.Search(item.Name)).ReturnsAsync(itemList);
            _mapperMock.Setup(m => m.Map<List<InventoryItem>>(It.IsAny<IEnumerable<InventoryItem>>())).Returns(itemList);

            var result = await _inventoryItemController.Search(item.Name);
            var actual = (OkObjectResult)result.Result;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(OkObjectResult));
            _inventoryItemServiceMock.Verify(mock => mock.Search(item.Name), Times.Once);
        }

        [TestMethod]
        public async Task Search_ShouldReturnNotFound_WhenItemWithSearchedNameDoesNotExist()
        {
            var item = CreateItem();
            var itemList = new List<InventoryItem>();

            var dtoExpected = MapModelToInventoryItemResultDto(item);
            item.Name = dtoExpected.Name;

            _inventoryItemServiceMock.Setup(c => c.Search(item.Name)).ReturnsAsync(itemList);
            _mapperMock.Setup(m => m.Map<IEnumerable<InventoryItem>>(It.IsAny<InventoryItem>())).Returns(itemList);

            var result = await _inventoryItemController.Search(item.Name);
            var actual = (NotFoundResult)result.Result;

            Assert.IsInstanceOfType(actual, typeof(NotFoundResult));
        }
    }
}
