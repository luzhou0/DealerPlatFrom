import { $http } from '@/http';
import { IShoppingCartInputDto } from './../interface/IShoppingCart';


// 添加购物车
export const addShoppingCart = async (data: IShoppingCartInputDto) => {
    var res = await $http.post("/ShoppingCart/SetShoppingCart", data);
    return res.data;
}

//查询购物车信息
export const getShoppingCarts = async (customerNo:string) => {
    var res = await $http.get("/ShoppingCart/GetShoppingCarts",{params:{customerNo}});
    return res.data;
}