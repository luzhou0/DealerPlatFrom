using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.ProductApp;
using DearlerPlatform.Service.ProductApp.Dto;
using DearlerPlatform.Service.ShoppingCartApp;
using DearlerPlatform.Service.ShoppingCartApp.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DearlerPlatform.Web.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartServcice _shoppingCartServcice;

        public ShoppingCartController(IShoppingCartServcice shoppingCartServcice, IProductService productService)
        {
            this._shoppingCartServcice = shoppingCartServcice;
        }
        /// <summary>
        /// 添加商品到购物车 有则修改商品数量 没有则新增
        /// </summary>
        /// <param name="shoppingCartInputDto"></param>
        /// <returns></returns>
        [HttpPost("SetShoppingCart")]
        public async Task<ShoppingCart> SetShoppingCart(ShoppingCartInputDto shoppingCartInputDto)
        {
            return await _shoppingCartServcice.SetShoppingCart(shoppingCartInputDto);
        }
        /// <summary>
        /// 获取购物车信息
        /// </summary>
        /// <param name="customerNo"></param>
        /// <returns></returns>
        [HttpGet("GetShoppingCarts")]
        public async Task<Dictionary<string, List<ShoppingCartDto>>> GetShoppingCartDtoAsync(string customerNo)
        {
            Dictionary<string, List<ShoppingCartDto>> shoppingCartResult = new();
            var shoppingCartDtos = await _shoppingCartServcice.GetShoppingCartDtosAsync(customerNo);
            var proDuctDtos = shoppingCartDtos.Select(s => s.ProductDto);
            var shoppingCartTypeDtos = proDuctDtos.Select(m => new ShoppingCartTypeDto
            { TypeNo = m.TypeNo, TypeName = m.TypeName }).DistinctBy(m => m.TypeNo).ToList();
            foreach (var type in shoppingCartTypeDtos)
            {
                string typejson = JsonConvert.SerializeObject(type);
                shoppingCartResult.Add(typejson, shoppingCartDtos.Where(m => m.ProductDto.TypeNo == type.TypeNo).ToList());
            };
            return shoppingCartResult;
        }
    }
}