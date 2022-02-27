using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using DearlerPlatform.Common.EventBusHelper;
using DearlerPlatform.Core;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.ProductApp.Dto;
using DearlerPlatform.Service.ShoppingCartApp.Dto;
using Microsoft.JSInterop;

namespace DearlerPlatform.Service.ProductApp
{
    public partial class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductPhoto> _productPhotoRepository;
        private readonly IRepository<ProductSale> _productSaleRepository;
        private readonly IRepository<ProductSaleAreaDiff> _productSaleAreaDiffRepository;
        private readonly IMapper _mapper;


        public ProductService(
            IRepository<Product> productRepository,
            IRepository<ProductPhoto> productPhotoRepository,
            IRepository<ProductSale> productSaleRepository,
            IRepository<ProductSaleAreaDiff> productSaleAreaDiffRepository,
            IMapper mapper,
            LocalEnevtBus<List<ShoppingCartDto>> localEnevtBusShoppingCartDto)
        {
            this._productRepository = productRepository;
            this._productPhotoRepository = productPhotoRepository;
            this._productSaleRepository = productSaleRepository;
            this._productSaleAreaDiffRepository = productSaleAreaDiffRepository;
            this._mapper = mapper;
            localEnevtBusShoppingCartDto.localEventHandler += LocalEventHandler;
        }
        /// <summary>
        /// 根据客户来获取对应的购物车信息
        /// </summary>
        /// <param name="shoppingCartDtos"></param>
        /// <returns></returns>
        public async Task LocalEventHandler(List<ShoppingCartDto> shoppingCartDtos)
        {
            var productNos = shoppingCartDtos.Select(s => s.ProductNo).ToArray();
            var productDtos = await GetProductByProductNos(productNos);
            shoppingCartDtos.ForEach(shoppingCartDto =>
            {
                var productDto = productDtos.FirstOrDefault(m => m.ProductNo == shoppingCartDto.ProductNo);
                shoppingCartDto.ProductDto = productDto;
            });
        }
        /// <summary>
        /// 根据productNo获取对应产品数据
        /// </summary>
        /// <param name="postProductNos"></param>
        /// <returns></returns>
        private async Task<List<ProductDto>> GetProductByProductNos(params string[] postProductNos)
        {
            var productNos = postProductNos.Distinct();
            var products = await _productRepository.GetListAsync(m => productNos.Contains(m.ProductNo));
            var productDtos = _mapper.Map<List<Product>, List<ProductDto>>(products);
            var productPhotos = await GetProductPhotosByProductNo(productDtos.Select(p => p.ProductNo).ToArray());
            var productSales = await GetProductSalesByProductNo(productDtos.Select(p => p.ProductNo).ToArray());
            productDtos.ForEach(p =>
            {
                p.ProductPhoto = productPhotos.FirstOrDefault(photo => photo.ProductNo == p.ProductNo);
                p.ProductSale = productSales.FirstOrDefault(sale => sale.ProductNo == p.ProductNo);
                //设置默认值
                p.ProductPhoto ??= new ProductPhoto { ProductPhotoUrl = "/img/productPhotos/1.jpg" };
                p.ProductSale ??= new ProductSale { SalePrice = 100.00 };
            });
            return productDtos;
        }

