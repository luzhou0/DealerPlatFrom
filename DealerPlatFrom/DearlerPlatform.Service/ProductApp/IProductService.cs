using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DearlerPlatform.Core;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.ProductApp.Dto;
using DearlerPlatform.Service.ShoppingCartApp.Dto;

namespace DearlerPlatform.Service.ProductApp
{
    public interface IProductService : IocTag
    {
        public Task<List<ProductDto>> GetProductDtos(string searchText, string productType, string blongTypeName, Dictionary<string,string> dicProductProps, PageWithSortDto pageWithSortDto);
        public Task<List<ProductPhoto>> GetProductPhotosByProductNo(params string[] productNo);
        public Task<List<ProductSale>> GetProductSalesByProductNo(params string[] productNos);
        public Task<List<BlongTypeDto>> GetBlongTypeDtosAsync();
        public Task<List<ProductTypeDto>> GetProductTypeDtosAsync(string belongTypeName);
        public Task<Dictionary<string, List<string>>> GetProductPropsAsync(string belongTypeName, string typeNo);
        public  Task LocalEventHandler(List<ShoppingCartDto> shoppingCartDtos);
    }
}