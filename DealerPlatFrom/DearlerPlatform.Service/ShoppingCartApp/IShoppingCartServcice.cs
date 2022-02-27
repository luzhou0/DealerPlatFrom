using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.ShoppingCartApp.Dto;

namespace DearlerPlatform.Service.ShoppingCartApp
{
    public interface IShoppingCartServcice:IocTag
    {
        public Task<ShoppingCart> SetShoppingCart(ShoppingCartInputDto shoppingCartInputDto);
        public Task<List<ShoppingCartDto>> GetShoppingCartDtosAsync(string customerNo);
    }
}