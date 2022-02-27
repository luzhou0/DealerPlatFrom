import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router'
import Login from "../views/Login.vue";
import Layout from "../views/Layout.vue";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "Login",
    component: Login,
  },
  {
    path: "/layout",
    name: "Layout",
    component: Layout,
    children: [
      {
        path: "/main",
        name: "Main",
        component: () =>
          import(/* webpackChunkName: "main" */ "../views/Main.vue"),
      }, {
        path: "/productList",
        name: "ProductList",
        component: () =>
          import(/* webpackChunkName: "productList" */ "../views/ProductList.vue"),
      },
      {
        path: "/shoppingCart",
        name: "ShoppingCart",
        component: () =>
          import(/* webpackChunkName: "shoppingCart" */ "../views/ShoppingCart.vue"),
      },
    ]
  },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
