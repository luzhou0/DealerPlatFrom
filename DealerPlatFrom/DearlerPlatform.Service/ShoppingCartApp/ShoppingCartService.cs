using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DearlerPlatform.Common.EventBusHelper;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.ShoppingCartApp.Dto;

namespace DearlerPlatform.Service.ShoppingCartApp
{
    public class ShoppingCartService : IShoppingCartServcice
    {
        private readonly IRepository<ShoppingCart> _shoppCartRepo;
        private readonly IMapper _mapper;
        private readonly LocalEnevtBus<List<ShoppingCartDto>> _localEnevtBusShoppCartDto;

        public ShoppingCartService(IRepository<ShoppingCart> shoppCartRepo, IMapper mapper, LocalEnevtBus<List<ShoppingCartDto>> localEnevtBusShoppCartDto)
        {
            _shoppCartRepo = shoppCartRepo;
            _mapper = mapper;
            this._localEnevtBusShoppCartDto = localEnevtBusShoppCartDto;
        }
        /// <summary>
        /// 购物车加车 增加数量
        /// </summary>
        /// <returns></returns>
        public async Task<ShoppingCart> SetShoppingCart(ShoppingCartInputDto shoppingCartInputDto)
        {
            ShoppingCart shoppingCartResult = null;
            //查询购物车表有没有这条数据
            var shoppingCart = await _shoppCartRepo.GetAsync(m => m.ProductNo == shoppingCartInputDto.ProductNo);
            //如果有则修改购物车数量 没有则新增
            if (shoppingCart != null)
            {
                shoppingCart.ProductNum++;
                shoppingCartResult = await _shoppCartRepo.UpdateAsync(shoppingCart);
            }
            else
            {
                var shoppingCartInsert = _mapper.Map<ShoppingCartInputDto, ShoppingCart>(shoppingCartInputDto);
                shoppingCartInsert.CartSelected = false;
                shoppingCartInsert.CartGuid = Guid.NewGuid().ToString();
                shoppingCartResult = await _shoppCartRepo.InsertAsync(shoppingCartInsert);
            }
            return shoppingCartResult;
        }

        public async Task<List<ShoppingCartDto>> GetShoppingCartDtosAsync(string customerNo)
        {
            var carts = await _shoppCartRepo.GetListAsync(m => m.CustomerNo == customerNo);
            var shoppingCartDtos = _mapper.Map<List<ShoppingCart>, List<ShoppingCartDto>>(carts);
            await _localEnevtBusShoppCartDto.Publish(shoppingCartDtos);
            return shoppingCartDtos;
        }
    }
}