        /// <summary>
        /// 获取商品表信息 分页查询
        /// </summary>
        /// <param name="searchText">搜索条件</param>
        /// <param name="productType">左侧边栏类型</param>
        /// <param name="blongTypeName">商品大类(五金/地板)</param>
        /// <param name="pageWithSortDto">排序条件</param>
        /// <returns></returns>
        public async Task<List<ProductDto>> GetProductDtos(string searchText, string productType, string blongTypeName, Dictionary<string, string> dicProductProps, PageWithSortDto pageWithSortDto)
        {
            pageWithSortDto.Sort ??= "ProductName";
            //searchText为null时
            searchText ??= "";
            var bzgg = dicProductProps.ContainsKey("ProductBZGG") ? dicProductProps["ProductBZGG"] : null;
            dicProductProps.TryGetValue("ProductCd", out string cd);
            dicProductProps.TryGetValue("ProductPp", out string pp);
            dicProductProps.TryGetValue("ProductXh", out string xh);
            dicProductProps.TryGetValue("ProductCz", out string cz);
            dicProductProps.TryGetValue("ProductHb", out string hb);
            dicProductProps.TryGetValue("ProductHd", out string hd);
            dicProductProps.TryGetValue("ProductGy", out string gy);
            dicProductProps.TryGetValue("ProductHs", out string hs);
            dicProductProps.TryGetValue("ProductMc", out string mc);
            dicProductProps.TryGetValue("ProductDj", out string dj);
            dicProductProps.TryGetValue("ProductGg", out string gg);
            dicProductProps.TryGetValue("ProductYs", out string ys);
            //跳过多少条 //Skip跳过多少条 Take返回多少个元素
            int skipNum = (pageWithSortDto.PageIndex - 1) * pageWithSortDto.PageSize;
            // var products = (from p in await _productRepository.GetListAsync()
            //                 where p.BelongTypeName == blongTypeName
            //                 && (p.TypeNo == productType || string.IsNullOrEmpty(productType))
            //                 && (p.ProductName.Contains(searchText) || string.IsNullOrEmpty(searchText))
            //                 && (bzgg == null || p.ProductBzgg == bzgg)
            //                 && (cd == null || p.ProductCd == cd)
            //                 && (pp == null || p.ProductPp == pp)
            //                 && (xh == null || p.ProductXh == xh)
            //                 && (cz == null || p.ProductCz == cz)
            //                 && (hb == null || p.ProductHb == hb)
            //                 && (hd == null || p.ProductHd == hd)
            //                 && (gy == null || p.ProductGy == gy)
            //                 && (hs == null || p.ProductHs == hs)
            //                 && (mc == null || p.ProductMc == mc)
            //                 && (dj == null || p.ProductDj == dj)
            //                 && (gg == null || p.ProductGg == gg)
            //                 && (ys == null || p.ProductYs == ys)
            //                 orderby p.GetType().GetProperty(pageWithSortDto.Sort).GetValue(p)
            //                 select p).Skip(skipNum).Take(pageWithSortDto.PageSize).ToList();
            var products = _productRepository.GetQueryable().Where(p => p.BelongTypeName == blongTypeName
                      && (p.TypeNo == productType || string.IsNullOrEmpty(productType))
                      && (p.ProductName.Contains(searchText) || string.IsNullOrEmpty(searchText))
                      && (bzgg == null || p.ProductBzgg == bzgg)
                      && (cd == null || p.ProductCd == cd)
                      && (pp == null || p.ProductPp == pp)
                      && (xh == null || p.ProductXh == xh)
                      && (cz == null || p.ProductCz == cz)
                      && (hb == null || p.ProductHb == hb)
                      && (hd == null || p.ProductHd == hd)
                      && (gy == null || p.ProductGy == gy)
                      && (hs == null || p.ProductHs == hs)
                      && (mc == null || p.ProductMc == mc)
                      && (dj == null || p.ProductDj == dj)
                      && (gg == null || p.ProductGg == gg)
                      && (ys == null || p.ProductYs == ys)
                      )//借助System.Linq.Dynamic， IQueryable根据排序字符串排序
                      .OrderBy(pageWithSortDto.Sort).Skip(skipNum).Take(pageWithSortDto.PageSize).ToList();
            //var products = (await _productRepository.GetListAsync()).OrderBy(m => m.ProductName).Skip(skipNum).Take(pageSize);
            var productDtos = _mapper.Map<List<ProductDto>>(products);
            var productPhotos = await GetProductPhotosByProductNo(productDtos.Select(m => m.ProductNo).ToArray());
            var productSales = await GetProductSalesByProductNo(productDtos.Select(m => m.ProductNo).ToArray());
            productDtos.ForEach(p =>
            {
                p.ProductPhoto = productPhotos.FirstOrDefault(m => m.ProductNo == p.ProductNo);
                p.ProductSale = productSales.FirstOrDefault(m => m.ProductNo == p.ProductNo);
                p.ProductPhoto ??= new ProductPhoto { ProductPhotoUrl = "/img/productPhotos/1.jpg" };
                p.ProductSale ??= new ProductSale { SalePrice = 0.00 };
            });
            return productDtos;
        }
        /// <summary>
        /// 获取商品类型名称(五金,板材,地板 BelongTypeName)
        /// </summary>
        /// <returns></returns>
        public async Task<List<BlongTypeDto>> GetBlongTypeDtosAsync()
        {
            return await Task.Run(() =>
              {
                  var blongType = _productRepository.GetQueryable().Select(m => new BlongTypeDto
                  {
                      BelongTypeName = m.BelongTypeName
                  }).Distinct().ToList();
                  return blongType;
              });
        }
        /// <summary>
        /// 获取商品类型(TypeName) 前端左侧边栏显示的类型
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductTypeDto>> GetProductTypeDtosAsync(string belongTypeName)
        {
            return await Task.Run(() =>
            {
                var type = _productRepository.GetQueryable().Where(m => m.BelongTypeName == belongTypeName && !string.IsNullOrWhiteSpace(m.BelongTypeName) && !string.IsNullOrWhiteSpace(m.TypeNo))
                .Select(m => new ProductTypeDto
                {
                    TypeNo = m.TypeNo,
                    TypeName = m.TypeName
                }).Distinct().ToList();
                return type;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="belongTypeName">商品大类</param>
        /// <param name="typeNo">商品类型左侧边栏</param>
        /// <returns></returns>
        public async Task<Dictionary<string, List<string>>> GetProductPropsAsync(string belongTypeName, string typeNo)
        {
            typeNo ??= "";
            Dictionary<string, List<string>> productProps = new();
            var products = await _productRepository.GetListAsync(m => m.BelongTypeName == belongTypeName && (m.TypeNo.ToUpper() == typeNo.ToUpper() || string.IsNullOrWhiteSpace(typeNo)));
            productProps.Add("ProductBzgg|包装规格", products.Select(m => m.ProductBzgg).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            productProps.Add("ProductCd|产地", products.Select(m => m.ProductCd).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            productProps.Add("ProductCz|材质", products.Select(m => m.ProductCz).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            productProps.Add("ProductDj|等级", products.Select(m => m.ProductDj).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            productProps.Add("ProductGg|规格", products.Select(m => m.ProductGg).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            productProps.Add("ProductGy|工艺", products.Select(m => m.ProductGy).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            productProps.Add("ProductHb|环保", products.Select(m => m.ProductHb).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            productProps.Add("ProductHd|厚度", products.Select(m => m.ProductHd).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            productProps.Add("ProductHs|花色", products.Select(m => m.ProductHs).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            productProps.Add("ProductMc|面材", products.Select(m => m.ProductMc).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            productProps.Add("ProductPp|品牌", products.Select(m => m.ProductPp).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            productProps.Add("ProductXh|型号", products.Select(m => m.ProductXh).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            productProps.Add("ProductYs|颜色", products.Select(m => m.ProductYs).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            return productProps;
        }
    }
}