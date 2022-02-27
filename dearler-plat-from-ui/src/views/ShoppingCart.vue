<template>
  <div>
    <div class="cart-list">
      <ul>
        <li v-for="type in shoppingCartType" :key="type.TypeNo">
          <p>
            <i :class="{ 'cart-select': type.TypeSelected}" @click="onSelectType(type)">✔</i>
            <span>{{(type.TypeName) }}</span>
          </p>
          <template v-for="cart in shoppingCarts">
            <div v-if="cart.productDto.typeNo==type.TypeNo" :key="cart.id">
              <i :class="{ 'cart-select': cart.cartSelected }" @click="onSelectCart(cart)">✔</i>
              <img :src="cart.productDto.productPhoto.productPhotoUrl"/>
              <p class="p-name">{{cart.productDto.productName}}</p>
              <p class="p-price">&yen;{{cart.productDto.productSale.salePrice}}</p>
              <p class="p-num">
                <span class="sub-num">-</span>
                <input type="text" />
                <span class="add0num">+</span>
                <b>块</b>
              </p>
            </div>
          </template>
        </li>
      </ul>
    </div>
    <div class="total-pad">
      <i :class="{ 'cart-select':isAllSelected }" @click="onSelectAll">✔</i>
      <span>全选</span>
      <span>
        合计：&yen;
        <b></b>
      </span>
      <button>确定下单</button>
    </div>
  </div>
</template>

<script lang="ts">
import { onMounted, reactive, toRefs } from "vue";
import { activeIndex } from "@/globalValue";
import {
  IShoppingCartInfo,
  IShoppingCartDto,
  IShoppingCartType
} from "@/interface/IShoppingCart";
import { getShoppingCarts } from "@/api/ShoppingCartApi";
export default {
  setup() {
    onMounted(async () => {
      activeIndex.value = 2;
      ShoppingCartInfo.getShoppingCart();
    });
    const ShoppingCartInfo: IShoppingCartInfo = reactive({
      shoppingCarts: [],
      shoppingCartType: [],
      isAllSelected: false,
      /**
       * 获取购物车信息
       */
      getShoppingCart: async () => {
        const customerNo = localStorage["CustomerNo"];
        const dicCart = await getShoppingCarts(customerNo);
        for (const key in dicCart) {
          var type = JSON.parse(key);
          ShoppingCartInfo.shoppingCartType.push(type);
          for (let index = 0; index < dicCart[key].length; index++) {
            ShoppingCartInfo.shoppingCarts.push(dicCart[key][index]);
          }
        }
      },
      /**
       * 产品选中
       */
      onSelectCart: (cart: IShoppingCartDto) => {
        cart.cartSelected = !cart.cartSelected;
        ShoppingCartInfo.checkTypeSelected();
      },
      /**
       * 产品类型选中
       */
      onSelectType: (type: IShoppingCartType) => {
        type.TypeSelected = !type.TypeSelected;
        ShoppingCartInfo.shoppingCarts
          .filter(m => m.productDto.typeNo == type.TypeNo)
          .forEach(s => {
            s.cartSelected = type.TypeSelected;
          });
        ShoppingCartInfo.checkTypeSelected();
      },
      /**
       * 全选
       */
      onSelectAll: () => {
        ShoppingCartInfo.isAllSelected = !ShoppingCartInfo.isAllSelected;
        ShoppingCartInfo.shoppingCarts.forEach(
          m => (m.cartSelected = ShoppingCartInfo.isAllSelected)
        );
        ShoppingCartInfo.checkTypeSelected();
      },
      /**
       * 查看当前类型下的物品是否都被选择，是则将属于类型选中 否则不选中
       */
      checkTypeSelected: () => {
        ShoppingCartInfo.shoppingCartType.forEach(type => {
          const shoppingCarts = ShoppingCartInfo.shoppingCarts.filter(
            m => m.productDto.typeNo == type.TypeNo
          );
          if (shoppingCarts.every(m => m.cartSelected == true)) {
            type.TypeSelected = true;
          } else {
            type.TypeSelected = false;
          }
          ShoppingCartInfo.checkAllSelected();
        });
      },
      /**
       * 所有商品被选中则全选
       */
      checkAllSelected: () => {
        if (ShoppingCartInfo.shoppingCarts.every(m => m.cartSelected == true)) {
          ShoppingCartInfo.isAllSelected = true;
        } else {
          ShoppingCartInfo.isAllSelected = false;
        }
      }
    });
    return { ...toRefs(ShoppingCartInfo) };
  }
};
</script>

