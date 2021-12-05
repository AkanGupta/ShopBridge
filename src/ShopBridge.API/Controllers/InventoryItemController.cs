using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopBridge.API.DTOs.InventoryItem;
using ShopBridge.Domain.Interfaces;
using ShopBridge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ShopBridge.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryItemController : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService;
        private readonly IMapper _mapper;
        private ILogger<InventoryItemController> _logger;
        public InventoryItemController(ILogger<InventoryItemController> logger, IMapper mapper,
                                IInventoryItemService inventoryItemService)
        {
            _logger = logger;
            _mapper = mapper;
            _inventoryItemService = inventoryItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageSize = 50, int page = 1)
        {
            try
            {
                if (pageSize <= 0)
                    pageSize = 50;
                if (page < 1)
                    page = 1;

                var items = await _inventoryItemService.GetAll();
                var count = items.Count();

                var paginatedItems = items
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Select(u => _mapper.Map<InventoryItemResultDto>(u))
                            .ToList();
                var paginatedList = new PaginatedList<InventoryItemResultDto>(paginatedItems, count, page, pageSize);
                return Ok(_mapper.Map<IEnumerable<InventoryItemResultDto>>(paginatedList));
            }
            catch (Exception ex)
            {
                _logger.LogError($"error while geting all item information: {ex}");
                return StatusCode((int)HttpStatusCode.NotFound, "error while  geting all items information");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _inventoryItemService.GetById(id);

                if (item == null)
                {
                    _logger.LogWarning($"Item with given id: {id} doesn't exists in the store.");
                    return NotFound();
                }

                return Ok(_mapper.Map<InventoryItemResultDto>(item));
            }
            catch(Exception ex)
            {
                _logger.LogError($"error while geting item information: {ex}");
                return StatusCode((int)HttpStatusCode.NotFound, "error while geting item information");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InventoryItemAddDto itemDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();

                var item = _mapper.Map<InventoryItem>(itemDto);
                var itemResult = await _inventoryItemService.Add(item);

                if (itemResult == null) return BadRequest();

                return Ok(_mapper.Map<InventoryItemResultDto>(itemResult));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while creating item: {ex}");
                return StatusCode((int)HttpStatusCode.BadRequest, "Error while creating item");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, InventoryItemEditDto itemDto)
        {
            try
            {
                if (id != itemDto.Id) return BadRequest();

                if (!ModelState.IsValid) return BadRequest();

                await _inventoryItemService.Update(_mapper.Map<InventoryItem>(itemDto));

                return Ok(itemDto);
            }
            catch(Exception ex)
            {
                _logger.LogError($"error while updating item : {ex}");
                return StatusCode((int)HttpStatusCode.BadRequest, "error while updating Item");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                var item = await _inventoryItemService.GetById(id);
                if (item == null)
                {
                    _logger.LogWarning($"Item with given name: {id} doesn't exists in the store.");
                    return NotFound();
                }

                await _inventoryItemService.Remove(item);

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError($"error while deleting item : {ex}");
                return StatusCode((int)HttpStatusCode.NotFound, "error while delting item");
            }
        }

        [HttpGet]
        [Route("search/{itemName}")]
        public async Task<ActionResult<List<InventoryItem>>> Search(string itemName)
        {
            try
            {
                var items = _mapper.Map<List<InventoryItem>>(await _inventoryItemService.Search(itemName));

                if (items == null || items.Count == 0)
                {
                    _logger.LogError($"Item with the give name: {itemName} is not found");
                    return NotFound();
                }
                return Ok(items);
            }
            catch(Exception ex)
            {
                _logger.LogError($"error while geting item: {ex}");
                return StatusCode((int)HttpStatusCode.NotFound, "error while  geting item");
            }
        }

    }
}
