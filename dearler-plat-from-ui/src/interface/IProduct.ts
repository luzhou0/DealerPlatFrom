//排序
export interface IProductInputDto {
    searchText:string|null;
    productType: string | null;
    blongTypeName: string;
    productProps: string | null,
    sort: string;
    pageIndex: number;
}
//商品属性
export interface IProductPropInputDto{
    belongTypeName:string;
    typeNo:string|null;
}

//产品信息
export interface IProduct {
    id: number;
    sysNo: string;
    productNo: string;
    productName: string;
    typeNo: string;
    typeName: string;
    productPp: string;
    productXh: string;
    productCz: string;
    productHb: string;
    productHd: string;
    productGy: string;
    productHs: string;
    productMc: string;
    productDj: string;
    productCd: string;
    productGg: string;
    productYs: string;
    unitNo: string;
    unitName: string;
    productNote: string;
    productBzgg: string;
    belongTypeNo: string;
    belongTypeName: string;
    productPhoto: IProductPhoto;
    productSale: IProductSale;
}
//商品图片
export interface IProductPhoto {
    id: number;
    sysNo: string | null;
    productNo: string;
    productPhotoUrl: string;
}
//商品价格
export interface IProductSale {
    id: number;
    sysNo: string;
    productNo: string;
    stockNo: string | null;
    salePrice: number;
}
//商品类型
export interface IBlongType {
    belongTypeName: string
}
//产品类型
export interface IProductType {
    typeNo: string;
    typeName: string;
}
export interface IProductInfo {
    timer:number;
    searchText:string|null;
    propSelect: any,
    selectTypeName: string;
    typeSelected: string;
    products: IProduct[];
    blongTypes: IBlongType[];
    productTypes: IProductType[];
    productProps:any;
    pageIndex:number;
    getProducts(blongTypeName: string, productType: string | null,searchText: string | null, productProps: string | null,pageIndex:number): void;
    getBlongTypes(): void;
    getProductTypes(blongTypeName: string): void;
    getProductProps(belongTypeName:string,typeNo:string|null):void;
    selectBlongType(blongTypeName: string): void;
    selectType(typeNo: string): void;
    search():void;
    getPropKey(key:string,index:number):string;
    selectProp(propKey: string, propValue: string):void;
    confirmFilter():void;
    onAddCart(productNo:string,productNum:number):void;
}
export interface IPageController {
    isShowLeft:boolean;
    isShowRight:boolean;
    showLeft():void;
    showRight():void;
    hideRight():void;
}