<style lang="scss" scoped>
.cart-list {
  text-align: left;
  ul {
    margin-bottom: 108px;
    li {
      background-color: #fff;
      margin-bottom: 12px;
      > p {
        padding-left: 46px;
        position: relative;
        height: 46px;
        border-bottom: 1px solid #ddd;
        i {
          border: 1px solid #a9a9a9;
          width: 18px;
          height: 18px;
          line-height: 18px;
          border-radius: 18px;
          position: absolute;
          left: 13px;
          top: 13px;
          text-align: center;
          font-size: 12px;
          color: #fff;
          font-style: normal;
        }
        i.cart-select {
          background-color: crimson;
          border: 1px solid crimson;
        }
        span {
          display: inline-block;
          border-left: 3px solid crimson;
          height: 28px;
          margin: 9px 0;
          padding-left: 8px;
          line-height: 30px;
        }
      }
      div {
        padding-left: 46px;
        position: relative;
        height: 98px;
        padding: 8px 14px 8px 148px;
        i {
          border: 1px solid #a9a9a9;
          width: 18px;
          height: 18px;
          line-height: 18px;
          border-radius: 18px;
          position: absolute;
          left: 13px;
          top: 28px;
          text-align: center;
          font-size: 12px;
          color: #fff;
          font-style: normal;
        }
        i.cart-select {
          background-color: crimson;
          border: 1px solid crimson;
        }

        img {
          width: 68px;
          height: 68px;
          background-color: #ccc;
          position: absolute;
          left: 58px;
          top: 20px;
        }

        p.p-name {
          font-size: 13px;
          margin-top: 10px;
          height: 30px;
        }
        p.p-price {
          font-size: 13px;
          height: 20px;
          color: crimson;
        }
        p.p-num {
          text-align: right;
          padding-right: 20px;

          span {
            display: inline-block;
            width: 18px;
            height: 18px;
            border: 1px solid crimson;
            color: crimson;
            border-radius: 9px;
            text-align: center;
            line-height: 18px;
          }

          input {
            width: 28px;
            border: none 0px;
            outline: none;
            text-align: center;
          }

          b {
            font-weight: normal;
            margin-left: 10px;
            font-size: 13px;
          }
        }
      }
    }
  }
}

.total-pad {
  height: 58px;
  width: 100%;
  background-color: #383838;
  position: fixed;
  left: 0;
  bottom: 40px;
  i {
    display: inline-block;
    border: 1px solid #a9a9a9;
    width: 18px;
    height: 18px;
    line-height: 18px;
    border-radius: 18px;
    background-color: #fff;
    margin-left: 13px;
    margin-top: 20px;
    vertical-align: bottom;
    height: 18px;
    text-align: center;
    font-size: 12px;
    color: #fff;
    font-style: normal;
  }

  i.cart-select {
    background-color: crimson;
    border: 1px solid crimson;
  }

  span {
    color: #fff;
    margin-left: 6px;
    font-size: 15px;
    b {
      font-size: 15px;
    }
  }

  button {
    float: right;
    height: 58px;
    width: 120px;
    border: 0 none;
    background-color: #ddd;
    color: #aaa;
    font-size: 15px;
    font-weight: bold;
  }
}
</style>