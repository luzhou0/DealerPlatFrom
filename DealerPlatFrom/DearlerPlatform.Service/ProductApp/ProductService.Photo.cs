using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DearlerPlatform.Domain;

namespace DearlerPlatform.Service.ProductApp
{
    public partial class ProductService : IProductService
    {
        //根据ProductNo获取ProductPhoto>表信息
        public async Task<List<ProductPhoto>> GetProductPhotosByProductNo(params string[] productNo)
        {
            var productPhotos = await _productPhotoRepository.GetListAsync(m => productNo.Contains(m.ProductNo));
            return productPhotos;
        }
        
    }
}