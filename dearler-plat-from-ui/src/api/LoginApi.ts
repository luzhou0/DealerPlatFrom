import { ILoginInfo } from "../interface/ILoginInfo"
import { $http } from "../http/index"
//登录信息提交
export const Login = async (data: ILoginInfo):Promise<string> => {
    return (await $http.post("/Login", data)).data;
}