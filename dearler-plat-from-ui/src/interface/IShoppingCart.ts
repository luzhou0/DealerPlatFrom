import { IProduct } from "./IProduct";

export interface IShoppingCartInputDto {
    customerNo: string;
    productNo: string;
    productNum: number;
}
export interface IShoppingCartDto {
    id: number;
    cartGuid: string;
    customerNo: string;
    productNo: string;
    productNum: number;
    cartSelected: boolean;
    productDto: IProduct;

}
export interface IShoppingCartType {
    TypeNo: string;
    TypeName: string;
    TypeSelected: boolean;
}
export interface IShoppingCartInfo {
    shoppingCarts: IShoppingCartDto[];
    shoppingCartType: IShoppingCartType[],
    isAllSelected:boolean;
    getShoppingCart(): void;
    onSelectCart(cart: IShoppingCartDto): void;
    onSelectType(type: IShoppingCartType): void;
    onSelectAll():void;
    checkTypeSelected(): void;
    checkAllSelected():void;   
}
