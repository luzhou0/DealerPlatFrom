using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DearlerPlatform.Core;
using DearlerPlatform.Service.ProductApp;
using DearlerPlatform.Service.ProductApp.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DearlerPlatform.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        [HttpGet("GetProduct")]
        public async Task<List<ProductDto>> GetProductDtosAsync(string? searchText, string? productType, string blongTypeName,string? productProps, string? sort, int pageIndex = 1, int pageSize = 10)
        {
            Dictionary<string,string> dicProductProps = new();
            var arrProductProps = productProps?.Split("^")?? Array.Empty<string>();
            foreach (var item in arrProductProps)
            {
                var key = item.Split("_")[0];
                var value = item.Split("_")[1];
                dicProductProps.Add(key,value);
            }
            var productDtos = await _productService.GetProductDtos(searchText, productType, blongTypeName,dicProductProps,new PageWithSortDto
            {
                Sort = sort,
                PageIndex = pageIndex,
                PageSize = pageSize
            });
            return productDtos;
        }
        [HttpGet("GetBlongType")]
        public async Task<List<BlongTypeDto>> GetBlongTypeDtosAsync()
        {
            return await _productService.GetBlongTypeDtosAsync();
        }

        [HttpGet("GetProductType")]
        public async Task<List<ProductTypeDto>> GetProductTypeDtosAsync(string belongTypeName)
        {
            return await _productService.GetProductTypeDtosAsync(belongTypeName);
        }
        [HttpGet("GetProductProps")]
        public async Task<Dictionary<string, List<string>>> GetProductPropsAsync(string belongTypeName, string? typeNo)
        {
            return await _productService.GetProductPropsAsync(belongTypeName,typeNo);
        }
    }
}