using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.ProductApp.Dto;
using DearlerPlatform.Service.ShoppingCartApp.Dto;

namespace DearlerPlatform.Service
{
    public class DealerPlatformProfile:Profile
    {
         public DealerPlatformProfile()
         {
             CreateMap<Product,ProductDto>().ReverseMap();
             CreateMap<ProductSale,ProductDto>().ReverseMap();
             CreateMap<ProductSaleAreaDiff,ProductDto>().ReverseMap();
             CreateMap<ProductPhoto,ProductDto>().ReverseMap();
             CreateMap<ShoppingCart,ShoppingCartInputDto>().ReverseMap();
             CreateMap<ShoppingCart,ShoppingCartDto>().ReverseMap();
         }
    }
}