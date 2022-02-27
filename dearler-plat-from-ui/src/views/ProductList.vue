<template>
  <div>
    <div class="search-pad">
      <input type="text" v-model="searchText" @input="search(searchText)" />
      <button @click="showRight()">筛选</button>
    </div>
    <div class="system-pad">
      <div
        :class="['system-item',{'system-select':selectTypeName==blongType.belongTypeName}]"
        v-for="blongType in blongTypes"
        :key="blongType.belongTypeName"
        v-on:click="selectBlongType(blongType.belongTypeName)"
      >
        <span>{{blongType.belongTypeName}}</span>
      </div>
    </div>
    <div class="producr-list">
      <ul>
        <li v-for="product in products" :key="product.Id">
          <img :src="product.productPhoto.productPhotoUrl" />
          <div>
            <p class="p-name">{{product.productName}}</p>
            <p class="p-type">类别:{{product.typeName}}</p>
            <p class="p-price">&yen;{{product.productSale.salePrice}}/张</p>
            <span class="p-add-cart" @click="onAddCart(product.productNo,1)"></span>
          </div>
        </li>
      </ul>
      <div :class="['left-menu',{'left-menu-show':isShowLeft}]">
        <div class="left-switch" @click="showLeft">
          <img src="/img/dealerImgs/up.png" />
        </div>
        <ul>
          <li
            v-for="producttype in productTypes"
            :key="producttype.typeNo"
            :class="{'left-item-select':typeSelected==producttype.typeNo}"
            v-on:click="selectType(producttype.typeNo)"
          >{{producttype.typeName}}</li>
        </ul>
      </div>
    </div>
    <!-- 右边物品属性面板 -->
    <div class="right-pad">
      <div class="list-pad">
        <ul class="f-type-list">
          <template v-for="(values,key) in productProps">
            <li v-if="values.length>0" :key="key">
              <p>{{getPropKey(key,1)}}</p>
              <ul class="f-item-list">
                <li
                  v-for="value in values"
                  :key="value"
                  @click="selectProp(getPropKey(key,0),value)"
                >
                  <span :class="{'prop-select':propSelect[getPropKey(key,0)]==value}">{{value}}</span>
                </li>
              </ul>
              <div class="clear-tag"></div>
            </li>
          </template>
        </ul>
      </div>
      <div class="right-edit">
        <button style="background-color:rgb(188,0,0); color:#fff" @click="confirmFilter">确定</button>
        <button @click="hideRight()">取消</button>
      </div>
    </div>
    <div class="cover" v-show="isShowRight" @click="hideRight()"></div>
  </div>
</template>

