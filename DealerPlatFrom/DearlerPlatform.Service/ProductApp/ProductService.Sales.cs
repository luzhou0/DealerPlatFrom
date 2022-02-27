using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DearlerPlatform.Domain;

namespace DearlerPlatform.Service.ProductApp
{
    public partial class ProductService : IProductService
    {
        //根据ProductNo获取商品价格信息
        public async Task<List<ProductSale>> GetProductSalesByProductNo(params string[] productNos)
        {
            var productSales = await _productSaleRepository.GetListAsync(m => productNos.Contains(m.ProductNo));
            return productSales;
        }
    }
}