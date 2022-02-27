<template>
  <div class="login-pad">
    <h2>
      <img src="/img/icons/Login.jpg" />
      经销商平台
    </h2>
    <p>
      <input type="text" placeholder="用户名" v-model="LoginInfo.CustomerNo" @blur="strToupper" />
    </p>
    <p>
      <input type="password" placeholder="密码" v-model="LoginInfo.PassWord" />
    </p>
    <button @click="submitLogin">→</button>
  </div>
</template>

<script lang="ts">
import { reactive } from "vue";
import { ILoginInfo } from "../interface/ILoginInfo";
import { Login } from "../api/LoginApi";
import { ElMessage } from "element-plus";
import { useRouter, Router } from "vue-router";
export default {
  setup() {
    //登录信息
    const LoginInfo: ILoginInfo = reactive({
      CustomerNo: "",
      PassWord: ""
    });
    //获取所有路由信息
    const $router: Router = useRouter();
    //token信息
    let token: string = "";
    //用户名转大写
    const strToupper = ():string|undefined => {
      if (LoginInfo.CustomerNo != "") {
        LoginInfo.CustomerNo = LoginInfo.CustomerNo.toUpperCase();
        return LoginInfo.CustomerNo;
      }
    };
    //提交登录信息
    const submitLogin = async (): Promise<void> => {
      if (LoginInfo.CustomerNo == "") {
        ElMessage.error("用户名不能为空!");
        return;
      }
      if (LoginInfo.PassWord == "") {
        ElMessage.error("密码不能为空!");
        return;
      }
      token = await Login(LoginInfo);
      if (token != "NoLoginInfo" && token != "NoUserInfo") {
        localStorage["CustomerNo"] = LoginInfo.CustomerNo
        $router.push("/productList");
      }
    };
    return { LoginInfo, submitLogin, strToupper};
  }
};
</script>

<style lang="scss" scoped>
.login-pad {
  text-align: center;
  width: 60%;
  margin: auto;
  margin-top: 25%;
  h2 {
    font-weight: normal;
    margin-bottom: 30px;
    img {
      width: 36px;
      height: 36px;
      vertical-align: -10px;
      background-color: transparent;
      display: inline-block;
    }
  }
  p {
    width: 100%;
    margin-top: 20px;
    input {
      width: 100%;
      background-color: transparent;
      border: 0 none;
      text-align: center;
      height: 36px;
    }
  }
  button {
    border: 0 none;
    margin-top: 36px;
    width: 60px;
    height: 60px;
    border-radius: 30px;
    color: white;
    font-size: 26px;
    outline: none;
    background-color: rgb(55, 121, 243);
  }
}
</style>