<script lang="ts">
import { onMounted, reactive, toRefs } from "vue";
import { activeIndex } from "../globalValue/index";
import {
  getProduct,
  getBlongType,
  getProductType,
  getProductProp
} from "../api/ProductApi";
import {
  IProductInfo,
  IProduct,
  IBlongType,
  IProductType,
  IPageController
} from "../interface/IProduct";
import { addShoppingCart } from "../api/ShoppingCartApi";
import { ElMessage } from "element-plus";
import { useRouter, useRoute } from "vue-router";
export default {
  setup() {
    const router = useRouter();
    const route = useRoute();
    onMounted(async () => {
      activeIndex.value = 1;
      window.addEventListener("scroll", handleScroll);
      resolutionAddress();
      productInfo.getProducts(
        selectTypeName,
        productType,
        keywords,
        productProps,
        productInfo.pageIndex
      );
      productInfo.getBlongTypes();
      productInfo.getProductTypes(productInfo.selectTypeName);
      productInfo.getProductProps(
        productInfo.selectTypeName,
        productInfo.typeSelected
      );
    });
    const pageController: IPageController = reactive({
      isShowLeft: false,
      isShowRight: false,
      //是否显示左侧边栏
      showLeft: () => {
        pageController.isShowLeft = !pageController.isShowLeft;
      },
      //显示右侧边栏
      showRight: () => {
        pageController.isShowRight = true;
        var dom = document.querySelector(".right-pad") as HTMLElement;
        dom.style.right = "0";
      },
      //隐藏右侧边栏
      hideRight: () => {
        pageController.isShowRight = false;
        var dom = document.querySelector(".right-pad") as HTMLElement;
        dom.style.right = "-85%";
      }
    });
    const productInfo: IProductInfo = reactive({
      timer: 0,
      searchText: "",
      propSelect: {},
      selectTypeName: "板材",
      typeSelected: "",
      products: [],
      blongTypes: [],
      productTypes: [],
      productProps: [],
      pageIndex: 1,
      //获取商品信息(分页)
      getProducts: async (
        blongTypeName: string,
        productType: string | null,
        searchText: string | null,
        productProps: string | null,
        pageIndex: number
      ) => {
        productInfo.products = (await getProduct({
          searchText,
          productType,
          blongTypeName,
          productProps,
          sort: "ProductName",
          pageIndex: productInfo.pageIndex
        })) as IProduct[];
      },
      //获取商品类型名称
      getBlongTypes: async () => {
        //筛选框下面(五金/地板/板材)
        productInfo.blongTypes = (await getBlongType()) as IBlongType[];
      },
      getProductTypes: async (blongTypeName: string) => {
        //左侧边栏数据
        productInfo.productTypes = (await getProductType(
          blongTypeName
        )) as IProductType[];
      },
      //右侧边栏数据
      getProductProps: async (
        belongTypeName: string,
        typeNo: string | null
      ) => {
        productInfo.productProps = await getProductProp({
          belongTypeName,
          typeNo
        });
      },
      /**
       * 点击大类时我们不需要考虑搜索的内容，因为每次点击大类，都应该清空搜索框
       * 但是所搜物品时就应该考虑大类
       */
      selectBlongType: async (blongTypeName: string) => {
        productInfo.propSelect = {};
        productInfo.searchText = "";
        productInfo.typeSelected = "";
        router.push(`/productList?belongtype=${blongTypeName}`);
      },
      /**
       * 选择物品类型，选择物品类型时可以清空搜索栏 左侧边栏
       */
      selectType: async (typeNo: string) => {
        productInfo.propSelect = {};
        productInfo.searchText = "";
        if (productInfo.typeSelected != typeNo) {
          productInfo.typeSelected = typeNo;
        } else {
          productInfo.typeSelected = "";
        }
        setRouter();
      },
      //筛选
      search: () => {
        clearTimeout(productInfo.timer);
        productInfo.timer = setTimeout(() => {
          setRouter();
        }, 1000);
      },
      /**
       * 获取物品属性种类的名称
       */
      getPropKey: (key: string, index: number): string => {
        return key.split("|")[index];
      },
      /**
       * 选择属性
       */
      selectProp: (propKey: string, propValue: string) => {
        if (productInfo.propSelect[propKey] == propValue) {
          productInfo.propSelect[propKey] == "";
        } else {
          productInfo.propSelect[propKey] = propValue;
        }
      },
      /**
       * 确定筛选
       */
      confirmFilter: () => {
        setRouter();
      },
      /**
       * 添加购物车
       */
      onAddCart: async (productNo: string, productNum: number) => {
        const customerNo = localStorage["CustomerNo"];
        var res = await addShoppingCart({
          customerNo,
          productNo,
          productNum
        });
        if (res != null) {
          ElMessage.success("添加购物车成功!");
        }else{
          ElMessage.error("添加购物车失败!");
        }
      }
    });
    //搜索信息
    let keywords: string = "";
    //商品大类
    let selectTypeName: string = "";
    //左侧边栏类型
    let productType: string = "";
    //右侧边栏属性
    let productProps: string = "";
    /**
     * 讲选中的属性转换成字符串
     */
    const productPropToString = () => {
      //属性拼接字符串清空
      productProps = "";
      for (const key in productInfo.propSelect) {
        const value = productInfo.propSelect[key];
        if (value != "") {
          productProps += `${key}_${value}^`;
        }
      }
      productProps = productProps.substring(0, productProps.length - 1);
    };

    /**
     * 设置路由
     */
    const setRouter = (): void => {
      // 根地址,包含跟路由及物品大类信息
      var url = `/productList?belongtype=${productInfo.selectTypeName}`;
      // 拼接物品搜索信息
      if (productInfo.searchText?.trim() != "") {
        url += `&keywords=${productInfo.searchText}`;
      }
      // 拼接物品类型
      if (productInfo.typeSelected?.trim() != "") {
        url += `&type=${productInfo.typeSelected}`;
      }
      // 拼接属性
      productPropToString();
      if (productProps != "") {
        url += `&prop=${productProps}`;
      }
      router.push(url);
    };
    /**
     * 解析地址
     */
    const resolutionAddress = () => {
      productInfo.searchText = keywords =
        (route.query.keywords as string) ?? "";
      productInfo.selectTypeName = selectTypeName =
        (route.query.belongtype as string) ?? "板材";
      productInfo.typeSelected = productType =
        (route.query.type as string) ?? "";
      productProps = (route.query.prop as string) ?? "";
      //格式大约是: &prop=xxx_xxx1||xxx_xxx2^yyy_yyy
      if (productProps != "") {
        let arrayProductProps = productProps.split("^");
        for (let i = 0; i < arrayProductProps.length; i++) {
          const element: string = arrayProductProps[i];
          productInfo.propSelect[element.split("_")[0]] = element.split("_")[1];
        }
      }
    };
    /**
     监听页面的滚动事件
     */
    const handleScroll = () => {
      var htmlDom = document.querySelector("html") as HTMLElement;
      // 1 获取当前整个页面长度
      var htmlHeight = htmlDom.offsetHeight;
      // 2 获取滚动条距离顶部的距离
      var scrollTop = htmlDom.scrollTop;
      // 3 获得屏幕可视区域的高度
      var screenHeight = document.documentElement.clientHeight;
      // 4 获取可视区域底部到整个页面底部的距离
      var diffHeight = htmlHeight - scrollTop - screenHeight;
      if (diffHeight == 0 && scrollTop > 0) {
        productInfo.pageIndex++;
        console.log(productInfo.pageIndex);
        productInfo.getProducts(
          selectTypeName,
          productType,
          keywords,
          productProps,
          productInfo.pageIndex
        );
      }
    };
    return { ...toRefs(productInfo), ...toRefs(pageController) };
  }
};
</script>

