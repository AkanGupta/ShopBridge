using AutoMapper;
using ShopBridge.API.DTOs.InventoryItem;
using ShopBridge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.API.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<InventoryItem, InventoryItemAddDto>().ReverseMap();
        }
    }
}
