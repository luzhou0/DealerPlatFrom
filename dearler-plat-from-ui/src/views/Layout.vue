<template>
  <div>
    <!-- 展示子页面内容 -->
    <router-view :key="route.fullPath"></router-view>
    <!-- 展示底部菜单 -->
    <div class="foot-menu-pad">
      <div class="foot-menu">
        <!-- <router-link to=""></router-link> -->
        <router-link to="/productList" class="foot-item">
          <b :class="['i-search',{'menu-sel':activeIndex==1}]"></b>
        </router-link>
        <router-link to="/shoppingCart" class="foot-item">
          <b :class="['i-cart',{'menu-sel':activeIndex==2}]">
            <i>10</i>
          </b>
        </router-link>
        <router-link to="/main" class="foot-item">
          <b :class="['i-user',{'menu-sel':activeIndex==3}]"></b>
        </router-link>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { activeIndex } from "../globalValue/index";
import { useRoute } from "vue-router";
export default {
  setup() {
    /**
     * Vue中给router-view 设置key的原因（$route-fullPath）
     */
    /**从/page/1  =>>>  /page/2, 由于这两个路由的$route.fullPath并不一样, 所以组件被强制不复用, 相关钩子加载顺序为:
       beforeRouteUpdate => created => mounted
       从/page?id=1  =>>>  /page?id=2, 由于这两个路由的$route.fullPath并不一样, 所以组件被强制不复用, 相关钩子加载顺序为:
       beforeRouteUpdate => created => mounted（通过绑定一个fullPath，可以识别当前页面路由的完整地址，当地址发生改变(包括参数改变)则重新渲染页面(例如动态路由参数的变化）
       原文链接：https://blog.csdn.net/lf811/article/details/117967027 */
    const route = useRoute();
    return { activeIndex, route };
  }
};
</script>

<style lang="scss" scoped>
.foot-menu-pad {
  height: 40px;
  .foot-menu {
    height: 40px;
    background-color: #ffffff;
    width: 100%;
    left: 0;
    bottom: 0;
    position: fixed;
    display: flex;
    .foot-item {
      flex: 1;
      height: 40px;
      line-height: 40px;
      b {
        background-color: #acacac;
        width: 26px;
        height: 26px;
        display: inline-block;
        border-radius: 13px;
        background-position: center;
        background-repeat: no-repeat;
        margin-top: 6px;
        i {
          position: relative;
          top: -15px;
          right: -15px;
          font-size: 12px;
          background-color: red;
          color: white;
          border-radius: 12px;
          font-weight: normal;
          font-style: normal;
          padding: 1px 3px;
        }
      }
      .i-search {
        background-image: url("/img/icons-png/search-white.png");
      }
      .i-cart {
        background-image: url("/img/icons-png/shoppingCar-white.png");
      }
      .i-user {
        background-image: url("/img/icons-png/user-white.png");
      }
      .menu-sel {
        background-color: rgb(201, 39, 39);
      }
    }
  }
}
</style>