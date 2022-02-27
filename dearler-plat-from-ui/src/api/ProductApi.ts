import { IProductPropInputDto } from './../interface/IProduct';
import { $http } from "../http/index"
import { IProductInputDto } from "../interface/IProduct"
//分页查询商品信息
export const getProduct = async (data: IProductInputDto) => {
    var res = await $http.get("/Product/GetProduct", { params: data });
    return res.data;
}
//获取商品类型(BelongTypeName)大类
export const getBlongType = async () => {
    var res = await $http.get("/Product/GetBlongType");
    return res.data;
}
//获取商品类型(TypeName)左侧边栏
export const getProductType = async (blongTypeName: string) => {
   var res = await $http.get("/Product/GetProductType?belongTypeName="+blongTypeName);
   return res.data;
}
//获取商品类型对应的属性 右侧边栏
export const getProductProp = async(data:IProductPropInputDto)=>{
    var res = await $http.get("/Product/GetProductProps",{params:data});
    return res.data;
}