<style lang="scss" scoped>
.search-pad {
  padding: 6px 20px;
  background-color: #f0f0f0;
  display: flex;
  position: fixed;
  width: 100%;
  z-index: 5;
  input {
    height: 28px;
    border: 1px solid #ddd;
    border-radius: 3px;
    flex: 1;
    outline: none;
    padding: 0 6px;
  }
  button {
    background-color: transparent;
    border: 0 none;
    width: 56px;
    font-size: 14px;
    font-weight: bold;
    outline: none;
  }
}
.system-pad {
  display: flex;
  position: fixed;
  top: 40px;
  width: 100%;
  z-index: 5;
  background-color: #f0f0f0;
  .system-item {
    flex: 1;
    text-align: center;
    border-bottom: 1px solid #ddd;
    border-right: 1px transparent solid;
    border-left: 1px transparent solid;
    span {
      border: 0 none;
      background-color: #e4e3e3;
      margin: 6px 5px;
      font-size: 12px;
      padding: 6px 0;
      display: block;
      height: 26px;
      border-radius: 4px;
    }
  }
  .system-select {
    border-bottom: 1px solid transparent;
    border-right: 1px #ddd solid;
    border-left: 1px #ddd solid;
    span {
      background-color: transparent;
    }
  }
}
.producr-list {
  padding-top: 79px;
  ul {
    background-color: #f0f0f0;
    li {
      list-style: none;
      position: relative;
      height: 86px;
      padding-left: 110px;
      img {
        width: 66px;
        height: 66px;
        position: absolute;
        left: 28px;
        top: 10px;
      }
      div {
        padding: 10px 0;
        border-bottom: 1px solid #f0f0f0;
        text-align: left;
        position: relative;
        p {
          font-size: 13px;
        }
        .p-type {
          font-size: 12px;
          margin-top: 8px;
        }
        .p-price {
          margin-top: 8px;
          color: red;
        }
        .p-add-cart {
          display: inline-block;
          background-color: crimson;
          width: 38px;
          height: 20px;
          border-radius: 20px;
          background-image: url("/img/dealerImgs/shopping212white.png");
          background-repeat: no-repeat;
          background-position: center;
          background-size: 20px;
          position: absolute;
          right: 10px;
          bottom: 10px;
        }
      }
    }
  }
  .left-menu {
    position: fixed;
    height: 100%;
    left: -105px;
    width: 125px;
    background-color: #fff;
    top: 79px;
    border-radius: 0 18px 0 0;
    border: 1px solid #d7d7d7;
    overflow: hidden;
    transition: 0.5s;
    .left-switch {
      width: 20px;
      position: absolute;
      right: 0;
      height: 100%;
      img {
        width: 20px;
        position: absolute;
        top: 40%;
        transform: rotate(90deg);
        right: 0px;
        transition: 0.5s;
      }
    }
    ul {
      position: absolute;
      height: 100%;
      width: 105px;
      background-color: #f8f8f8;
      overflow: auto;
      li {
        width: 105px;
        height: 50px;
        text-align: center;
        line-height: 50px;
        border-bottom: 1px solid #d7d7d7;
        font-size: 12px;
        color: #333;
        padding: 0;
      }
      li.left-item-select {
        background-color: #fff;
      }
    }
  }
  .left-menu-show {
    left: 0;
    .left-switch {
      img {
        transform: rotate(-90deg);
      }
    }
  }
}
.right-pad {
  position: fixed;
  /* right: -85%; */
  right: -85%;
  top: 0;
  width: 85%;
  height: 100%;
  background-color: #f7f7f7;
  z-index: 103;
  transition: 580ms;
  z-index: 101;

  ul {
    list-style: none;
    overflow: hidden;
  }
  .list-pad {
    overflow: auto;
    height: 100%;
    padding-bottom: 40px;
    .f-type-list {
      overflow: hidden;

      > li {
        padding: 10px;
        background-color: #fff;
        margin-bottom: 10px;

        .f-item-list {
          overflow: hidden;
          display: flex;
          flex-wrap: wrap;

          li {
            flex-basis: 33.3%;

            span {
              display: block;
              margin-top: 10px;
              margin-right: 10px;
              background: #eee;
              border: 1px solid #eee;
              padding: 5px 0;
              text-align: center;
              border-radius: 6px;
              font-size: 13px;
              overflow: hidden;
              height: 29px;
              line-height: 22px;
            }

            .prop-select {
              border: 1px solid red;
              background: #fff;
              color: red;
            }
          }
        }

        p {
          font-size: 14px;
        }
      }
    }
  }
  .right-edit {
    position: absolute;
    bottom: 0;
    left: 0;
    width: 100%;

    button {
      float: left;
      height: 40px;
      width: 50%;
      line-height: 40px;
      text-align: center;
      border: 0px none;
    }
  }
}
.cover {
  position: fixed;
  z-index: 11;
  height: 100%;
  width: 100%;
  left: 0;
  top: 0;
  background-color: rgba(51, 51, 51, 0.36);
}
</style>